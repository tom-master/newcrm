using System;
using NewCRM.Domain.Entitys.System;

namespace NewCRM.Domain.Interface.BoundedContextMember
{
    public interface IModifyDeskMemberServices
    {
        /// <summary>
        /// 修改文件夹的信息
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="memberIcon"></param>
        /// <param name="memberId"></param>
        void ModifyFolderInfo(String memberName, String memberIcon, Int32 memberId);

        /// <summary>
        /// 修改成员信息
        /// </summary>
        /// <param name="member"></param>
        void ModifyMemberInfo(Member member);

        /// <summary>
        /// 移除用户的桌面app成员
        /// </summary>
        /// <param name="memberId"></param>
        void RemoveMember(Int32 memberId);
    }
}
