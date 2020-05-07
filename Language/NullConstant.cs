using System.Text;

namespace FB.Utils.JsonPath.Language {
    public class NullConstant : PredicativeExpressionElement {
        public NullConstant(SourcePosition pos) : base(pos) { }

        public override void Dump(StringBuilder builder, int level, string delimiter = "\u2502 ", string lastDelimiter = "\u251c ") {
            builder.Append(delimiter);
            builder.Append(lastDelimiter);
            builder.Append("Expr const null");
            builder.AppendLine();
        }

        public override LanguageElement Clone() => new NullConstant(SourcePosition);

        public override void Visit(IVisitor visitor) => visitor.VisitNullConstant(this);
    }
}
