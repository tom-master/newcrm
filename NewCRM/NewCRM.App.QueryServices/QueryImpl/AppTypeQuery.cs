using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.Repositories.IRepository.System;
using NewCRM.QueryServices.DomainSpecification;

namespace NewCRM.QueryServices.QueryImpl
{
    [InheritedExport(typeof(IAppTypeQuery))]
    public sealed class AppTypeQuery : IAppTypeQuery
    {
        [Import]
        private IAppTypeRepository AppTypeRepository { get; set; }

        public IEnumerable<AppType> Find(ISpecification<AppType> specification)
        {
            return AppTypeRepository.Entities.Where(appType => specification.IsSatisfiedBy(appType)).ToList();
        }
    }
}
