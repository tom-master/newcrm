using System;
using System.Collections.Generic;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.Factory.DomainSpecification;

namespace NewCRM.Domain.Repositories
{
    public interface IDomainModelQueryProviderFormCache
    {

        IEnumerable<T> Query<T>(Specification<T> entity) where T : DomainModelBase, IAggregationRoot;

        IEnumerable<T> QueryPage<T>(Specification<T> entity, out Int32 totalCount, Int32 pageIndex, Int32 pageSize) where T : DomainModelBase, IAggregationRoot;
    }
}
