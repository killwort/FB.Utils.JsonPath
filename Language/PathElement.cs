namespace FB.Utils.JsonPath.Language {
    public abstract class PathElement : Expression {
        protected PathElement(SourcePosition pos) : base(pos) { }

        public PathElement Next;
    }
}