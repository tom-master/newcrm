using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entities.DomainModel.System;

namespace NewCRM.Domain.Interface
{
    public interface IModifyDeskMemberInfoServices
    {
        /// <summary>
        /// 修改文件夹的信息
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="memberIcon"></param>
        /// <param name="memberId"></param>
        /// <param name="accountId"></param>
        void ModifyFolderInfo(String memberName, String memberIcon, Int32 memberId, Int32 accountId);

        /// <summary>
        /// 修改成员信息
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="member"></param>
        void ModifyMemberInfo(Int32 accountId, Member member);
    }
}
