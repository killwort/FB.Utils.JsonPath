using System;
using System.Text;

namespace FB.Utils.JsonPath.Language {
    public class RootElement : PathElement {
        public RootElement(SourcePosition pos) : base(pos) { }

        public override void Dump(StringBuilder builder, int level, string delimiter = "\u2502 ", string lastDelimiter = "\u251c ") {
            builder.Append(delimiter);
            builder.Append(lastDelimiter);
            builder.Append("PathElement   ROOT");
            builder.AppendLine();
            var nextDelimiter = "│ ";
            if (lastDelimiter == "└ ")
                nextDelimiter = "  ";
            if (level == 0)
                nextDelimiter = "";
            Next?.Dump(builder, level + 1, delimiter + nextDelimiter, "└ ");
        }

        public override LanguageElement Clone() => new RootElement(SourcePosition) {Next = (PathElement) Next?.Clone()};
        public override LanguageElement Clone(Func<LanguageElement, LanguageElement> modifier) => modifier(new RootElement(SourcePosition) {Next = (PathElement) Next?.Clone(modifier)});
        public override void Visit(IVisitor visitor) => visitor.VisitRoot(this);
    }
}
