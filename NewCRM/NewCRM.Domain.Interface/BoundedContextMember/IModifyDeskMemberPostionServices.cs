using System;

namespace NewCRM.Domain.Interface.BoundedContextMember
{
    public interface IModifyDeskMemberPostionServices
    {
        /// <summary>
        /// 桌面成员移动到码头中
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="memberId"></param>
        void MemberInDock(Int32 accountId, Int32 memberId);

        /// <summary>
        /// 桌面成员移出码头中
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="memberId"></param>
        /// <param name="deskId"></param>
        void MemberOutDock(Int32 accountId, Int32 memberId, Int32 deskId);

        /// <summary>
        /// 成员从码头移动到文件夹中
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="memberId"></param>
        /// <param name="folderId"></param>
        void DockToFolder(Int32 accountId, Int32 memberId, Int32 folderId);

        /// <summary>
        /// 成员从文件夹中移动到码头
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="memberId"></param>
        void FolderToDock(Int32 accountId, Int32 memberId);

        /// <summary>
        /// 成员从桌面中移动到文件夹
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="memberId"></param>
        /// <param name="folderId"></param>
        void DeskToFolder(Int32 accountId, Int32 memberId, Int32 folderId);

        /// <summary>
        /// 成员从文件夹移动到桌面
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="memberId"></param>
        /// <param name="deskId"></param>
        void FolderToDesk(Int32 accountId, Int32 memberId, Int32 deskId);

        /// <summary>
        /// 成员从文件夹移动到另一个文件夹
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="memberId"></param>
        /// <param name="folderId"></param>
        void FolderToOtherFolder(Int32 accountId, Int32 memberId, Int32 folderId);

        /// <summary>
        /// 成员从桌面移动到另一个桌面
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="memberId"></param>
        /// <param name="deskId"></param>
        void DeskToOtherDesk(Int32 accountId, Int32 memberId, Int32 deskId);

    }
}
