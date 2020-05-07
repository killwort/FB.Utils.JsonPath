using System;
using System.Text;

namespace FB.Utils.JsonPath.Language {
    public class Arithmetic : BinaryPredicativeExpressionElement {
        public Arithmetic() : base(new SourcePosition()) { }
        public Arithmetic(SourcePosition pos) : base(pos) { }

        public override void Dump(StringBuilder builder, int level, string delimiter = "\u2502 ", string lastDelimiter = "\u251c ") {
            builder.Append(delimiter);
            builder.Append(lastDelimiter);
            builder.Append("Expr Arithmetic ");
            builder.Append(Operator);
            builder.AppendLine();
            var nextDelimiter = "│ ";
            if (lastDelimiter == "└ ")
                nextDelimiter = "  ";
            if (level == 0)
                nextDelimiter = "";
            Left.Dump(builder, level + 1, delimiter + nextDelimiter, Right == null ? "└ " : "├ ");
            Right?.Dump(builder, level + 1, delimiter + nextDelimiter, "└ ");
        }

        public override LanguageElement Clone() => Clone<Arithmetic>(x => x.Operator = Operator);
        public override LanguageElement Clone(Func<LanguageElement, LanguageElement> modifier) => Clone<Arithmetic>(modifier, x => x.Operator = Operator);

        public ArithmeticOperator Operator;

        public override void Visit(IVisitor visitor) => visitor.VisitArithmetic(this);
    }
}
