using System;
using System.Collections.Generic;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.QueryServices.DomainSpecification;
using NewCRM.QueryServices.Query;

namespace NewCRM.QueryServices
{
    public interface IAccountQuery : IQuery<Account>
    {
        IEnumerable<Account> PageBy(ISpecification<Account> specification , Int32 pageIndex, Int32 pageSize, out Int32 totalCount);
    }
}
