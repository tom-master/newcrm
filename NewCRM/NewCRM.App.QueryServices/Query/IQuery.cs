using System;
using System.Collections.Generic;
using NewCRM.Domain.Entities.DomainModel;
using NewCRM.Domain.Entities.DomainSpecification;

namespace NewCRM.QueryServices.Query
{
    public interface IQuery 
    {
        IEnumerable<T> Find<T>(ISpecification<T> specification) where T : DomainModelBase, IAggregationRoot;

        IEnumerable<T> PageBy<T>(ISpecification<T> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount) where T : DomainModelBase, IAggregationRoot;
    }
}