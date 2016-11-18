using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NewCRM.Domain.Entitys;
using NewCRM.Infrastructure.CommonTools.CustomExtension;

namespace NewCRM.Domain.DomainSpecification
{
    /// <summary>
    /// 规约抽象类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Specification<T> where T : DomainModelBase, IAggregationRoot
    {
        /// <summary>
        /// 查询表达式
        /// </summary>
        public abstract Expression<Func<T, Boolean>> Expression { get; }

        /// <summary>
        /// 排序表达式集合
        /// </summary>
        public abstract IList<Expression<Func<PropertySortCondition>>> OrderByExpressions { get; protected set; }

        /// <summary>
        /// 添加一个排序表达式
        /// </summary>
        /// <param name="expression"></param>
        public abstract void AddOrderByExpression(Expression<Func<PropertySortCondition>> expression);

        /// <summary>
        /// 重置排序表达式集合
        /// </summary>
        public abstract void ResetOrderByExpressions();

    }
}
