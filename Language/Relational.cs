using System;
using System.Text;

namespace FB.Utils.JsonPath.Language {
    public class Relational : BinaryPredicativeExpressionElement {
        public Relational() : base(new SourcePosition()) { }
        public Relational(SourcePosition pos) : base(pos) { }

        public override void Dump(StringBuilder builder, int level, string delimiter = "\u2502 ", string lastDelimiter = "\u251c ") {
            builder.Append(delimiter);
            builder.Append(lastDelimiter);
            builder.Append("Expr Rel ");
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

        public Relation Operator;
        public override LanguageElement Clone() => Clone<Relational>(x => x.Operator = Operator);
        public override LanguageElement Clone(Func<LanguageElement, LanguageElement> modifier) => Clone<Relational>(modifier, x => x.Operator = Operator);
        public override void Visit(IVisitor visitor) => visitor.VisitRelational(this);
    }
}
