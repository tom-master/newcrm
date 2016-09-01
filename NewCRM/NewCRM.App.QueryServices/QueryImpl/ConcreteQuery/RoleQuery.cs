using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.Security;
using NewCRM.Domain.Entities.Repositories;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.QueryServices.DomainSpecification;
using NewCRM.QueryServices.Query;

namespace NewCRM.QueryServices.QueryImpl.ConcreteQuery
{
    public sealed class RoleQuery : IQuery<Role>
    {
        [Export]
        private IRepository<Role> RoleRepository { get; set; }

        public IEnumerable<Role> Find(ISpecification<Role> specification)
        {
            return RoleRepository.Entities.Where(role => specification.IsSatisfiedBy(role)).ToList();
        }

        public IEnumerable<Role> PageBy(ISpecification<Role> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            var query = RoleRepository.Entities.Where(account => specification.IsSatisfiedBy(account)).PageBy(pageIndex, pageSize, d => d.AddTime);
            totalCount = query.Count();
            return query.ToList();
        }
    }
}
