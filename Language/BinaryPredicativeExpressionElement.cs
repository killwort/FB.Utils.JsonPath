using System;

namespace FB.Utils.JsonPath.Language {
    public abstract class BinaryPredicativeExpressionElement : PredicativeExpressionElement {
        public PredicativeExpressionElement Left, Right;

        protected BinaryPredicativeExpressionElement(SourcePosition pos) : base(pos) { }

        public T Clone<T>(Action<T> init)
            where T : BinaryPredicativeExpressionElement, new() {
            var rv = new T {
                SourcePosition = SourcePosition,
                Left = (PredicativeExpressionElement) Left?.Clone(),
                Right = (PredicativeExpressionElement) Right?.Clone(),
            };
            init?.Invoke(rv);
            return rv;
        }

        public LanguageElement Clone<T>(Func<LanguageElement, LanguageElement> modifier, Action<T> init)
            where T : BinaryPredicativeExpressionElement, new() {
            var rv = new T() {
                SourcePosition = SourcePosition,
                Left = (PredicativeExpressionElement) Left?.Clone(modifier),
                Right = (PredicativeExpressionElement) Right?.Clone(modifier)
            };
            init?.Invoke(rv);
            return modifier(rv);
        }
    }
}
