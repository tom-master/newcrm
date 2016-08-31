using System;
using System.ComponentModel.Composition;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Dto;
using NewCRM.Dto.Dto;

namespace NewCRM.Application.Services
{
    [Export(typeof(IDeskApplicationServices))]
    internal class DeskApplicationServices : BaseApplicationServices, IDeskApplicationServices
    {
        public void ModifyDefaultDeskNumber(Int32 accountId, Int32 newDefaultDeskNumber)
        {
            ValidateParameter.Validate(accountId).Validate(newDefaultDeskNumber);

            AccountContext.ConfigServices.ModifyDefaultShowDesk(accountId, newDefaultDeskNumber);
        }


        public void ModifyDockPosition(Int32 accountId, Int32 defaultDeskNumber, String newPosition)
        {
            ValidateParameter.Validate(accountId).Validate(defaultDeskNumber).Validate(newPosition);

            AccountContext.ConfigServices.ModifyDockPosition(accountId, defaultDeskNumber, newPosition);
        }

        public MemberDto GetMember(Int32 accountId, Int32 memberId, Boolean isFolder = default(Boolean))
        {
            ValidateParameter.Validate(accountId).Validate(memberId);

            return AccountContext.ConfigServices.GetMember(accountId, memberId, isFolder).ConvertToDto<Member, MemberDto>();
        }

        public void MemberInDock(Int32 accountId, Int32 memberId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId);

            AccountContext.ConfigServices.MemberInDock(accountId, memberId);
        }

        public void MemberOutDock(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId);

            AccountContext.ConfigServices.MemberOutDock(accountId, memberId, deskId);
        }

        public void DockToFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(folderId);

            AccountContext.ConfigServices.DockToFolder(accountId, memberId, folderId);
        }

        public void FolderToDock(Int32 accountId, Int32 memberId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId);

            AccountContext.ConfigServices.FolderToDock(accountId, memberId);
        }

        public void DeskToFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(folderId);

            AccountContext.ConfigServices.DeskToFolder(accountId, memberId, folderId);
        }

        public void FolderToDesk(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(deskId);

            AccountContext.ConfigServices.FolderToDesk(accountId, memberId, deskId);
        }

        public void FolderToOtherFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(folderId);

            AccountContext.ConfigServices.FolderToOtherFolder(accountId, memberId, folderId);
        }

        public void DeskToOtherDesk(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(deskId);

            AccountContext.ConfigServices.DeskToOtherDesk(accountId, memberId, deskId);
        }

        public void ModifyFolderInfo(String memberName, String memberIcon, Int32 memberId, Int32 accountId)
        {
            ValidateParameter.Validate(memberName).Validate(memberIcon).Validate(memberId).Validate(accountId);

            AccountContext.ConfigServices.ModifyFolderInfo(memberName, memberIcon, memberId, accountId);
        }

        public void RemoveMember(Int32 accountId, Int32 memberId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId);

            AccountContext.ConfigServices.RemoveMember(accountId, memberId);
        }

        public void ModifyMemberInfo(Int32 accountId, MemberDto member)
        {
            ValidateParameter.Validate(accountId).Validate(member);

            AccountContext.ConfigServices.ModifyMemberInfo(accountId, member.ConvertToModel<MemberDto, Member>());
        }
    }
}
