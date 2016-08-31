using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.Security;
using NewCRM.Domain.Entities.Repositories.IRepository.Security;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.QueryServices.DomainSpecification;

namespace NewCRM.QueryServices.QueryImpl
{
    [InheritedExport(typeof(IPowerQuery))]
    public sealed class PowerQuery : IPowerQuery
    {
        [Export]
        private IPowerRepository PowerRepository { get; set; }

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
