using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.Repositories;
using NewCRM.Domain.Entities.Repositories.IRepository.System;
using NewCRM.QueryServices.DomainSpecification;
using NewCRM.QueryServices.Query;

namespace NewCRM.QueryServices.QueryImpl.ConcreteQuery
{
    public sealed class AppTypeQuery : IQuery<AppType>
    {
        [Import]
        private IRepository<AppType> AppTypeRepository { get; set; }

        public IEnumerable<AppType> Find(ISpecification<AppType> specification)
        {
            return AppTypeRepository.Entities.Where(appType => specification.IsSatisfiedBy(appType)).ToList();
        }

        public IEnumerable<AppType> PageBy(ISpecification<AppType> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            throw new NotImplementedException();
        }
    }
}
