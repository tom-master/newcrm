using System;
using NewCRM.Dto; 

namespace NewCRM.Application.Services.Interface
{
    public interface IDeskServices
    {
        #region have return value


        /// <summary>
        /// 根据用户id获取桌面的成员
        /// </summary>
        MemberDto GetMember(Int32 accountId, Int32 memberId, Boolean isFolder = default(Boolean));

        #endregion

        #region not have return value

        /// <summary>
        /// 修改默认显示的桌面
        /// </summary>
        void ModifyDefaultDeskNumber(Int32 accountId, Int32 newDefaultDeskNumber);

        /// <summary>
        /// 修改码头的位置
        /// </summary>
        void ModifyDockPosition(Int32 accountId, Int32 defaultDeskNumber, String newPosition);

        /// <summary>
        /// 桌面成员移动到码头中
        /// </summary>
        void MemberInDock(Int32 accountId, Int32 memberId);

        /// <summary>
        /// 桌面成员移出码头
        /// </summary>
        void MemberOutDock(Int32 accountId, Int32 memberId, Int32 deskId);

        /// <summary>
        /// 成员从码头移动到文件夹中
        /// </summary>
        void DockToFolder(Int32 accountId, Int32 memberId, Int32 folderId);

        /// <summary>
        /// 成员从文件夹中移动到码头中
        /// </summary>
        void FolderToDock(Int32 accountId, Int32 memberId);

        /// <summary>
        /// 成员从桌面中移动到文件夹
        /// </summary>
        void DeskToFolder(Int32 accountId, Int32 memberId, Int32 folderId);

        /// <summary>
        /// 成员从文件夹移动到桌面
        /// </summary>
        void FolderToDesk(Int32 accountId, Int32 memberId, Int32 deskId);

        /// <summary>
        /// 成员从文件夹移动到另一个文件夹
        /// </summary>
        void FolderToOtherFolder(Int32 accountId, Int32 memberId, Int32 folderId);

        /// <summary>
        /// 成员从桌面移动到另一个桌面
        /// </summary>
        void DeskToOtherDesk(Int32 accountId, Int32 memberId, Int32 deskId);

        /// <summary>
        /// 修改文件夹信息
        /// </summary>
        void ModifyFolderInfo(Int32 accountId, String memberName, String memberIcon, Int32 memberId);

        /// <summary>
        /// 移除用户的桌面app成员
        /// </summary>
        void RemoveMember(Int32 accountId, Int32 memberId);

        /// <summary>
        /// 修改成员信息
        /// </summary>
        void ModifyMemberInfo(Int32 accountId, MemberDto member);

        /// <summary>
        /// 创建新的文件夹
        /// </summary>
        void CreateNewFolder(String folderName, String folderImg, Int32 deskId,Int32 accountId);

        /// <summary>
        /// 从码头移动到另一个桌面
        /// </summary>
        void DockToOtherDesk(Int32 accountId, Int32 memberId, Int32 deskId);

        /// <summary>
        /// 更新桌面成员的图标
        /// </summary>
        void ModifyMemberIcon(Int32 accountId, Int32 memberId, String newIcon);

        #endregion
    }
}
