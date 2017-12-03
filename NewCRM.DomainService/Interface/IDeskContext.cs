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


        /// <summary>
        /// 修改成员图标显示大小
        /// </summary>
        void ModifyMemberDisplayIconSize(Int32 accountId, Int32 newSize);

        /// <summary>
        /// 修改成员之间的垂直间距
        /// </summary>
        void ModifyMemberVerticalSpacing(Int32 accountId, Int32 newSize);

        /// <summary>
        /// 修改成员之间的水平间距
        /// </summary>
        void ModifyMemberHorizontalSpacing(Int32 accountId, Int32 newSize);

        /// <summary>
        /// 修改默认显示的桌面编号
        /// </summary>
        void ModifyDefaultDeskNumber(Int32 accountId, Int32 newDefaultDeskNumber);

        /// <summary>
        /// 修改码头位置
        /// </summary>
        void ModifyDockPosition(Int32 accountId, Int32 defaultDeskNumber, String newPosition);

        /// <summary>
	    /// 桌面成员移动到码头中
	    /// </summary>
	    void MemberInDock(Int32 accountId, Int32 memberId);

        /// <summary>
        /// 桌面成员移出码头中
        /// </summary>
        void MemberOutDock(Int32 accountId, Int32 memberId, Int32 deskId);

        /// <summary>
        /// 成员从码头移动到文件夹中
        /// </summary>
        void DockToFolder(Int32 accountId, Int32 memberId, Int32 folderId);

        /// <summary>
        /// 成员从文件夹中移动到码头
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
        /// 成员从码头移动到桌面
        /// </summary>
        void DockToOtherDesk(Int32 accountId, Int32 memberId, Int32 deskId);

        /// <summary>
        /// 创建文件夹
        /// </summary>
        void CreateNewFolder(Int32 deskId, String folderName, String folderImg, Int32 accountId);

        /// <summary>
        /// 修改壁纸来源
        /// </summary>
        void ModifyWallpaperSource(String source, Int32 accountId);
    }
}
