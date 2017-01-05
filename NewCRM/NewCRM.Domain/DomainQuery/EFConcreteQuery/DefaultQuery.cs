using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.DomainQuery.Query;
using NewCRM.Domain.DomainSpecification;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.Repositories;
using NewCRM.Infrastructure.CommonTools.CustomExtension;

namespace NewCRM.Domain.DomainQuery.EFConcreteQuery
{

    [Export(typeof(IQuery))]
    internal class DefaultQuery : IQuery
    {
        [Import]
        private IDomainModelQueryProvider QueryProvider { get; set; }

        /// <summary>
        /// 查找并返回单个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <returns></returns>
        public T FindOne<T>(Specification<T> specification) where T : DomainModelBase, IAggregationRoot
        {
            return QueryProvider.Query(specification).FirstOrDefault();
        }

        /// <summary>
        /// 查找并返回集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <returns></returns>
        public IEnumerable<T> Find<T>(Specification<T> specification) where T : DomainModelBase, IAggregationRoot
        {
            return QueryProvider.Query(specification).ToList();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<T> PageBy<T>(Specification<T> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount) where T : DomainModelBase, IAggregationRoot
        {
            var query = QueryProvider.Query(specification);

            totalCount = query.Count();

            return query.PageBy(pageIndex, pageSize, specification.OrderBy).ToList();
        }

        public T FindOne<T>(T key) where T : DomainModelBase, IAggregationRoot
        {
            return QueryProvider.Query(key);
        }

        public IEnumerable<T> Find<T>(T key) where T : DomainModelBase, IAggregationRoot
        {
            return QueryProvider.Querys(key);
        }
    }
}
