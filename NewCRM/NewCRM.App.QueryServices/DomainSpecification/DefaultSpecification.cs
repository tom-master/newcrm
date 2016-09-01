using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.Entities.DomainModel;

namespace NewCRM.QueryServices.DomainSpecification
{
    [Export(typeof(ISpecification<>))]
    public class Specification<T> : ISpecification<T> where T : DomainModelBase,IAggregationRoot
    {
        public Expression<Func<T, dynamic>> Selector { get; set; }

        public Func<IQueryable<T>, IOrderedQueryable<T>> Sort { get; set; }

        public Func<IQueryable<T>, IQueryable<T>> PostProcess { get; set; }

        public Specification(Expression<Func<T, dynamic>> selector)
        {
            Selector = selector;
        }


        public Expression<Func<T, Boolean>> Predicate { get; set; }


        public Boolean IsSatisfiedBy(T candidate)
        {
            return Selector.Compile()(candidate);
        }
    }
}
