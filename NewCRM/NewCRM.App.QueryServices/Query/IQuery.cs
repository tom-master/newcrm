using System.Collections.Generic;
using NewCRM.Domain.Entities.DomainModel;
using NewCRM.QueryServices.DomainSpecification;

namespace NewCRM.QueryServices.Query
{
    public interface IQuery<T> where T : DomainModelBase, IAggregationRoot
    {
        IEnumerable<T> Find(ISpecification<T> specification);
    }
}