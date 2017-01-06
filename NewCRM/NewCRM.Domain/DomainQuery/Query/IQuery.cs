using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NewCRM.Domain.DomainSpecification;
using NewCRM.Domain.Entitys;

namespace NewCRM.Domain.DomainQuery.Query
{
    public interface IQuery
    {
        /// <summary>
        /// 查找并返回单个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <returns></returns>
        T FindOne<T>(Specification<T> specification) where T : DomainModelBase, IAggregationRoot;

        T FindOne<T>(Expression<Func<T, Boolean>> key) where T : DomainModelBase, IAggregationRoot;

        /// <summary>
        /// 查找并返回集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification">规约对象</param>
        /// <returns></returns>
        IEnumerable<T> Find<T>(Specification<T> specification) where T : DomainModelBase, IAggregationRoot;

        IEnumerable<T> Find<T>(Expression<Func<T, Boolean>> key) where T : DomainModelBase, IAggregationRoot;

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification">规约对象</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="totalCount">总条数</param>
        /// <returns></returns>
        IEnumerable<T> PageBy<T>(Specification<T> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount) where T : DomainModelBase, IAggregationRoot;

    }
}