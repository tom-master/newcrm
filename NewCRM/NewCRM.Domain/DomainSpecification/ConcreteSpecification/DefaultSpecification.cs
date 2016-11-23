using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NewCRM.Domain.Entitys;
using NewCRM.Infrastructure.CommonTools.CustomExtension;

namespace NewCRM.Domain.DomainSpecification.ConcreteSpecification
{

    /// <summary>
    /// 默认规约 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class DefaultSpecification<T> : Specification<T> where T : DomainModelBase, IAggregationRoot
    {
        public override Expression<Func<T, Boolean>> Expression { get; }

        public override IList<Expression<Func<PropertySortCondition>>> OrderByExpressions { get; protected set; }


        public DefaultSpecification(Expression<Func<T, Boolean>> expression)
        {
            Expression = expression;
        }

        public DefaultSpecification() : this(T => true) { }


        public override void AddOrderByExpression(Expression<Func<PropertySortCondition>> expression)
        {
            if (OrderByExpressions == null)
            {
                OrderByExpressions = new List<Expression<Func<PropertySortCondition>>>();
            }

            OrderByExpressions.Add(expression);
        }

        public override void ResetOrderByExpressions() => OrderByExpressions?.Clear();

    }
}
