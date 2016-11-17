using System;
using System.Linq.Expressions;
using NewCRM.Domain.Entitys;

namespace NewCRM.Domain.DomainSpecification.Factory
{
    /// <summary>
    /// 抽象规约工厂
    /// </summary>
    public abstract class SpecificationFactory
    {
        /// <summary>
        /// 创建并返回一个规约对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public abstract Specification<T> Create<T>(Expression<Func<T, Boolean>> expression = default(Expression<Func<T, Boolean>>)) where T : DomainModelBase, IAggregationRoot;
    }
}
