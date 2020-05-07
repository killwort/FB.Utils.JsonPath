using System;
using System.Collections.Generic;
using System.Linq;
using FB.Utils.JsonPath.Language;
using Newtonsoft.Json.Linq;

namespace FB.Utils.JsonPath {
    public class EvaluatingVisitor : IVisitor {
        private List<JToken> _result;
        private Stack<List<JToken>> _context = new Stack<List<JToken>>();
        private JToken _root;
        private Dictionary<string, Func<IEnumerable<IEnumerable<JToken>>, IEnumerable<JToken>>> _functions = new Dictionary<string, Func<IEnumerable<IEnumerable<JToken>>, IEnumerable<JToken>>>();
        public EvaluatingVisitor(JToken root) { _root = root; }

        public IEnumerable<JToken> Result => _result ??= _context.Pop();

        public void VisitArithmetic(Arithmetic arithmetic) {
            if (arithmetic.Right == null) {
                arithmetic.Left.Visit(this);
            } else {
                var save = _context.Peek();
                arithmetic.Left.Visit(this);
                _context.Push(save);
                arithmetic.Right.Visit(this);
                var rhs = _context.Pop();
                var lhs = _context.Pop();
                var lhsVal = lhs.OfType<JValue>().First();
                var rhsVal = rhs.OfType<JValue>().First();
                JToken val;
                if (lhsVal.Type == JTokenType.Null)
                    val = JValue.CreateNull();
                else if (lhsVal.Type == JTokenType.Float || lhsVal.Type == JTokenType.Integer) {
                    switch (arithmetic.Operator) {
                        case ArithmeticOperator.Subtraction:
                            val = new JValue(lhsVal.Value<double>() - rhsVal.Value<double>());
                            break;
                        case ArithmeticOperator.Multiplication:
                            val = new JValue(lhsVal.Value<double>() * rhsVal.Value<double>());
                            break;
                        case ArithmeticOperator.Division:
                            val = new JValue(lhsVal.Value<double>() / rhsVal.Value<double>());
                            break;
                        case ArithmeticOperator.Modulus:
                            val = new JValue(lhsVal.Value<double>() % rhsVal.Value<double>());
                            break;
                        case ArithmeticOperator.Addition:
                        default:
                            val = new JValue(lhsVal.Value<double>() + rhsVal.Value<double>());
                            break;
                    }
                } else {
                    if (arithmetic.Operator == ArithmeticOperator.Addition)
                        val = new JValue(lhsVal.Value<string>() + rhsVal.Value<string>());
                    else
                        val = new JValue(double.NaN);
                }

                _context.Push(new List<JToken> {val});
            }
        }

        public void VisitArrayIndex(ArrayIndex arrayIndex) {
            var newItems = new List<JToken>();
            foreach (var ctx in _context.Pop()) {
                if (ctx is JArray a) {
                    newItems.AddRange(arrayIndex.Indexes.Select(i => i < a.Count ? a[i] : null).Where(x => x != null));
                }
            }

            _context.Push(newItems);
        }

        public void VisitArrayIteration(ArrayIteration arrayIteration) {
            var newItems = new List<JToken>();
            foreach (var ctx in _context.Pop()) {
                if (ctx is JArray a) {
                    newItems.AddRange(arrayIteration.Select(a));
                }
            }

            _context.Push(newItems);
        }

        public void VisitConstant<T>(Constant<T> constant) {
            _context.Pop();
            _context.Push(new List<JToken> {new JValue(constant.Value)});
        }

        public void VisitConjunction(Conjunction conjunction) {
            if (conjunction.Right == null) {
                conjunction.Left.Visit(this);
            } else {
                var save = _context.Peek();
                conjunction.Left.Visit(this);
                var lhs = _context.Pop();
                if (Truish(lhs.OfType<JValue>())) {
                    _context.Push(save);
                    conjunction.Right.Visit(this);
                    var rhs = _context.Pop();
                    if (Truish(rhs.OfType<JValue>())) {
                        _context.Push(new List<JToken> {new JValue(true)});
                        return;
                    }
                }

                _context.Push(new List<JToken> {new JValue(false)});
            }
        }

        public void VisitDescendantsPathElement(DescendantsPathElement descendantsPathElement) {
            var currentContext = _context.Pop();
            var descendants = new List<JToken>();
            var thisLevelChildren = currentContext;
            while (thisLevelChildren.Any()) {
                thisLevelChildren = thisLevelChildren.SelectMany(x => x is JObject o ? o.PropertyValues() : x is JArray a ? (IEnumerable<JToken>) a : Enumerable.Empty<JToken>()).ToList();
                descendants.AddRange(thisLevelChildren);
            }

            _context.Push(descendants);
            descendantsPathElement.Next?.Visit(this);
        }

        public void VisitDisjunction(Disjunction disjunction) {
            if (disjunction.Right == null) {
                disjunction.Left.Visit(this);
            } else {
                var save = _context.Peek();
                disjunction.Left.Visit(this);
                var lhs = _context.Pop();
                if (Truish(lhs.OfType<JValue>())) {
                    _context.Push(new List<JToken> {new JValue(true)});
                    return;
                }

                _context.Push(save);
                disjunction.Right.Visit(this);
                var rhs = _context.Pop();
                _context.Push(new List<JToken> {new JValue(Truish(rhs.OfType<JValue>()))});
            }
        }

