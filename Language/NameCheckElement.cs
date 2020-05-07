using System;
using System.Text;

namespace FB.Utils.JsonPath.Language {
    public class NameCheckElement : PathElementWithPredicate {
        public NameCheckElement(SourcePosition pos) : base(pos) { }

        public string Name;

        public override void Dump(StringBuilder builder, int level, string delimiter = "\u2502 ", string lastDelimiter = "\u251c ") {
            builder.Append(delimiter);
            builder.Append(lastDelimiter);
            builder.Append("PathElement   NAME   ");
            builder.Append(Name);
            builder.AppendLine();
            var nextDelimiter = "│ ";
            if (lastDelimiter == "└ ")
                nextDelimiter = "  ";
            if (level == 0)
                nextDelimiter = "";
            Predicate?.Dump(builder, level + 1, delimiter + nextDelimiter, Next==null?"└ ":"├ ");
            Next?.Dump(builder, level + 1, delimiter + nextDelimiter, "└ ");
        }

        public override LanguageElement Clone() =>
            new NameCheckElement(SourcePosition) {
                Predicate = (Predicate) Predicate?.Clone(),
                Next = (PathElement) Next?.Clone(),
                Name=Name
            };

        public override LanguageElement Clone(Func<LanguageElement, LanguageElement> modifier) => modifier(
            new NameCheckElement(SourcePosition) {
                Predicate = (Predicate) Predicate?.Clone(modifier),
                Next = (PathElement) Next?.Clone(modifier),
                Name=Name
            }
        );

        public override void Visit(IVisitor visitor) => visitor.VisitNameCheck(this);
    }
}
