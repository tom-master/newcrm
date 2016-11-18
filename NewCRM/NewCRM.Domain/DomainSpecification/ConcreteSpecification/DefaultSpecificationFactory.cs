using System;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using NewCRM.Domain.DomainSpecification.Factory;

namespace NewCRM.Domain.DomainSpecification.ConcreteSpecification
{
    /// <summary>
    /// 默认规约工厂
    /// </summary>
    [Export(typeof(SpecificationFactory))]
    internal sealed class DefaultSpecificationFactory : SpecificationFactory
    {
        public override Specification<T> Create<T>(Expression<Func<T, Boolean>> expression = default(Expression<Func<T, Boolean>>)) => expression == default(Expression<Func<T, Boolean>>) ? new DefaultSpecification<T>() : new DefaultSpecification<T>(expression);

    }
}
