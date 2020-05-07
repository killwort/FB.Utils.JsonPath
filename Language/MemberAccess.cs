using System.Text;

namespace FB.Utils.JsonPath.Language {
    public class MemberAccess : Predicate {
        public MemberAccess(SourcePosition pos) : base(pos) { }

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
            Member.Dump(builder, level + 1, delimiter + nextDelimiter, "└ ");
        }

        public PathElement Member;

        public override LanguageElement Clone() => new MemberAccess(SourcePosition) {Member = (PathElement) Member.Clone()};

        public override void Visit(IVisitor visitor) => visitor.VisitMemberAccess(this);
    }
}
