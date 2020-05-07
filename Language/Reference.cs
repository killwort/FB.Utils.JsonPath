using System;
using System.Text;

namespace FB.Utils.JsonPath.Language {
    public class Reference : PredicativeExpressionElement {
        public Reference(SourcePosition pos) : base(pos) { }

        public override void Dump(StringBuilder builder, int level, string delimiter = "\u2502 ", string lastDelimiter = "\u251c ") {
            builder.Append(delimiter);
            builder.Append(lastDelimiter);
            builder.Append("Expr ref");
            builder.AppendLine();
            var nextDelimiter = "│ ";
            if (lastDelimiter == "└ ")
                nextDelimiter = "  ";
            if (level == 0)
                nextDelimiter = "";
            Path.Dump(builder, level + 1, delimiter + nextDelimiter, "└ ");
        }

        public PathElement Path;

        public override LanguageElement Clone() => new Reference(SourcePosition) {
            Path = (PathElement)Path.Clone()
        };

        public override LanguageElement Clone(Func<LanguageElement, LanguageElement> modifier) => modifier(new Reference(SourcePosition) {Path = (PathElement) Path.Clone(modifier)});

        public override void Visit(IVisitor visitor) => visitor.VisitReference(this);
    }
}
