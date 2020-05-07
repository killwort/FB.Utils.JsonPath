namespace FB.Utils.JsonPath.Language {
    public interface IVisitor {
        void VisitArithmetic(Arithmetic arithmetic);
        void VisitArrayIndex(ArrayIndex arrayIndex);
        void VisitArrayIteration(ArrayIteration arrayIteration);
        void VisitConstant<T>(Constant<T> constant);
        void VisitConjunction(Conjunction conjunction);
        void VisitDescendantsPathElement(DescendantsPathElement descendantsPathElement);
        void VisitDisjunction(Disjunction disjunction);
        void VistExpressionArgument(ExpressionArgument expressionArgument);
        void VisitPredicativeArgument(PredicativeArgument predicativeArgument);
        void VisitFilterPredicate(FilterPredicate filterPredicate);
        void VisitFunctionCall(FunctionCall functionCall);
        void VisitMemberAccess(MemberAccess memberAccess);
        void VisitNameCheck(NameCheckElement nameCheckElement);
        void VisitNullConstant(NullConstant nullConstant);
        void VisitReference(Reference reference);
        void VisitRelational(Relational relational);
        void VisitRoot(RootElement rootElement);
        void VisitTypeCheck(TypeCheckElement typeCheckElement);
        void VisitUnion(UnionPredicate unionPredicate);
        void VisitWildcard(WildcardPredicate wildcardPredicate);
    }
}
