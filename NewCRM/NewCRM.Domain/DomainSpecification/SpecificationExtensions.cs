using System.Linq;
using NewCRM.Domain.Entities.DomainModel;

namespace NewCRM.Domain.Entities.DomainSpecification
{
    public static class SpecificationExtensions
    {
        public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right) where T : DomainModelBase,IAggregationRoot
        {
            return new Specification<T>(candidate => left.IsSatisfiedBy(candidate) && right.IsSatisfiedBy(candidate));
        }

        public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right) where T : DomainModelBase, IAggregationRoot
        {
            return new Specification<T>(candidate => left.IsSatisfiedBy(candidate) || right.IsSatisfiedBy(candidate));
        }

        public static ISpecification<T> Not<T>(this ISpecification<T> left) where T : DomainModelBase, IAggregationRoot
        {
            return new Specification<T>(candidate => !left.IsSatisfiedBy(candidate));
        }
    }
}
