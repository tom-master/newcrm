using System;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.Entities.DomainModel;

namespace NewCRM.QueryServices.DomainSpecification
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


        public static ISpecification<T> OrderByDescending<T>(this ISpecification<T> left, ISpecification<T> right) where T : DomainModelBase, IAggregationRoot
        {
            var newSpecification = new Specification<T>(left.Selector) { PostProcess = left.PostProcess };
            if (left.Sort != null)
            {
                newSpecification.Sort = items => left.Sort(items).ThenBy(right.Selector);
            }
            else
            {
                newSpecification.Sort = items => items.OrderBy(right.Selector);
            }
            return newSpecification;
        }
    }
}
