using System;
using System.Text;

namespace FB.Utils.JsonPath.Language {
    public class FilterPredicate : Predicate {
        public FilterPredicate(SourcePosition pos) : base(pos) { }

        public PredicativeExpressionElement Predicate;

        public override void Dump(StringBuilder builder, int level, string delimiter = "\u2502 ", string lastDelimiter = "\u251c ") {
            builder.Append(delimiter);
            builder.Append(lastDelimiter);
            builder.Append("Predicate   filter");
            builder.AppendLine();
            var nextDelimiter = "│ ";
            if (lastDelimiter == "└ ")
                nextDelimiter = "  ";
            if (level == 0)
                nextDelimiter = "";
            Predicate.Dump(builder, level + 1, delimiter + nextDelimiter, "└ ");
        }

        public override LanguageElement Clone() => new FilterPredicate(SourcePosition) {Predicate = (PredicativeExpressionElement) Predicate.Clone()};

        public override LanguageElement Clone(Func<LanguageElement, LanguageElement> modifier) =>
            modifier(new FilterPredicate(SourcePosition) {Predicate = (PredicativeExpressionElement) Predicate.Clone(modifier)});

        public override void Visit(IVisitor visitor) => visitor.VisitFilterPredicate(this);
    }
}
