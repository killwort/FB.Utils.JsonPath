using System.Linq;

namespace FB.Utils.JsonPath.Language {
    public static class Optimizer {
        public static Expression Optimize(this Expression expression) {
            return (Expression) expression.Clone(
                item => {
                    switch (item) {
                        case BinaryPredicativeExpressionElement bfee:
                            if (bfee.Right == null) return bfee.Left;
                            break;
                        case UnionPredicate up:
                            if (up.Items.Count == 1) return up.Items.First();
                            break;
                    }

                    return item;
                }
            );
        }
    }
}
