using System;
using System.Collections.Generic;
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

        /// <summary>
        /// 查找并返回集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <returns></returns>
        IEnumerable<T> Find<T>(Specification<T> specification) where T : DomainModelBase, IAggregationRoot;

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IEnumerable<T> PageBy<T>(Specification<T> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount) where T : DomainModelBase, IAggregationRoot;
    }
}