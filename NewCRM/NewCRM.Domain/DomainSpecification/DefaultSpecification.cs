using System;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.Entities.DomainModel;

namespace NewCRM.Domain.Entities.DomainSpecification
{
    public class Specification<T> : ISpecification<T> where T : DomainModelBase, IAggregationRoot
    {
        private readonly Func<T, Boolean> _selector;


        public Specification(Func<T, Boolean> selector)
        {
            _selector = selector;
        }

        public Specification()
        {
            _selector = arg => true;
        }


        public Expression<Func<T, Boolean>> Predicate { get; set; }


        public Boolean IsSatisfiedBy(T candidate)
        {
            return _selector(candidate);
        }
    }
}
