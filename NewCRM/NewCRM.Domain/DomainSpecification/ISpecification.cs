using System;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.Entities.DomainModel;

namespace NewCRM.Domain.Entities.DomainSpecification
{
    public interface ISpecification<T> where T : DomainModelBase, IAggregationRoot
    {
        Boolean IsSatisfiedBy(T candidate);
    }
}
