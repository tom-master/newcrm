using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.Factory.DomainSpecification;

namespace NewCRM.Domain.Factory.DomainQuery.Query
{
    public abstract class QueryBase : IQuery
    {
        public virtual U FindOne<T, U>(Specification<T> specification, Expression<Func<T, U>> selector) where T : DomainModelBase
        {
            return default(U);
        }

        public virtual T FindOne<T>(Specification<T> specification) where T : DomainModelBase
        {
            return default(T);
        }

        public virtual IEnumerable<U> Find<T, U>(Specification<T> specification, Expression<Func<T, U>> selector) where T : DomainModelBase
        {
            return default(IEnumerable<U>);
        }

        public virtual IEnumerable<U> PageBy<T, U>(Specification<T> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount, Expression<Func<T, U>> selector) where T : DomainModelBase
        {
            totalCount = 0;
            return default(IEnumerable<U>);
        }

    }
}
