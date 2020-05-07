using System.Collections.Generic;
using System.Text;

namespace FB.Utils.JsonPath.Language {
    public class ArrayIndex : Predicate {
        public ArrayIndex(SourcePosition pos, IEnumerable<int> indexes = null) : base(pos) {
            if (indexes != null)
                Indexes.AddRange(indexes);
        }

        public override void Dump(StringBuilder builder, int level, string delimiter = "\u2502 ", string lastDelimiter = "\u251c ") {
            builder.Append(delimiter);
            builder.Append(lastDelimiter);
            builder.Append("Predicate   array index ");
            builder.AppendJoin(", ", Indexes);
            builder.AppendLine();
        }

        public override LanguageElement Clone() => new ArrayIndex(SourcePosition, Indexes);

        public ArrayIndex Add(int n) {
            Indexes.Add(n);
            return this;
        }

        public readonly List<int> Indexes = new List<int>();

        public override void Visit(IVisitor visitor) => visitor.VisitArrayIndex(this);
    }
}
