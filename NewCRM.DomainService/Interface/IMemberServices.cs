using System;
using System.Collections.Generic;
using NewCRM.Domain.Entitys.System;

namespace NewCRM.Domain.Services.Interface
{
    public interface IMemberServices
    {
        /// <summary>
        /// 获取桌面成员
        /// </summary>
        /// <returns></returns>
        List<Member> GetDeskMembers(Int32 accountId);
    }
}
