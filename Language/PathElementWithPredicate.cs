namespace FB.Utils.JsonPath.Language {
    public abstract class PathElementWithPredicate : PathElement {
        protected PathElementWithPredicate(SourcePosition pos) : base(pos) { }

        public Predicate Predicate;
    }
}