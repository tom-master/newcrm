using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.Factory.DomainCreate;
using NewCRM.Domain.Factory.DomainQuery.Query;
using NewCRM.Domain.Factory.DomainSpecification;
using NewCRM.Domain.Repositories;
using NewCRM.Infrastructure.CommonTools.CustomExtension;

namespace NewCRM.Domain.Factory.DomainQuery.ConcreteQuery
{
    public class DefaultQuery : QueryBase
    {
        private readonly IDomainModelQueryProvider _queryProvider;
        private readonly DomainFactory _domainFactory;

        public DefaultQuery(IDomainModelQueryProvider queryProvider, DomainFactory domainFactory)
        {
            _queryProvider = queryProvider;
            _domainFactory = domainFactory;
        }

        /// <summary>
        /// 查找并返回单个对象
        /// </summary>
        public override U FindOne<T, U>(Specification<T> specification, Expression<Func<T, U>> selector)
        {
            return _queryProvider.Query(specification).Select(selector).FirstOrDefault();
        }

        /// <summary>
        /// 查找并返回集合
        /// </summary>
        public override IEnumerable<U> Find<T, U>(Specification<T> specification, Expression<Func<T, U>> selector)
        {
            return _queryProvider.Query(specification).Select(selector).ToList();
        }

        /// <summary>
        /// 分页
        /// </summary>
        public override IEnumerable<U> PageBy<T, U>(Specification<T> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount, Expression<Func<T, U>> selector)
        {
            var query = _queryProvider.Query(specification);
            totalCount = query.Count();
            return query.PageBy(pageIndex, pageSize, specification.OrderBy).Select(selector).ToList();
        }
    }
}
