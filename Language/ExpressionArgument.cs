using System;
using System.Text;

namespace FB.Utils.JsonPath.Language {
    public class ExpressionArgument : FunctionArgument {
        public ExpressionArgument(SourcePosition pos) : base(pos) { }

        public override void Dump(StringBuilder builder, int level, string delimiter = "\u2502 ", string lastDelimiter = "\u251c ") {
            builder.Append(delimiter);
            builder.Append(lastDelimiter);
            builder.Append("ExpressionArgument");
            builder.AppendLine();
            var nextDelimiter = "│ ";
            if (lastDelimiter == "└ ")
                nextDelimiter = "  ";
            if (level == 0)
                nextDelimiter = "";
            Expression.Dump(builder, level + 1, delimiter + nextDelimiter, "└ ");
        }

        public Expression Expression;
        public override LanguageElement Clone() => new ExpressionArgument(SourcePosition) {Expression = (Expression) Expression.Clone()};

        public override LanguageElement Clone(Func<LanguageElement, LanguageElement> modifier) =>
            modifier(new ExpressionArgument(SourcePosition) {Expression = (Expression) Expression.Clone(modifier)});

        public override void Visit(IVisitor visitor) => visitor.VistExpressionArgument(this);
    }
}
