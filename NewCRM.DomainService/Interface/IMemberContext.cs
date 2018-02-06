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

        /// <summary>
        /// 检查成员名称
        /// </summary>
        Boolean CheckMemberName(String name);

        /// <summary>
        /// 修改文件夹的信息
        /// </summary>
        void ModifyFolderInfo(Int32 accountId, String memberName, String memberIcon, Int32 memberId);

        /// <summary>
        /// 修改成员信息
        /// </summary>
        void ModifyMemberInfo(Int32 accountId, Member member);

        /// <summary>
        /// 卸载用户的桌面app成员
        /// </summary>
        void UninstallMember(Int32 accountId, Int32 memberId);

        /// <summary>
        ///修改桌面成员的Icon
        /// </summary>
        void ModifyMemberIcon(Int32 accountId, Int32 memberId, String newIcon);
    }
}
