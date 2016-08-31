using System;
using System.Collections.Generic;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.QueryServices.DomainSpecification;
using NewCRM.QueryServices.Query;

namespace NewCRM.QueryServices
{
    public interface IAppQuery : IQuery<App>
    {
        IEnumerable<App> PageBy(ISpecification<App> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);
    }
}
