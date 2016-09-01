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
    public sealed class PowerQuery : IQuery<Power>
    {
        [Export]
        private IRepository<Power> PowerRepository { get; set; }

        public IEnumerable<Power> Find(ISpecification<Power> specification)
        {
            return PowerRepository.Entities.Where(power => specification.IsSatisfiedBy(power)).ToList();
        }

        public IEnumerable<Power> PageBy(ISpecification<Power> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            var query = PowerRepository.Entities.Where(account => specification.IsSatisfiedBy(account)).PageBy(pageIndex, pageSize, d => d.AddTime);
            totalCount = query.Count();
            return query.ToList();
        }
    }
}
