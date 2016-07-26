﻿using System;
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
        /// <param name="deskId"></param>
        void MemberOutDock(Int32 userId, Int32 memberId, Int32 deskId);

        /// <summary>
        /// 成员从码头移动到文件夹中
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="memberId"></param>
        /// <param name="folderId"></param>
        void DockToFolder(Int32 userId, Int32 memberId, Int32 folderId);

        /// <summary>
        /// 成员从文件夹中移动到码头中
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="memberId"></param>
        void FolderToDock(Int32 userId, Int32 memberId);

        /// <summary>
        /// 成员从桌面中移动到文件夹
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="memberId"></param>
        /// <param name="folderId"></param>
        void DeskToFolder(Int32 userId, Int32 memberId, Int32 folderId);

        /// <summary>
        /// 成员从文件夹移动到桌面
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="memberId"></param>
        void FolderToDesk(Int32 userId, Int32 memberId);

        /// <summary>
        /// 成员从文件夹移动到另一个文件夹
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="memberId"></param>
        /// <param name="folderId"></param>
        void FolderToOtherFolder(Int32 userId, Int32 memberId, Int32 folderId);

        /// <summary>
        /// 成员从桌面移动到另一个桌面
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="memberId"></param>
        /// <param name="deskId"></param>
        void DeskToOtherDesk(Int32 userId, Int32 memberId, Int32 deskId);

        /// <summary>
        /// 修改文件夹信息
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="memberIcon"></param>
        /// <param name="memberId"></param>
        /// <param name="userId"></param>
        void ModifyFolderInfo(String memberName, String memberIcon, Int32 memberId, Int32 userId);


        /// <summary>
        /// 移除用户的桌面app成员
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="memberId"></param>
        void RemoveMember(Int32 userId, Int32 memberId);

        #endregion
    }
}