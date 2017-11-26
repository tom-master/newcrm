using System;
using System.Collections.Generic;
using NewCRM.Domain.Entitys.System;

namespace NewCRM.Domain.Services.Interface
{
    public interface IMemberContext
    {
        /// <summary>
        /// 获取桌面成员列表
        /// </summary>
        /// <returns></returns>
        List<Member> GetMembers(Int32 accountId);

        /// <summary>
        /// 获取桌面成员
        /// </summary>
        /// <returns></returns>
        Member GetMember(Int32 accountId, Int32 memberId, Boolean isFolder);
    }
}
