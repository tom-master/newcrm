using System;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Application.Interface;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Application.Services
{
    [Export(typeof(IDeskApplicationServices))]
    public class DeskApplicationServices : BaseServices.BaseServices, IDeskApplicationServices
    {
        public void ModifyDefaultDeskNumber(Int32 accountId, Int32 newDefaultDeskNumber)
        {
            ValidateParameter.Validate(accountId).Validate(newDefaultDeskNumber);

            var accountResult = GetAccountInfoService(accountId);

            accountResult.Config.ModifyDefaultDesk(newDefaultDeskNumber);

            Repository.Create<Account>().Update(accountResult);

            UnitOfWork.Commit();
        }

        public void ModifyDockPosition(Int32 accountId, Int32 defaultDeskNumber, String newPosition)
        {
            ValidateParameter.Validate(accountId).Validate(defaultDeskNumber).Validate(newPosition);

            DeskContext.ModifyDockPostionServices.ModifyDockPosition(accountId, defaultDeskNumber, newPosition);

            UnitOfWork.Commit();
        }

        public MemberDto GetMember(Int32 accountId, Int32 memberId, Boolean isFolder = default(Boolean))
        {
            ValidateParameter.Validate(accountId).Validate(memberId);

            var accountResult = GetAccountInfoService(accountId);

            if (accountResult == null)
            {
                throw new BusinessException("该用户可能已被禁用或被删除，请联系管理员");
            }

            var accountConfig = accountResult.Config;

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

            AccountContext.ModifyAccountConfigServices.MemberInDock(accountId, memberId);

            UnitOfWork.Commit();
        }

        public void MemberOutDock(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId);

            AccountContext.ModifyAccountConfigServices.MemberOutDock(accountId, memberId, deskId);

            UnitOfWork.Commit();
        }

        public void DockToFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(folderId);

            AccountContext.ModifyAccountConfigServices.DockToFolder(accountId, memberId, folderId);

            UnitOfWork.Commit();
        }

        public void FolderToDock(Int32 accountId, Int32 memberId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId);

            AccountContext.ModifyAccountConfigServices.FolderToDock(accountId, memberId);

            UnitOfWork.Commit();
        }

        public void DeskToFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(folderId);

            AccountContext.ModifyAccountConfigServices.DeskToFolder(accountId, memberId, folderId);

            UnitOfWork.Commit();
        }

        public void FolderToDesk(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(deskId);

            AccountContext.ModifyAccountConfigServices.FolderToDesk(accountId, memberId, deskId);

            UnitOfWork.Commit();
        }

        public void FolderToOtherFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(folderId);

            AccountContext.ModifyAccountConfigServices.FolderToOtherFolder(accountId, memberId, folderId);

            UnitOfWork.Commit();
        }

        public void DeskToOtherDesk(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(deskId);

            AccountContext.ModifyAccountConfigServices.DeskToOtherDesk(accountId, memberId, deskId);

            UnitOfWork.Commit();
        }

        public void ModifyFolderInfo(String memberName, String memberIcon, Int32 memberId, Int32 accountId)
        {
            ValidateParameter.Validate(memberName).Validate(memberIcon).Validate(memberId).Validate(accountId);

            DeskContext.ModifyDeskMemberServices.ModifyFolderInfo(memberName, memberIcon, memberId, accountId);

            UnitOfWork.Commit();
        }

        public void RemoveMember(Int32 accountId, Int32 memberId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId);

            DeskContext.ModifyDeskMemberServices.RemoveMember(accountId, memberId);

            UnitOfWork.Commit();
        }

        public void ModifyMemberInfo(Int32 accountId, MemberDto member)
        {
            ValidateParameter.Validate(accountId).Validate(member);

            DeskContext.ModifyDeskMemberServices.ModifyMemberInfo(accountId, member.ConvertToModel<MemberDto, Member>());

            UnitOfWork.Commit();
        }
    }
}
