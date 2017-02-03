using System;
using System.Linq.Expressions;
using NewCRM.Domain.Entitys;

namespace NewCRM.Domain.Factory.DomainSpecification
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
        public abstract Expression<Func<T, Boolean>> Expression { get; internal set; }

        /// <summary>
        /// 排序表达式集合
        /// </summary>
        public abstract Expression<Func<T, Object>> OrderBy { get; protected set; }

        /// <summary>
        /// 添加一个排序表达式
        /// </summary>
        /// <param name="expression"></param>
        public abstract void AddOrderByExpression(Expression<Func<T, Object>> expression);

        /// <summary>
        /// 重置排序表达式集合
        /// </summary>
        public abstract void ResetOrderByExpressions();

    }
}
