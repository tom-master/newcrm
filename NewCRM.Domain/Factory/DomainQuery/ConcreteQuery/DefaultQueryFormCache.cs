using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain.Factory.DomainQuery.Query;
using NewCRM.Domain.Factory.DomainSpecification;
using NewCRM.Domain.Repositories;

namespace NewCRM.Domain.Factory.DomainQuery.ConcreteQuery
{
	public class DefaultQueryFormCache : QueryBase
    {
        private readonly IDomainModelQueryProviderFormCache _domainModelQueryProviderFormCache;

        
        public DefaultQueryFormCache(IDomainModelQueryProviderFormCache domainModelQueryProviderFormCache)
        {
            _domainModelQueryProviderFormCache = domainModelQueryProviderFormCache;
        }

        public override IEnumerable<T> Find<T>(Specification<T> specification)
        {
            return _domainModelQueryProviderFormCache.Query(specification);
        }

        public override T FindOne<T>(Specification<T> specification)
        {
            return _domainModelQueryProviderFormCache.Query(specification).FirstOrDefault();
        }

        public override IEnumerable<T> PageBy<T>(Specification<T> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            return _domainModelQueryProviderFormCache.QueryPage(specification, out totalCount, pageIndex, pageSize);
        }
    }
}
