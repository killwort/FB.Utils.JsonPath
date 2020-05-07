using System.Collections.Generic;
using System.Security.AccessControl;
using FB.Utils.JsonPath.Language;
using Newtonsoft.Json.Linq;

namespace FB.Utils.JsonPath {
    public delegate IEnumerable<JToken> JsonPathCustomFunction(IEnumerable<IEnumerable<JToken>> arguments);

    public static class JsonPathExtensions {
        public static Expression ParseJsonPath(this string source) {
            var collector = new ErrorCollector();
            var parser = new Parser(new Scanner(new StringBuffer(source)), collector);
            parser.Parse();
            collector.ThrowErrors();
            return parser.result;
        }


        public static EvaluatingVisitor CreateEvaluatorWithFunctions(IDictionary<string, JsonPathCustomFunction> functions) {
            var visitor = new EvaluatingVisitor();
            if (functions != null)
                foreach (var fn in functions)
                    visitor.RegisterFunction(fn.Key, fn.Value);
            return visitor;
        }

        public static IEnumerable<JToken> EvaluateJsonPath(this JToken src, string jsonPath, IDictionary<string, JsonPathCustomFunction> functions = null) {
            return EvaluateJsonPath(src, jsonPath.ParseJsonPath(), CreateEvaluatorWithFunctions(functions));
        }

        public static IEnumerable<JToken> EvaluateJsonPath(this JToken src, Expression jsonPath, EvaluatingVisitor evaluator = null) {
            if (evaluator == null) evaluator = new EvaluatingVisitor(src);
            else evaluator.Reset(src);
            jsonPath.Visit(evaluator);
            return evaluator.Result;
        }

        public static IEnumerable<JToken> EvaluateJsonPath(this JToken src, string jsonPath, EvaluatingVisitor evaluator = null) {
            if (evaluator == null) evaluator = new EvaluatingVisitor(src);
            else evaluator.Reset(src);
            jsonPath.ParseJsonPath().Visit(evaluator);
            return evaluator.Result;
        }

        public static IEnumerable<JToken> EvaluateJsonPath(this JToken src, Expression jsonPath, IDictionary<string, JsonPathCustomFunction> functions = null) {
            var evaluator = CreateEvaluatorWithFunctions(functions);
            evaluator.Reset(src);
            jsonPath.Visit(evaluator);
            return evaluator.Result;
        }
    }
}
