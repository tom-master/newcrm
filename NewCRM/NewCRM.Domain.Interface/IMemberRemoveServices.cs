using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Domain.Interface
{
    public interface IMemberRemoveServices
    {
        /// <summary>
        /// 移除用户的桌面app成员
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="memberId"></param>
        void RemoveMember(Int32 accountId, Int32 memberId);
    }
}
