using System;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using NewCRM.Domain.Entities.DomainSpecification.Factory;

namespace NewCRM.Domain.Entities.DomainSpecification.ConcreteSpecification
{
    /// <summary>
    /// 默认规约工厂
    /// </summary>
    [Export(typeof(SpecificationFactory))]
    public sealed class DefaultSpecificationFactory : SpecificationFactory
    {
        public override Specification<T> Create<T>(Expression<Func<T, Boolean>> expression = default(Expression<Func<T, Boolean>>))
        {
            if (expression == default(Expression<Func<T, Boolean>>))
            {
                return new DefaultSpecification<T>();
            }
            return new DefaultSpecification<T>(expression);
        }
    }
}
