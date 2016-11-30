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

        public sealed override Expression<Func<T, Object>> OrderBy
        {
            get; protected set;
        }


        public DefaultSpecification(Expression<Func<T, Boolean>> expression)
        {
            Expression = expression;

            OrderBy = t => t.Id;
        }

        public DefaultSpecification() : this(T => true)
        {

        }


        public override void AddOrderByExpression(Expression<Func<T, Object>> expression)
        {
            OrderBy = expression;
        }

        public override void ResetOrderByExpressions() => OrderBy = null;

    }
}
