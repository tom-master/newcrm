using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Application.Services.Interface;
using NewCRM.Domain;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewLib;

namespace NewCRM.Application.Services
{
    public class DeskServices : BaseServiceContext, IDeskServices
    {
        private readonly IMemberContext _memberContext;
        private readonly IDeskContext _deskContext;

        public DeskServices(IMemberContext memberContext, IDeskContext deskContext)
        {
            _memberContext = memberContext;
            _deskContext = deskContext;
        }

        public MemberDto GetMember(Int32 accountId, Int32 memberId, Boolean isFolder)
        {
            ValidateParameter.Validate(accountId).Validate(memberId);
            var result = _memberContext.GetMember(accountId, memberId, isFolder);
            if (result == null)
            {
                throw new BusinessException($"未找到app");
            }
            return new MemberDto
            {
                AppId = result.AppId,
                AppUrl = result.AppUrl,
                DeskIndex = result.DeskIndex,
                FolderId = result.FolderId,
                Height = result.Height,
                IconUrl = result.IsIconByUpload ? ProfileManager.FileUrl + result.IconUrl : result.IconUrl,
                Id = result.Id,
                IsDraw = result.IsDraw,
                IsFlash = result.IsFlash,
                IsFull = result.IsFull,
                IsLock = result.IsLock,
                IsMax = result.IsMax,
                IsOnDock = result.IsOnDock,
                IsOpenMax = result.IsOpenMax,
                IsResize = result.IsResize,
                IsSetbar = result.IsSetbar,
                MemberType = result.MemberType.ToString(),
                Name = result.Name,
                Width = result.Width,
                AccountId = result.AccountId
            };
        }

        public IDictionary<String, IList<dynamic>> GetDeskMembers(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            var result = GetCache(CacheKey.Desktop(accountId), () => _memberContext.GetMembers(accountId));
            var deskGroup = result.GroupBy(a => a.DeskIndex);
            var deskDictionary = new Dictionary<String, IList<dynamic>>();
            foreach (var desk in deskGroup)
            {
                var members = desk.ToList();
                var deskMembers = new List<dynamic>();
                foreach (var member in members)
                {
                    if (member.MemberType == MemberType.Folder)
                    {
                        deskMembers.Add(new
                        {
                            type = member.MemberType.ToString().ToLower(),
                            memberId = member.Id,
                            appId = member.AppId,
                            name = member.Name,
                            icon = member.IsIconByUpload ? ProfileManager.FileUrl + member.IconUrl : member.IconUrl,
                            width = member.Width,
                            height = member.Height,
                            isOnDock = member.IsOnDock,
                            isDraw = member.IsDraw,
                            isOpenMax = member.IsOpenMax,
                            isSetbar = member.IsSetbar,
                            apps = members.Where(m => m.FolderId == member.Id).Select(app => new
                            {
                                type = app.MemberType.ToString().ToLower(),
                                memberId = app.Id,
                                appId = app.AppId,
                                name = app.Name,
                                icon = member.IsIconByUpload ? ProfileManager.FileUrl + member.IconUrl : member.IconUrl,
                                width = app.Width,
                                height = app.Height,
                                isOnDock = app.IsOnDock,
                                isDraw = app.IsDraw,
                                isOpenMax = app.IsOpenMax,
                                isSetbar = app.IsSetbar,
                            })
                        });
                    }
                    else
                    {
                        if (member.FolderId == 0)
                        {
                            var internalType = member.MemberType.ToString().ToLower();
                            deskMembers.Add(new
                            {
                                type = internalType,
                                memberId = member.Id,
                                appId = member.AppId,
                                name = member.Name,
                                icon = member.IsIconByUpload ? ProfileManager.FileUrl + member.IconUrl : member.IconUrl,
                                width = member.Width,
                                height = member.Height,
                                isOnDock = member.IsOnDock,
                                isDraw = member.IsDraw,
                                isOpenMax = member.IsOpenMax,
                                isSetbar = member.IsSetbar
                            });
                        }
                    }
                }
                deskDictionary.Add(desk.Key.ToString(), deskMembers);
            }

            return deskDictionary;
        }

