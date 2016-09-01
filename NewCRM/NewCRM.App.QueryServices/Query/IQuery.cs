using System;
using System.Collections.Generic;
using NewCRM.Domain.Entities.DomainModel;
using NewCRM.QueryServices.DomainSpecification;

namespace NewCRM.QueryServices.Query
{
    public interface IQuery<T> where T : DomainModelBase, IAggregationRoot
    {
        IEnumerable<T> Find(ISpecification<T> specification);

        IEnumerable<T> PageBy(ISpecification<T> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);
    }
}