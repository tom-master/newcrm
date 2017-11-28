using System;
using NewCRM.Application.Services.Interface;
using NewCRM.Domain;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Dto;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Application.Services
{
    public class DeskServices : BaseServiceContext, IDeskServices
    {
        private readonly IModifyDeskMemberServices _modifyDeskMemberServices;
        private readonly IModifyDockPostionServices _modifyDockPostionServices;
        private readonly ICreateNewFolderServices _createNewFolderServices;
        private readonly IMemberContext _memberContext;
        private readonly IDeskContext _deskContext;

        public DeskServices(IModifyDeskMemberServices modifyDeskMemberServices,
            IModifyDockPostionServices modifyDockPostionServices,
            ICreateNewFolderServices createNewFolderServices,
            IMemberContext memberContext,
            IDeskContext deskContext)
        {
            _modifyDeskMemberServices = modifyDeskMemberServices;
            _modifyDockPostionServices = modifyDockPostionServices;
            _createNewFolderServices = createNewFolderServices;
            _memberContext = memberContext;
            _deskContext = deskContext;
        }

        public MemberDto GetMember(Int32 accountId, Int32 memberId, Boolean isFolder)
        {
            ValidateParameter.Validate(accountId).Validate(memberId);

            var result = _memberContext.GetMember(accountId, memberId, isFolder);
            if(result == null)
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
                IconUrl = result.IconUrl,
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
                Width = result.Width
            };
        }

        public void ModifyDefaultDeskNumber(Int32 accountId, Int32 newDefaultDeskNumber)
        {
            ValidateParameter.Validate(accountId).Validate(newDefaultDeskNumber);
            _deskContext.ModifyDefaultDeskNumber(accountId, newDefaultDeskNumber);
        }

        public void ModifyDockPosition(Int32 accountId, Int32 defaultDeskNumber, String newPosition)
        {
            ValidateParameter.Validate(accountId).Validate(defaultDeskNumber).Validate(newPosition);
            _deskContext.ModifyDockPosition(accountId, defaultDeskNumber, newPosition);
        }

        public void MemberInDock(Int32 accountId, Int32 memberId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId);
            _deskContext.MemberInDock(accountId, memberId);
        }

        public void MemberOutDock(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(deskId);
            _deskContext.MemberOutDock(accountId, memberId, deskId);
        }

        public void DockToFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(folderId);
            _deskContext.DockToFolder(accountId, memberId, folderId);
        }

        public void FolderToDock(Int32 accountId, Int32 memberId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId);
            _deskContext.FolderToDock(accountId, memberId);
        }

        public void DeskToFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(folderId);
            _deskContext.DeskToFolder(accountId, memberId, folderId);
        }

        public void FolderToDesk(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(deskId);
            _deskContext.FolderToDesk(accountId, memberId, deskId);
        }

        public void FolderToOtherFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(folderId);
            _deskContext.FolderToOtherFolder(accountId, memberId, folderId);
        }

        public void DeskToOtherDesk(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(deskId);
            _deskContext.DeskToOtherDesk(accountId, memberId, deskId);
        }

        public void ModifyFolderInfo(Int32 accountId, String memberName, String memberIcon, Int32 memberId)
        {
            ValidateParameter.Validate(accountId).Validate(memberName).Validate(memberIcon).Validate(memberId);
            _modifyDeskMemberServices.ModifyFolderInfo(accountId, memberName, memberIcon, memberId);
        }

        public void RemoveMember(Int32 accountId, Int32 memberId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId);
            _modifyDeskMemberServices.RemoveMember(accountId, memberId);
        }

        public void ModifyMemberInfo(Int32 accountId, MemberDto member)
        {
            ValidateParameter.Validate(accountId).Validate(member);
            _modifyDeskMemberServices.ModifyMemberInfo(accountId, member.ConvertToModel<MemberDto, Member>());
        }

        public void CreateNewFolder(String folderName, String folderImg, Int32 deskId)
        {
            ValidateParameter.Validate(folderName).Validate(folderImg).Validate(deskId);
            _createNewFolderServices.NewFolder(deskId, folderName, folderImg);
        }

        public void DockToOtherDesk(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(deskId);
            _deskContext.DockToOtherDesk(accountId, memberId, deskId);
        }

        public void ModifyMemberIcon(Int32 accountId, Int32 memberId, String newIcon)
        {
            ValidateParameter.Validate(memberId).Validate(newIcon);
            _modifyDeskMemberServices.ModifyMemberIcon(accountId, memberId, newIcon);
        }
    }
}
