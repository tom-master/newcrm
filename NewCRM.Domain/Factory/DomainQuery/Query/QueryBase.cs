using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.Factory.DomainSpecification;

namespace NewCRM.Domain.Factory.DomainQuery.Query
{
    public abstract class QueryBase : IQuery
    {
        public virtual T FindOne<T>(Specification<T> specification, Expression<Func<T, Object>> selector) where T : DomainModelBase, IAggregationRoot
        {
            return default(T);
        }

        public virtual T FindOne<T>(Specification<T> specification) where T : DomainModelBase, IAggregationRoot
        {
            return default(T);
        }

        public virtual IEnumerable<T> Find<T>(Specification<T> specification, Expression<Func<T, Object>> selector) where T : DomainModelBase, IAggregationRoot
        {
            return default(IEnumerable<T>);
        }

        public virtual IEnumerable<T> Find<T>(Specification<T> specification) where T : DomainModelBase, IAggregationRoot
        {
            return default(IEnumerable<T>);
        }

        public virtual IEnumerable<T> PageBy<T>(Specification<T> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount, Expression<Func<T, Object>> selector) where T : DomainModelBase, IAggregationRoot
        {
            totalCount = 0;

            return default(IEnumerable<T>);
        }

        public virtual IEnumerable<T> PageBy<T>(Specification<T> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount) where T : DomainModelBase, IAggregationRoot
        {
            totalCount = 0;

            return default(IEnumerable<T>);
        }
    }
}