        public void VisitFilterPredicate(FilterPredicate filterPredicate) {
            var rv = new List<JToken>();
            foreach (var ctx in _context.Pop()) {
                if (ctx is JArray a) {
                    foreach (var elem in a) {
                        _context.Push(new List<JToken> {elem});
                        filterPredicate.Predicate.Visit(this);
                        var result = _context.Pop();
                        if (result.Count != 0 && result.Any(Truish))
                            rv.Add(elem);
                    }
                } else {
                    _context.Push(new List<JToken> {ctx});
                    filterPredicate.Predicate.Visit(this);
                    var result = _context.Pop();
                    if (result.Count != 0 && result.Any(Truish))
                        rv.Add(ctx);
                }
            }

            _context.Push(rv);
        }

        private static bool Truish(IEnumerable<JToken> tokens) { return (tokens.Any(Truish)); }

        private static bool Truish(JToken token) {
            switch (token) {
                case null:
                    break;
                case JArray a:
                    if (a.Count > 0) return true;
                    break;
                case JValue val1 when val1.Type == JTokenType.Boolean:
                    if (val1.Value<bool>()) return true;
                    break;
                case JValue val2 when val2.Type == JTokenType.Array:
                    if (val2.ToArray().Length > 0) return true;
                    break;
                case JValue val3 when val3.Type == JTokenType.Float:
                    if ((int) val3.Value<float>() != 0) return true;
                    break;
                case JValue val4 when val4.Type == JTokenType.Integer:
                    if (val4.Value<int>() != 0) return true;
                    break;
                case JValue val5 when val5.Type == JTokenType.String:
                    if (val5.Value<string>() != "") return true;
                    break;
                case JObject o when o.Properties().Any():
                    return true;
            }

            return false;
        }

        public void VisitNameCheck(NameCheckElement nameCheckElement) {
            var currentTokens = _context.Pop();
            IEnumerable<JToken> potentialTokens;
            if (string.IsNullOrEmpty(nameCheckElement.Name))
                potentialTokens = currentTokens.OfType<JObject>().SelectMany(x => x.PropertyValues());
            else
                potentialTokens = currentTokens.OfType<JObject>().Select(x => x.Property(nameCheckElement.Name)).Where(x => x != null).Select(x => x.Value);
            _context.Push(potentialTokens.ToList());
            nameCheckElement.Predicate?.Visit(this);
            nameCheckElement.Next?.Visit(this);
        }

        public void VisitNullConstant(NullConstant nullConstant) {
            _context.Pop();
            _context.Push(new List<JToken> {JValue.CreateNull()});
        }

        public void VisitReference(Reference reference) { reference.Path.Visit(this); }

        public void VisitRelational(Relational relational) {
            if (relational.Right == null) {
                relational.Left.Visit(this);
            } else {
                var save = _context.Peek();
                relational.Left.Visit(this);
                _context.Push(save);
                relational.Right.Visit(this);
                var rhs = _context.Pop();
                var lhs = _context.Pop();
                var val = new JValue(lhs.OfType<JValue>().Any(l => rhs.OfType<JValue>().Any(r => CompareOutcome(l.CompareTo(r), relational.Operator))));
                _context.Push(new List<JToken> {val});
            }
        }

        private bool CompareOutcome(int compareTo, Relation relationalOperator) {
            switch (relationalOperator) {
                case Relation.Equal: return compareTo == 0;
                case Relation.NotEqual: return compareTo != 0;
                case Relation.Less: return compareTo < 0;
                case Relation.Greater: return compareTo > 0;
                case Relation.LessOrEqual: return compareTo <= 0;
                case Relation.GreaterOrEqual: return compareTo >= 0;
            }

            return false;
        }

        public void VisitRoot(RootElement rootElement) {
            _context.Push(new List<JToken>(new[] {_root}));
            rootElement.Next?.Visit(this);
        }

        public void VisitWildcard(WildcardPredicate wildcardPredicate) {
            var newItems = new List<JToken>();
            foreach (var ctx in _context.Pop()) {
                if (ctx is JArray a)
                    newItems.AddRange(a);
                else
                    newItems.Add(ctx);
            }

            _context.Push(newItems);
        }

        public void VisitUnion(UnionPredicate unionPredicate) {
            var save = _context.Pop();
            var rv = new List<JToken>();
            foreach (var p in unionPredicate.Items) {
                _context.Push(save);
                p.Visit(this);
                rv.AddRange(_context.Pop());
            }

            _context.Push(rv);
        }

        public void VisitMemberAccess(MemberAccess memberAccess) { memberAccess.Member.Visit(this); }

        public void VisitTypeCheck(TypeCheckElement typeCheckElement) {
            _context.Push(_context.Pop().Where(x => x.Type == typeCheckElement.Type).ToList());
            typeCheckElement.Predicate?.Visit(this);
            typeCheckElement.Next?.Visit(this);
        }

        public void VisitFunctionCall(FunctionCall functionCall) {
            List<JToken> save;
            if (_context.Count > 0)
                save = _context.Peek();
            else
                save = new List<JToken>();
            var argv = new List<List<JToken>>();
            foreach (var argument in functionCall.Arguments) {
                _context.Push(save);
                argument.Visit(this);
                argv.Add(_context.Pop());
            }

            _context.Push(_functions[functionCall.Function](argv).ToList());
        }

        public void VistExpressionArgument(ExpressionArgument expressionArgument) { expressionArgument.Expression.Visit(this); }

        public void VisitPredicativeArgument(PredicativeArgument predicativeArgument) { predicativeArgument.Predicate.Visit(this); }

        public void RegisterFunction(string name, Func<IEnumerable<IEnumerable<JToken>>, IEnumerable<JToken>> func) { _functions[name] = func; }
    }
}
