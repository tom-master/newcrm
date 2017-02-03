using System.Linq.Expressions;

namespace NewCRM.Domain.Factory.DomainSpecification
{

    internal sealed class ParameterVisitor : ExpressionVisitor
    {
        internal ParameterVisitor(ParameterExpression paramExpr)
        {
            ParameterExpression = paramExpr;
        }

        internal ParameterExpression ParameterExpression { get; private set; }

        internal Expression Replace(Expression expr) => Visit(expr);

        protected override Expression VisitParameter(ParameterExpression p) => ParameterExpression;

    }
}
