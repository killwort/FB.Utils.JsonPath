using System.Text;
using FB.Utils.JsonPath.Language;

namespace FB.Utils.JsonPath {
    public class StringBuilderVisitor : IVisitor {
        private StringBuilder _builder = new StringBuilder();
        public string Result => _builder.ToString();

        private static readonly string[] ArithmeticOperators = {"+", "-", "*", "/", "%"};
        public void VisitArithmetic(Arithmetic arithmetic) {
            arithmetic.Left.Visit(this);
            if (arithmetic.Right != null) {
                _builder.Append(" ").Append(ArithmeticOperators[(int)arithmetic.Operator]).Append(" ");
                arithmetic.Right.Visit(this);
            }
        }

        public void VisitArrayIndex(ArrayIndex arrayIndex) {
            _builder.Append(string.Join(", ", arrayIndex.Indexes));
        }

        public void VisitArrayIteration(ArrayIteration arrayIteration) {
            _builder.Append(arrayIteration.Start).Append(":").Append(arrayIteration.End);
            if (arrayIteration.Step.HasValue) _builder.Append(":").Append(arrayIteration.Step);
        }

        public void VisitConstant<T>(Constant<T> constant) {
            if (typeof(T) == typeof(string))
                _builder.Append("\"").Append(constant.Value).Append("\"");
            else
                _builder.Append(constant.Value);
        }

        public void VisitConjunction(Conjunction conjunction) {
            conjunction.Left.Visit(this);
            if (conjunction.Right != null) {
                _builder.Append(" && ");
                conjunction.Right.Visit(this);
            }
        }

        public void VisitDescendantsPathElement(DescendantsPathElement descendantsPathElement) {
            //_builder.Append("..");
            VisitPathElement(descendantsPathElement);
        }

        private void VisitPathElement(PathElement pathElement) {
            if (pathElement.Next != null) {
                _builder.Append(".");
                pathElement.Next.Visit(this);
            }
        }

        public void VisitDisjunction(Disjunction disjunction) {
            disjunction.Left.Visit(this);
            if (disjunction.Right != null) {
                _builder.Append(" || ");
                disjunction.Right.Visit(this);
            }
        }

        public void VistExpressionArgument(ExpressionArgument expressionArgument) { expressionArgument.Expression.Visit(this); }

        public void VisitPredicativeArgument(PredicativeArgument predicativeArgument) { predicativeArgument.Predicate.Visit(this); }

        public void VisitFilterPredicate(FilterPredicate filterPredicate) {
            _builder.Append("?(");
            filterPredicate.Predicate.Visit(this);
            _builder.Append(")");
        }

        public void VisitFunctionCall(FunctionCall functionCall) {
            _builder.Append(functionCall.Function).Append("(");
            for (var index = 0; index < functionCall.Arguments.Count; index++) {
                var arg = functionCall.Arguments[index];
                if (index != 0)
                    _builder.Append(", ");
                arg.Visit(this);
            }

            _builder.Append(")");
        }

        public void VisitMemberAccess(MemberAccess memberAccess) { memberAccess.Member.Visit(this); }

        public void VisitNameCheck(NameCheckElement nameCheckElement) {
            _builder.Append(string.IsNullOrEmpty(nameCheckElement.Name)?"*":nameCheckElement.Name);
            VisitPathElementWithPredicate(nameCheckElement);
        }

        private void VisitPathElementWithPredicate(PathElementWithPredicate pathElement) {
            if (pathElement.Predicate != null) {
                _builder.Append("[");
                pathElement.Predicate.Visit(this);
                _builder.Append("]");
            }

            VisitPathElement(pathElement);
        }

        public void VisitNullConstant(NullConstant nullConstant) { _builder.Append("null"); }

        public void VisitReference(Reference reference) {
            _builder.Append("@.");
            reference.Path.Visit(this);
        }

        private static readonly string[] RelationalOperators = {
            "=",
            "!=",
            "<",
            ">",
            "<=",
            ">=",
        };
        public void VisitRelational(Relational relational) {
            relational.Left.Visit(this);
            if (relational.Right != null) {
                _builder.Append(" ").Append(RelationalOperators[(int)relational.Operator]).Append(" ");
                relational.Right.Visit(this);
            }
        }

        public void VisitRoot(RootElement rootElement) {
            _builder.Append("$");
            VisitPathElement(rootElement);
        }

        public void VisitTypeCheck(TypeCheckElement typeCheckElement) {
            _builder.Append(typeCheckElement.Type.ToString()).Append("()");
            VisitPathElementWithPredicate(typeCheckElement);
        }

        public void VisitUnion(UnionPredicate unionPredicate) {
            for (var index = 0; index < unionPredicate.Items.Count; index++) {
                if (index != 0)
                    _builder.Append(", ");
                unionPredicate.Items[index].Visit(this);
            }
        }

        public void VisitWildcard(WildcardPredicate wildcardPredicate) { _builder.Append("*"); }
    }
}
