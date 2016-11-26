using System;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainSpecification;
using NewCRM.Domain.Entitys;

namespace NewCRM.Domain.Repositories
{
    public interface IDomainModelQueryProvider
    {
        IQueryable<T> Query<T>(Specification<T> selector) where T : DomainModelBase, IAggregationRoot;


        IQueryable<dynamic> Query<T>(Specification<T> selector, Expression<Func<T, dynamic>> selectorField) where T : DomainModelBase, IAggregationRoot;
    }
}