        public void ModifyDefaultDeskNumber(Int32 accountId, Int32 newDefaultDeskNumber)
        {
            ValidateParameter.Validate(accountId).Validate(newDefaultDeskNumber);
            _deskContext.ModifyDefaultDeskNumber(accountId, newDefaultDeskNumber);
            RemoveOldKeyWhenModify(CacheKey.Config(accountId));
        }

        public void ModifyDockPosition(Int32 accountId, Int32 defaultDeskNumber, String newPosition)
        {
            ValidateParameter.Validate(accountId).Validate(defaultDeskNumber).Validate(newPosition);
            _deskContext.ModifyDockPosition(accountId, defaultDeskNumber, newPosition);
            RemoveOldKeyWhenModify(CacheKey.Config(accountId));
        }

        public void MemberInDock(Int32 accountId, Int32 memberId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId);
            _deskContext.MemberInDock(accountId, memberId);
            RemoveOldKeyWhenModify(CacheKey.Desktop(accountId));
        }

        public void MemberOutDock(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(deskId);
            _deskContext.MemberOutDock(accountId, memberId, deskId);
            RemoveOldKeyWhenModify(CacheKey.Desktop(accountId));
        }

        public void DockToFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(folderId);
            _deskContext.DockToFolder(accountId, memberId, folderId);
            RemoveOldKeyWhenModify(CacheKey.Desktop(accountId));
        }

        public void FolderToDock(Int32 accountId, Int32 memberId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId);
            _deskContext.FolderToDock(accountId, memberId);
            RemoveOldKeyWhenModify(CacheKey.Desktop(accountId));
        }

        public void DeskToFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(folderId);
            _deskContext.DeskToFolder(accountId, memberId, folderId);
            RemoveOldKeyWhenModify(CacheKey.Desktop(accountId));
        }

        public void FolderToDesk(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(deskId);
            _deskContext.FolderToDesk(accountId, memberId, deskId);
            RemoveOldKeyWhenModify(CacheKey.Desktop(accountId));
        }

        public void FolderToOtherFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(folderId);
            _deskContext.FolderToOtherFolder(accountId, memberId, folderId);
            RemoveOldKeyWhenModify(CacheKey.Desktop(accountId));
        }

        public void DeskToOtherDesk(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(deskId);
            _deskContext.DeskToOtherDesk(accountId, memberId, deskId);
            RemoveOldKeyWhenModify(CacheKey.Desktop(accountId));
        }

        public void ModifyFolderInfo(Int32 accountId, String memberName, String memberIcon, Int32 memberId)
        {
            ValidateParameter.Validate(accountId).Validate(memberName).Validate(memberIcon).Validate(memberId);
            _memberContext.ModifyFolderInfo(accountId, memberName, memberIcon, memberId);
            RemoveOldKeyWhenModify(CacheKey.Desktop(accountId));
        }

        public void UninstallMember(Int32 accountId, Int32 memberId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId);
            _memberContext.UninstallMember(accountId, memberId);
            RemoveOldKeyWhenModify(CacheKey.Desktop(accountId));
        }

        public void ModifyMemberInfo(Int32 accountId, MemberDto member)
        {
            ValidateParameter.Validate(accountId).Validate(member);
            _memberContext.ModifyMemberInfo(accountId, member.ConvertToModel<MemberDto, Member>());
            RemoveOldKeyWhenModify(CacheKey.Desktop(accountId));
        }

        public void CreateNewFolder(String folderName, String folderImg, Int32 deskId, Int32 accountId)
        {
            ValidateParameter.Validate(folderName).Validate(folderImg).Validate(deskId);
            _deskContext.CreateNewFolder(deskId, folderName, folderImg, accountId);
        }

        public void DockToOtherDesk(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(deskId);
            _deskContext.DockToOtherDesk(accountId, memberId, deskId);
            RemoveOldKeyWhenModify(CacheKey.Desktop(accountId));
        }

        public void ModifyMemberIcon(Int32 accountId, Int32 memberId, String newIcon)
        {
            ValidateParameter.Validate(memberId).Validate(newIcon);
            _memberContext.ModifyMemberIcon(accountId, memberId, newIcon);
            RemoveOldKeyWhenModify(CacheKey.Desktop(accountId));
        }

        public void ModifyWallpaperSource(String source, Int32 accountId)
        {
            ValidateParameter.Validate(source).Validate(accountId);
            _deskContext.ModifyWallpaperSource(source, accountId);
        }
    }
}
