using System;
using System.Linq;
using NewCRM.Application.Interface;
using NewCRM.Application.Services.Services;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContext.Agent;
using NewCRM.Domain.Interface.BoundedContext.Desk;
using NewCRM.Domain.ValueObject;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Application.Services
{
    internal class DeskApplicationServices : BaseService, IDeskApplicationServices
    {

        private readonly IDeskContext _deskContext;

        private readonly IAccountContext _accountContext;


        public DeskApplicationServices(IDeskContext deskContext, IAccountContext accountContext)
        {
            _deskContext = deskContext;

            _accountContext = accountContext;
        }


        public MemberDto GetMember(Int32 memberId, Boolean isFolder = default(Boolean))
        {
            ValidateParameter.Validate(memberId);

            var desks = GetDesks();

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


        public void ModifyDefaultDeskNumber(Int32 newDefaultDeskNumber)
        {
            ValidateParameter.Validate(newDefaultDeskNumber);

            var accountResult = Query.FindOne(FilterFactory.Create((Account account) => account.Id == AccountId));

            accountResult.Config.ModifyDefaultDesk(newDefaultDeskNumber);

            Repository.Create<Account>().Update(accountResult);

            UnitOfWork.Commit();
        }

        public void ModifyDockPosition(Int32 defaultDeskNumber, String newPosition)
        {
            ValidateParameter.Validate(defaultDeskNumber).Validate(newPosition);

            _deskContext.ModifyDockPostionServices.ModifyDockPosition(defaultDeskNumber, newPosition);

            UnitOfWork.Commit();
        }

        public void MemberInDock(Int32 memberId)
        {
            ValidateParameter.Validate(memberId);

            _accountContext.ModifyAccountConfigServices.MemberInDock(memberId);

            UnitOfWork.Commit();
        }

        public void MemberOutDock(Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(memberId);

            _accountContext.ModifyAccountConfigServices.MemberOutDock(memberId, deskId);

            UnitOfWork.Commit();
        }

        public void DockToFolder(Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(memberId).Validate(folderId);

            _accountContext.ModifyAccountConfigServices.DockToFolder(memberId, folderId);

            UnitOfWork.Commit();
        }

        public void FolderToDock(Int32 memberId)
        {
            ValidateParameter.Validate(memberId);

            _accountContext.ModifyAccountConfigServices.FolderToDock(memberId);

            UnitOfWork.Commit();
        }

        public void DeskToFolder(Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(memberId).Validate(folderId);

            _accountContext.ModifyAccountConfigServices.DeskToFolder(memberId, folderId);

            UnitOfWork.Commit();
        }

        public void FolderToDesk(Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(memberId).Validate(deskId);

            _accountContext.ModifyAccountConfigServices.FolderToDesk(memberId, deskId);

            UnitOfWork.Commit();
        }

        public void FolderToOtherFolder(Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(memberId).Validate(folderId);

            _accountContext.ModifyAccountConfigServices.FolderToOtherFolder(memberId, folderId);

            UnitOfWork.Commit();
        }

        public void DeskToOtherDesk(Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(memberId).Validate(deskId);

            _accountContext.ModifyAccountConfigServices.DeskToOtherDesk(memberId, deskId);

            UnitOfWork.Commit();
        }

        public void ModifyFolderInfo(String memberName, String memberIcon, Int32 memberId)
        {
            ValidateParameter.Validate(memberName).Validate(memberIcon).Validate(memberId);

            _deskContext.ModifyDeskMemberServices.ModifyFolderInfo(memberName, memberIcon, memberId);

            UnitOfWork.Commit();
        }

        public void RemoveMember(Int32 memberId)
        {
            ValidateParameter.Validate(memberId);

            _deskContext.ModifyDeskMemberServices.RemoveMember(memberId);

            UnitOfWork.Commit();
        }

        public void ModifyMemberInfo(MemberDto member)
        {
            ValidateParameter.Validate(member);

            _deskContext.ModifyDeskMemberServices.ModifyMemberInfo(member.ConvertToModel<MemberDto, Member>());

            UnitOfWork.Commit();
        }

        public void CreateNewFolder(String folderName, String folderImg, Int32 deskId)
        {
            ValidateParameter.Validate(folderName).Validate(folderImg).Validate(deskId);

            _deskContext.CreateNewFolder.NewFolder(deskId, folderName, folderImg);

            UnitOfWork.Commit();
        }

        public void DockToOtherDesk(Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(memberId).Validate(deskId);

            _accountContext.ModifyAccountConfigServices.DockToOtherDesk(memberId, deskId);

            UnitOfWork.Commit();
        }
    }
}
