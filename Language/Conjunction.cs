using System;
using System.Text;

namespace FB.Utils.JsonPath.Language {
    public class Conjunction : BinaryPredicativeExpressionElement {
        public Conjunction():base(new SourcePosition()){}
        public Conjunction(SourcePosition pos) : base(pos) { }

        public override void Dump(StringBuilder builder, int level, string delimiter = "\u2502 ", string lastDelimiter = "\u251c ") {
            builder.Append(delimiter);
            builder.Append(lastDelimiter);
            builder.Append("Expr &&");
            builder.AppendLine();
            var nextDelimiter = "│ ";
            if (lastDelimiter == "└ ")
                nextDelimiter = "  ";
            if (level == 0)
                nextDelimiter = "";
            Left.Dump(builder, level + 1, delimiter + nextDelimiter, Right == null ? "└ " : "├ ");
            Right?.Dump(builder, level + 1, delimiter + nextDelimiter, "└ ");
        }

        public override LanguageElement Clone() => Clone<Conjunction>(x => { });
        public override LanguageElement Clone(Func<LanguageElement, LanguageElement> modifier) => Clone<Conjunction>(modifier, x => { });

        public override void Visit(IVisitor visitor) => visitor.VisitConjunction(this);
    }
}
