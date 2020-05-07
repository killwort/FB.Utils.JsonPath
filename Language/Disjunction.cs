using System;
using System.Text;

namespace FB.Utils.JsonPath.Language {
    public class Disjunction : BinaryPredicativeExpressionElement {
        public Disjunction() : base(new SourcePosition()) { }
        public Disjunction(SourcePosition pos) : base(pos) { }

        public override void Dump(StringBuilder builder, int level, string delimiter = "\u2502 ", string lastDelimiter = "\u251c ") {
            builder.Append(delimiter);
            builder.Append(lastDelimiter);
            builder.Append("Expr ||");
            builder.AppendLine();
            var nextDelimiter = "│ ";
            if (lastDelimiter == "└ ")
                nextDelimiter = "  ";
            if (level == 0)
                nextDelimiter = "";
            Left.Dump(builder, level + 1, delimiter + nextDelimiter, Right == null ? "└ " : "├ ");
            Right?.Dump(builder, level + 1, delimiter + nextDelimiter, "└ ");
        }

        public override LanguageElement Clone() => Clone<Disjunction>(x => { });
        public override LanguageElement Clone(Func<LanguageElement, LanguageElement> modifier) => Clone<Disjunction>(modifier, x => { });
        public override void Visit(IVisitor visitor) => visitor.VisitDisjunction(this);
    }
}
