using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.Factory.DomainSpecification;

namespace NewCRM.Domain.Factory.DomainQuery.Query
{
    public interface IQuery
    {
        /// <summary>
        /// 查找并返回单个对象
        /// </summary>
        T FindOne<T>(Specification<T> specification, Expression<Func<T, Object>> selector) where T : DomainModelBase, IAggregationRoot;

        /// <summary>
        /// 查找并返回单个对象
        /// </summary>
        T FindOne<T>(Specification<T> specification) where T : DomainModelBase, IAggregationRoot;

        /// <summary>
        /// 查找并返回集合
        /// </summary>
        IEnumerable<T> Find<T>(Specification<T> specification, Expression<Func<T, Object>> selector) where T : DomainModelBase, IAggregationRoot;

        /// <summary>
        /// 查找并返回集合
        /// </summary>
        IEnumerable<T> Find<T>(Specification<T> specification) where T : DomainModelBase, IAggregationRoot;

        /// <summary>
        /// 分页
        /// </summary>
        IEnumerable<T> PageBy<T>(Specification<T> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount, Expression<Func<T, Object>> selector ) where T : DomainModelBase, IAggregationRoot;

        /// <summary>
        /// 分页
        /// </summary>
        IEnumerable<T> PageBy<T>(Specification<T> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount) where T : DomainModelBase, IAggregationRoot;

    }
}