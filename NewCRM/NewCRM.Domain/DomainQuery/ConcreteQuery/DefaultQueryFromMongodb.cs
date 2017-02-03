using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.DomainQuery.Query;
using NewCRM.Domain.DomainSpecification;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.Repositories;

namespace NewCRM.Domain.DomainQuery.ConcreteQuery
{
    [Export("Mongodb", typeof(IQuery))]
    internal class DefaultQueryFromMongodb : IQuery
    {
        private readonly IDomainModelQueryProvider _queryProvider;

        [ImportingConstructor]
        public DefaultQueryFromMongodb([Import("Mongodb", typeof(IDomainModelQueryProvider))]IDomainModelQueryProvider queryProvider)
        {
            _queryProvider = queryProvider;
        }

        public T FindOne<T>(Specification<T> specification) where T : DomainModelBase, IAggregationRoot
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Find<T>(Specification<T> specification) where T : DomainModelBase, IAggregationRoot
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> PageBy<T>(Specification<T> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount) where T : DomainModelBase, IAggregationRoot
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
