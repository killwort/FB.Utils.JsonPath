using System;
using System.Text;

namespace FB.Utils.JsonPath.Language {
    public abstract class LanguageElement {
        protected LanguageElement(SourcePosition pos) { SourcePosition = pos; }

        public SourcePosition SourcePosition;
        public abstract void Dump(StringBuilder builder, int level, string delimiter = "│ ", string lastDelimiter = "├ ");

        public abstract LanguageElement Clone();
        public virtual LanguageElement Clone(Func<LanguageElement, LanguageElement> modifier) => modifier(Clone());

        public abstract void Visit(IVisitor visitor);
    }
}
