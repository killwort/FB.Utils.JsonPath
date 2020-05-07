using System;
using System.Text;
using Newtonsoft.Json.Linq;

namespace FB.Utils.JsonPath.Language {
    public class TypeCheckElement : PathElementWithPredicate {
        public TypeCheckElement(SourcePosition pos) : base(pos) { }
        public JTokenType Type;

        public override void Dump(StringBuilder builder, int level, string delimiter = "\u2502 ", string lastDelimiter = "\u251c ") {
            builder.Append(delimiter);
            builder.Append(lastDelimiter);
            builder.Append("PathElement   NODETYPE   ");
            builder.Append(Type);
            builder.AppendLine();
            var nextDelimiter = "│ ";
            if (lastDelimiter == "└ ")
                nextDelimiter = "  ";
            if (level == 0)
                nextDelimiter = "";
            Predicate?.Dump(builder, level + 1, delimiter + nextDelimiter, Next == null ? "└ " : "├ ");
            Next?.Dump(builder, level + 1, delimiter + nextDelimiter, "└ ");
        }

        public override LanguageElement Clone() =>
            new TypeCheckElement(SourcePosition) {
                Predicate = (Predicate) Predicate?.Clone(),
                Next = (PathElement) Next?.Clone(),
                Type = Type
            };

        public override LanguageElement Clone(Func<LanguageElement, LanguageElement> modifier) =>
            modifier(
                new TypeCheckElement(SourcePosition) {
                    Predicate = (Predicate) Predicate?.Clone(modifier),
                    Next = (PathElement) Next?.Clone(modifier),
                    Type = Type
                }
            );

        public override void Visit(IVisitor visitor) => visitor.VisitTypeCheck(this);
    }
}
