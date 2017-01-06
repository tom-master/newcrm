using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainSpecification;
using NewCRM.Domain.Entitys;

namespace NewCRM.Domain.Repositories
{
    public interface IDomainModelQueryProvider
    {
        IQueryable<T> Query<T>(Specification<T> selector) where T : DomainModelBase, IAggregationRoot;

        T Query<T>(Expression<Func<T,Boolean>> entity) where T : DomainModelBase, IAggregationRoot;
        IEnumerable<T> Querys<T>(Expression<Func<T, Boolean>> entity) where T : DomainModelBase, IAggregationRoot;
    }
}
