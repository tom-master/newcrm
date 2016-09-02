using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel;
using NewCRM.Domain.Entities.DomainSpecification;
using NewCRM.Domain.Entities.Repositories;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.QueryServices.Query;

namespace NewCRM.QueryServices.ConcreteQuery
{
    [Export(typeof(IQuery))]
    internal class DefaultQuery : IQuery
    {
        [Import]
        private IDomainModelQueryProvider QueryProvider { get; set; }


        public IEnumerable<T> Find<T>(ISpecification<T> specification) where T : DomainModelBase, IAggregationRoot
        {
            return QueryProvider.Query(specification).ToList();
        }

        public IEnumerable<T> PageBy<T>(ISpecification<T> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount) where T : DomainModelBase, IAggregationRoot
        {
            var query = QueryProvider.Query(specification).PageBy(pageIndex, pageSize, d => d.AddTime);
            totalCount = query.Count();
            return query.ToList();
        }
    }
}
