using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.Factory.DomainQuery.Query;
using NewCRM.Domain.Factory.DomainSpecification;
using NewCRM.Domain.Repositories;

namespace NewCRM.Domain.Factory.DomainQuery.ConcreteQuery
{
    [Export("Redis",typeof(IQuery))]
    internal class DefaultQueryFormCache : IQuery
    {
        private readonly IDomainModelQueryProviderFormCache _domainModelQueryProviderFormCache;

        [ImportingConstructor]
        public DefaultQueryFormCache(IDomainModelQueryProviderFormCache domainModelQueryProviderFormCache)
        {
            _domainModelQueryProviderFormCache = domainModelQueryProviderFormCache;
        }

        public IEnumerable<T> Find<T>(Specification<T> specification) where T : DomainModelBase, IAggregationRoot
        {
            return _domainModelQueryProviderFormCache.Query(specification);
        }

        public T FindOne<T>(Specification<T> specification) where T : DomainModelBase, IAggregationRoot
        {
            return _domainModelQueryProviderFormCache.Query(specification).FirstOrDefault();
        }

        public IEnumerable<T> PageBy<T>(Specification<T> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount) where T : DomainModelBase, IAggregationRoot
        {
            return _domainModelQueryProviderFormCache.QueryPage(specification, out totalCount, pageIndex, pageSize);
        }
    }
}
