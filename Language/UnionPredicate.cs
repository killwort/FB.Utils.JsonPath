using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Converters;

namespace FB.Utils.JsonPath.Language {
    public class UnionPredicate : Predicate {
        public UnionPredicate(SourcePosition pos, IEnumerable<Predicate> items = null) : base(pos) {
            if (items != null)
                Items.AddRange(items);
        }

        public override void Dump(StringBuilder builder, int level, string delimiter = "\u2502 ", string lastDelimiter = "\u251c ") {
            builder.Append(delimiter);
            builder.Append(lastDelimiter);
            builder.Append("Predicate   union");
            builder.AppendLine();
            var nextDelimiter = "│ ";
            if (lastDelimiter == "└ ")
                nextDelimiter = "  ";
            if (level == 0)
                nextDelimiter = "";
            for (var i = 0; i < Items.Count; i++) {
                Items[i].Dump(builder, level + 1, delimiter + nextDelimiter, i == Items.Count - 1 ? "└ " : "├ ");
            }
        }

        public readonly List<Predicate> Items = new List<Predicate>();
        public override LanguageElement Clone() => new UnionPredicate(SourcePosition, Items.Select(x => (Predicate) x.Clone()));
        public override LanguageElement Clone(Func<LanguageElement, LanguageElement> modifier) => modifier(new UnionPredicate(SourcePosition, Items.Select(x => (Predicate) x.Clone(modifier))));
        public override void Visit(IVisitor visitor) => visitor.VisitUnion(this);
    }
}
