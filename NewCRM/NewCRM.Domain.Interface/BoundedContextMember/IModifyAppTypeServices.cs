using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Domain.Interface.BoundedContextMember
{

    public interface IModifyAppTypeServices
    {
        void DeleteAppType(Int32 appTypeId);
    }
}
