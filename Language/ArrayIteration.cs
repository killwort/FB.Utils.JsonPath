using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace FB.Utils.JsonPath.Language {
    public class ArrayIteration : Predicate {
        public ArrayIteration(SourcePosition pos) : base(pos) { }

        public override void Dump(StringBuilder builder, int level, string delimiter = "\u2502 ", string lastDelimiter = "\u251c ") {
            builder.Append(delimiter);
            builder.Append(lastDelimiter);
            builder.Append("Predicate   array iteration ");
            builder.Append(Start);
            builder.Append("->");
            builder.Append(End);
            builder.Append(", ");
            builder.Append(Step);
            builder.AppendLine();
        }

        public override LanguageElement Clone() =>
            new ArrayIteration(SourcePosition) {
                End = End,
                Start = Start,
                Step = Step
            };

        public int? Start, End, Step;

        public override void Visit(IVisitor visitor) => visitor.VisitArrayIteration(this);

        public IEnumerable<JToken> Select(JArray arr) {
            var start = Start ?? 0;
            if (start < 0) start = Math.Min( Math.Max(0, arr.Count + start),arr.Count-1);
            var end = End ?? (arr.Count-1);
            if (end < 0) end = Math.Min(Math.Max(0, arr.Count + end), arr.Count - 1);
            var step = Math.Max(1, Step ?? 1);
            for (var i = start; i <= end; i += step) {
                yield return arr[i];
            }
        }
    }
}
