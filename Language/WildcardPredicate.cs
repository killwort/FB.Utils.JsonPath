using System.Text;

namespace FB.Utils.JsonPath.Language {
    public class WildcardPredicate : Predicate {
        public WildcardPredicate(SourcePosition pos) : base(pos) { }

        public override void Dump(StringBuilder builder, int level, string delimiter = "\u2502 ", string lastDelimiter = "\u251c ") {
            builder.Append(delimiter);
            builder.Append(lastDelimiter);
            builder.Append("Predicate   *");
            builder.AppendLine();
        }

        public override LanguageElement Clone() => new WildcardPredicate(SourcePosition);

        public override void Visit(IVisitor visitor) => visitor.VisitWildcard(this);
    }
}
