using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FB.Utils.JsonPath.Language {
    public class FunctionCall : Expression {
        public FunctionCall(SourcePosition pos, string fn = null, IEnumerable<FunctionArgument> args = null) : base(pos) {
            Function = fn;
            if (args != null)
                Arguments.AddRange(args);
        }

        public string Function;
        public readonly List<FunctionArgument> Arguments = new List<FunctionArgument>();

        public override void Dump(StringBuilder builder, int level, string delimiter = "│ ", string lastDelimiter = "├ ") {
            builder.Append(delimiter);
            builder.Append(lastDelimiter);
            builder.Append("FunctionCall   ");
            builder.Append(Function);
            builder.AppendLine();
            var nextDelimiter = "│ ";
            if (lastDelimiter == "└ ")
                nextDelimiter = "  ";
            if (level == 0)
                nextDelimiter = "";
            for (var i = 0; i < Arguments.Count; i++) {
                Arguments[i].Dump(builder, level + 1, delimiter + nextDelimiter, i == Arguments.Count - 1 ? "└ " : "├ ");
            }
        }

        public override LanguageElement Clone() => new FunctionCall(SourcePosition, Function, Arguments.Select(x => (FunctionArgument) x.Clone()));

        public override LanguageElement Clone(Func<LanguageElement, LanguageElement> modifier) =>
            modifier(new FunctionCall(SourcePosition, Function, Arguments.Select(x => (FunctionArgument) x.Clone(modifier))));

        public override void Visit(IVisitor visitor) => visitor.VisitFunctionCall(this);
    }
}
