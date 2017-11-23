using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Domain.Services.Interface
{
    public interface IDeskContext
    {
        /// <summary>
        /// 修改成员排列方向X轴
        /// </summary>
        void ModifyMemberDirectionToX(Int32 accountId);

        /// <summary>
        /// 修改成员排列方向X轴
        /// </summary>
        void ModifyMemberDirectionToY(Int32 accountId);
    }
}
