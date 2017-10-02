using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain.Factory.DomainQuery.Query;
using NewCRM.Domain.Factory.DomainSpecification;
using NewCRM.Domain.Repositories;

namespace NewCRM.Domain.Factory.DomainQuery.ConcreteQuery
{
    internal class DefaultQueryFromMongodb : QueryBase
    {
        private readonly IDomainModelQueryProvider _queryProvider;

        
        public DefaultQueryFromMongodb(IDomainModelQueryProvider queryProvider)
        {
            _queryProvider = queryProvider;
        }

        public override IEnumerable<T> PageBy<T>(Specification<T> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount) 
        {
            var query = _queryProvider.Query(specification).OrderByDescending(d => d.AddTime);

            totalCount = query.Count();

            var internalPageIndex = 0;

            if (((pageIndex - 1) * pageSize) == 0)
            {
                internalPageIndex = 5;
            }
            else
            {
                internalPageIndex = (pageIndex - 1) * pageSize;
            }

            return query.Take(internalPageIndex).Take(pageSize);
        }
    }
}
