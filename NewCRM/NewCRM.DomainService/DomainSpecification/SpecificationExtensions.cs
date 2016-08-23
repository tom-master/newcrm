using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entities.DomainModel;

namespace NewCRM.Domain.Services.DomainSpecification
{
    internal static class SpecificationExtensions
    {
        public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right) where T : DomainModelBase
        {
            return new Specification<T>(candidate => left.IsSatisfiedBy(candidate) && right.IsSatisfiedBy(candidate));
        }

        public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right) where T : DomainModelBase
        {
            return new Specification<T>(candidate => left.IsSatisfiedBy(candidate) || right.IsSatisfiedBy(candidate));
        }

        public static ISpecification<T> Not<T>(this ISpecification<T> left) where T : DomainModelBase
        {
            return new Specification<T>(candidate => !left.IsSatisfiedBy(candidate));
        }
    }
}
