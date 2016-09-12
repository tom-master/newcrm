using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Domain.Interface
{
    public interface IModifyDockPostionServices
    {
        /// <summary>
        /// 修改应用码头的位置
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="defaultDeskNumber"></param>
        /// <param name="newPosition"></param>
        void ModifyDockPosition(Int32 accountId, Int32 defaultDeskNumber, String newPosition);
    }
}
