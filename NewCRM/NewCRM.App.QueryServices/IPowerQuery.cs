using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entities.DomainModel.Security;
using NewCRM.QueryServices.DomainSpecification;
using NewCRM.QueryServices.Query;

namespace NewCRM.QueryServices
{
    public interface IPowerQuery : IQuery<Power>
    {
        IEnumerable<Power> PageBy(ISpecification<Power> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);
    }
}
