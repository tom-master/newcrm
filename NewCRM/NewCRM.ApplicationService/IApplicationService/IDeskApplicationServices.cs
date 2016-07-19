using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Dto.Dto;

namespace NewCRM.Application.Services.IApplicationService
{
    public interface IDeskApplicationServices
    {
        #region desk

        /// <summary>
        /// 修改默认显示的桌面
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newDefaultDeskNumber"></param>
        void ModifyDefaultDeskNumber(Int32 userId, Int32 newDefaultDeskNumber);

        /// <summary>
        /// 修改码头的位置
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="defaultDeskNumber"></param>
        /// <param name="newPosition"></param>
        void ModifyDockPosition(Int32 userId, Int32 defaultDeskNumber, String newPosition);

        /// <summary>
        /// 根据用户id获取桌面的成员
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="memberId"></param>
        /// <param name="isFolder"></param>
        /// <returns></returns>
        MemberDto GetMember(Int32 userId, Int32 memberId, Boolean isFolder = default(Boolean));

        /// <summary>
        /// 桌面成员移动到码头中
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="memberId"></param>
        void MemberInDock(Int32 userId, Int32 memberId);

        /// <summary>
        /// 桌面成员移出码头
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="memberId"></param>
        void MemberOutDock(Int32 userId, Int32 memberId);

        /// <summary>
        /// 成员从码头移动到文件夹中
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="memberId"></param>
        /// <param name="folderId"></param>
        void DockToFolder(Int32 userId, Int32 memberId, Int32 folderId);

        #endregion
    }
}
