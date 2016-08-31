using System;
using System.Collections.Generic;
using NewCRM.Domain.Entities.DomainModel.Security;
using NewCRM.QueryServices.DomainSpecification;
using NewCRM.QueryServices.Query;

namespace NewCRM.QueryServices
{
    public interface IRoleQuery : IQuery<Role>
    {
        IEnumerable<Role> PageBy(ISpecification<Role> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);
    }
}
