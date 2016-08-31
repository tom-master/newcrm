using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.Repositories.IRepository.Account;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.QueryServices.DomainSpecification;

namespace NewCRM.QueryServices.QueryImpl
{
    [InheritedExport(typeof(IAccountQuery))]
    internal sealed class AccountQuery : IAccountQuery
    {
        [Import]
        private IAccountRepository AccountRepository { get; set; }

        public IEnumerable<Account> Find(ISpecification<Account> specification)
        {
            return AccountRepository.Entities.Where(account => specification.IsSatisfiedBy(account)).ToList();
        }


        public IEnumerable<Account> PageBy(ISpecification<Account> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            var query = AccountRepository.Entities.Where(account => specification.IsSatisfiedBy(account)).PageBy(pageIndex, pageSize, d => d.AddTime);
            totalCount = query.Count();
            return query.ToList();
        }
    }
}
