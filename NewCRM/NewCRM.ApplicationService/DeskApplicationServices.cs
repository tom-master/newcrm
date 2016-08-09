using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Services;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustomHelper;

namespace NewCRM.Application.Services
{
    [Export(typeof(IDeskApplicationServices))]
    internal class DeskApplicationServices : BaseApplicationServices, IDeskApplicationServices
    {
        public void ModifyDefaultDeskNumber(Int32 userId, Int32 newDefaultDeskNumber)
        {
            ValidateParameter.Validate(userId).Validate(newDefaultDeskNumber);
            DeskServices.ModifyDefaultShowDesk(userId, newDefaultDeskNumber);
        }


        public void ModifyDockPosition(Int32 userId, Int32 defaultDeskNumber, String newPosition)
        {
            ValidateParameter.Validate(userId).Validate(defaultDeskNumber).Validate(newPosition);

            DeskServices.ModifyDockPosition(userId, defaultDeskNumber, newPosition);
        }

        public MemberDto GetMember(Int32 userId, Int32 memberId, Boolean isFolder = default(Boolean))
        {
            ValidateParameter.Validate(userId).Validate(memberId);
            return DeskServices.GetMember(userId, memberId, isFolder).ConvertToDto<Member, MemberDto>();
        }

        public void MemberInDock(Int32 userId, Int32 memberId)
        {
            ValidateParameter.Validate(userId).Validate(memberId);
            DeskServices.MemberInDock(userId, memberId);
        }

        public void MemberOutDock(Int32 userId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(userId).Validate(memberId);
            DeskServices.MemberOutDock(userId, memberId, deskId);
        }

        public void DockToFolder(Int32 userId, Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(userId).Validate(memberId).Validate(folderId);
            DeskServices.DockToFolder(userId, memberId, folderId);
        }

        public void FolderToDock(Int32 userId, Int32 memberId)
        {
            ValidateParameter.Validate(userId).Validate(memberId);

            DeskServices.FolderToDock(userId, memberId);
        }

        public void DeskToFolder(Int32 userId, Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(userId).Validate(memberId).Validate(folderId);
            DeskServices.DeskToFolder(userId, memberId, folderId);
        }

        public void FolderToDesk(Int32 userId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(userId).Validate(memberId).Validate(deskId);
            DeskServices.FolderToDesk(userId, memberId, deskId);
        }

        public void FolderToOtherFolder(Int32 userId, Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(userId).Validate(memberId).Validate(folderId);
            DeskServices.FolderToOtherFolder(userId, memberId, folderId);
        }

        public void DeskToOtherDesk(Int32 userId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(userId).Validate(memberId).Validate(deskId);
            DeskServices.DeskToOtherDesk(userId, memberId, deskId);
        }

        public void ModifyFolderInfo(String memberName, String memberIcon, Int32 memberId, Int32 userId)
        {
            ValidateParameter.Validate(memberName).Validate(memberIcon).Validate(memberId).Validate(userId);
            DeskServices.ModifyFolderInfo(memberName, memberIcon, memberId, userId);
        }

        public void RemoveMember(Int32 userId, Int32 memberId)
        {
            ValidateParameter.Validate(userId).Validate(memberId);
            DeskServices.RemoveMember(userId, memberId);
        }

        public void ModifyMemberInfo(Int32 userId, MemberDto member)
        {
            ValidateParameter.Validate(userId).Validate(member);

            DeskServices.ModifyMemberInfo(userId, member.ConvertToModel<MemberDto, Member>());
        }
    }
}
