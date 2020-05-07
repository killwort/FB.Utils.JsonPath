using System.Text;

namespace FB.Utils.JsonPath.Language {
    public class Constant<T> : PredicativeExpressionElement {
        public Constant(SourcePosition pos, T value) : base(pos) { Value = value; }
        public T Value;

        public override void Dump(StringBuilder builder, int level, string delimiter = "\u2502 ", string lastDelimiter = "\u251c ") {
            builder.Append(delimiter);
            builder.Append(lastDelimiter);
            builder.Append("Expr const b ");
            builder.Append(Value);
            builder.AppendLine();
        }

        public override LanguageElement Clone() => new Constant<T>(SourcePosition, Value);
        public override void Visit(IVisitor visitor) => visitor.VisitConstant(this);
    }
}
