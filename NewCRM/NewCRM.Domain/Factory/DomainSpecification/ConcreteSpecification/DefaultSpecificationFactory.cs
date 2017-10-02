using System;
using System.Linq.Expressions;
using NewCRM.Domain.Factory.DomainSpecification.Factory;

namespace NewCRM.Domain.Factory.DomainSpecification.ConcreteSpecification
{
    /// <summary>
    /// 默认规约工厂
    /// </summary>
    internal sealed class DefaultSpecificationFactory : SpecificationFactory
    {
        public override Specification<T> Create<T>(Expression<Func<T, Boolean>> expression = default(Expression<Func<T, Boolean>>))
        {
            return expression == default(Expression<Func<T, Boolean>>) ? new DefaultSpecification<T>() : new DefaultSpecification<T>(expression);
        }

    }
}
