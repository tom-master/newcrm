using System;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Application.Interface;
using NewCRM.Application.Services.Services;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.ValueObject;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Application.Services
{
    [Export(typeof(IDeskApplicationServices))]
    internal class DeskApplicationServices : BaseService, IDeskApplicationServices
    {

        private readonly Int32 _accountId;

        [ImportingConstructor]
        public DeskApplicationServices([Import(typeof(AccountDto))] AccountDto account)
        {
            if (account != null)
            {
                _accountId = account.Id;
            }
        }

        public void ModifyDefaultDeskNumber(Int32 newDefaultDeskNumber)
        {
            ValidateParameter.Validate(_accountId).Validate(newDefaultDeskNumber);

            var accountResult = GetAccountInfoService(_accountId);

            accountResult.Config.ModifyDefaultDesk(newDefaultDeskNumber);

            Repository.Create<Account>().Update(accountResult);

            UnitOfWork.Commit();
        }

        public void ModifyDockPosition(Int32 defaultDeskNumber, String newPosition)
        {
            ValidateParameter.Validate(_accountId).Validate(defaultDeskNumber).Validate(newPosition);

            DeskContext.ModifyDockPostionServices.ModifyDockPosition(_accountId, defaultDeskNumber, newPosition);

            UnitOfWork.Commit();
        }

        public MemberDto GetMember(Int32 memberId, Boolean isFolder = default(Boolean))
        {
            ValidateParameter.Validate(_accountId).Validate(memberId);

            var desks = Query.Find(FilterFactory.Create((Desk d) => d.AccountId == _accountId));

            foreach (var desk in desks)
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

        public void MemberInDock(Int32 memberId)
        {
            ValidateParameter.Validate(_accountId).Validate(memberId);

            AccountContext.ModifyAccountConfigServices.MemberInDock(_accountId, memberId);

            UnitOfWork.Commit();
        }

        public void MemberOutDock(Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(_accountId).Validate(memberId);

            AccountContext.ModifyAccountConfigServices.MemberOutDock(_accountId, memberId, deskId);

            UnitOfWork.Commit();
        }

        public void DockToFolder(Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(_accountId).Validate(memberId).Validate(folderId);

            AccountContext.ModifyAccountConfigServices.DockToFolder(_accountId, memberId, folderId);

            UnitOfWork.Commit();
        }

        public void FolderToDock(Int32 memberId)
        {
            ValidateParameter.Validate(_accountId).Validate(memberId);

            AccountContext.ModifyAccountConfigServices.FolderToDock(_accountId, memberId);

            UnitOfWork.Commit();
        }

        public void DeskToFolder(Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(_accountId).Validate(memberId).Validate(folderId);

            AccountContext.ModifyAccountConfigServices.DeskToFolder(_accountId, memberId, folderId);

            UnitOfWork.Commit();
        }

        public void FolderToDesk(Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(_accountId).Validate(memberId).Validate(deskId);

            AccountContext.ModifyAccountConfigServices.FolderToDesk(_accountId, memberId, deskId);

            UnitOfWork.Commit();
        }

        public void FolderToOtherFolder(Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(_accountId).Validate(memberId).Validate(folderId);

            AccountContext.ModifyAccountConfigServices.FolderToOtherFolder(_accountId, memberId, folderId);

            UnitOfWork.Commit();
        }

        public void DeskToOtherDesk(Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(_accountId).Validate(memberId).Validate(deskId);

            AccountContext.ModifyAccountConfigServices.DeskToOtherDesk(_accountId, memberId, deskId);

            UnitOfWork.Commit();
        }

        public void ModifyFolderInfo(String memberName, String memberIcon, Int32 memberId)
        {
            ValidateParameter.Validate(memberName).Validate(memberIcon).Validate(memberId).Validate(_accountId);

            DeskContext.ModifyDeskMemberServices.ModifyFolderInfo(memberName, memberIcon, memberId, _accountId);

            UnitOfWork.Commit();
        }

        public void RemoveMember(Int32 memberId)
        {
            ValidateParameter.Validate(_accountId).Validate(memberId);

            DeskContext.ModifyDeskMemberServices.RemoveMember(_accountId, memberId);

            UnitOfWork.Commit();
        }

        public void ModifyMemberInfo(MemberDto member)
        {
            ValidateParameter.Validate(_accountId).Validate(member);

            DeskContext.ModifyDeskMemberServices.ModifyMemberInfo(_accountId, member.ConvertToModel<MemberDto, Member>());

            UnitOfWork.Commit();
        }

        public void CreateNewFolder(String folderName, String folderImg, Int32 deskId)
        {
            ValidateParameter.Validate(folderName).Validate(folderImg).Validate(deskId);

            DeskContext.CreateNewFolder.NewFolder(deskId,folderName,folderImg);

            UnitOfWork.Commit();
        }
    }
}
