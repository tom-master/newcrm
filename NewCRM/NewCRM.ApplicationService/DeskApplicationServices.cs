using System;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustemException;
using NewCRM.QueryServices.DomainSpecification;

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

            var accountConfig = AccountQuery.Find(new Specification<Account>(account => account.Id == accountId)).FirstOrDefault()?.Config;


            foreach (var desk in accountConfig.Desks)
            {
                var members = desk.Members;
                if (isFolder)
                {
                    var folderMember = members.FirstOrDefault(member => member.Id == memberId && member.MemberType == MemberType.Folder);
                    if (folderMember != null)
                    {
                        return folderMember.ConvertToDto<Member, MemberDto>();
                    }
                }
                else
                {
                    var appMember = members.FirstOrDefault(member => member.AppId == memberId && member.MemberType == MemberType.App);
                    if (appMember != null)
                    {
                        return appMember.ConvertToDto<Member, MemberDto>();
                    }
                }
            }
            throw new BusinessException($"未找到app");
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
