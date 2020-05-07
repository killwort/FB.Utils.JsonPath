using System;
using System.Text;

namespace FB.Utils.JsonPath.Language {
    public class DescendantsPathElement : PathElement {
        public DescendantsPathElement(SourcePosition pos) : base(pos) { }

        public override void Dump(StringBuilder builder, int level, string delimiter = "\u2502 ", string lastDelimiter = "\u251c ") {
            builder.Append(delimiter);
            builder.Append(lastDelimiter);
            builder.Append("PathElement   DESCENDANTS");
            builder.AppendLine();
            var nextDelimiter = "│ ";
            if (lastDelimiter == "└ ")
                nextDelimiter = "  ";
            if (level == 0)
                nextDelimiter = "";
            Next?.Dump(builder, level + 1, delimiter + nextDelimiter, "└ ");
        }


        public override LanguageElement Clone() => new DescendantsPathElement(SourcePosition) {Next = (PathElement)Next?.Clone()};
        public override LanguageElement Clone(Func<LanguageElement, LanguageElement> modifier) => modifier(new DescendantsPathElement(SourcePosition) {Next = (PathElement) Next?.Clone(modifier)});

        public override void Visit(IVisitor visitor) => visitor.VisitDescendantsPathElement(this);
    }
}
