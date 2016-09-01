using System;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.Entities.DomainModel;

namespace NewCRM.QueryServices.DomainSpecification
{
    public interface ISpecification<T> where T : DomainModelBase, IAggregationRoot
    {
        Expression<Func<T, dynamic>> Selector { get; set; }

        Func<IQueryable<T>, IOrderedQueryable<T>> Sort { get; set; }

        Func<IQueryable<T>, IQueryable<T>> PostProcess { get; set; }

        Boolean IsSatisfiedBy(T candidate);
    }
}
