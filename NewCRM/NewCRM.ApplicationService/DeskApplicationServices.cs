using System;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Application.Interface;
using NewCRM.Domain;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Domain.ValueObject;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Application.Services
{
    [Export(typeof(IDeskApplicationServices))]
    internal class DeskApplicationServices : IDeskApplicationServices
    {
        [Import]
        public BaseServiceContext BaseContext { get; set; }

        private readonly IModifyDeskMemberServices _modifyDeskMemberServices;

        private readonly IModifyDockPostionServices _modifyDockPostionServices;

        private readonly ICreateNewFolderServices _createNewFolderServices;

        private readonly IModifyDeskMemberPostionServices _modifyDeskMemberPostionServices;

        [ImportingConstructor]
        public DeskApplicationServices(IModifyDeskMemberServices modifyDeskMemberServices,
            IModifyDockPostionServices modifyDockPostionServices,
            ICreateNewFolderServices createNewFolderServices,
            IModifyDeskMemberPostionServices modifyDeskMemberPostionServices)
        {
            _modifyDeskMemberServices = modifyDeskMemberServices;

            _modifyDockPostionServices = modifyDockPostionServices;

            _createNewFolderServices = createNewFolderServices;

            _modifyDeskMemberPostionServices = modifyDeskMemberPostionServices;
        }

        public MemberDto GetMember(Int32 memberId, Boolean isFolder = default(Boolean))
        {
            BaseContext.ValidateParameter.Validate(memberId);

            var desks = BaseContext.Query.Find((Desk desk) => desk.AccountId == BaseContext.GetAccountId());

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
            BaseContext.ValidateParameter.Validate(newDefaultDeskNumber);

            var accountResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create((Account account) => account.Id == BaseContext.GetAccountId()));

            accountResult.Config.ModifyDefaultDesk(newDefaultDeskNumber);

            BaseContext.Repository.Create<Account>().Update(accountResult);

            BaseContext.UnitOfWork.Commit();
        }

        public void ModifyDockPosition(Int32 defaultDeskNumber, String newPosition)
        {
            BaseContext.ValidateParameter.Validate(defaultDeskNumber).Validate(newPosition);

            _modifyDockPostionServices.ModifyDockPosition(defaultDeskNumber, newPosition);

            BaseContext.UnitOfWork.Commit();
        }

        public void MemberInDock(Int32 memberId)
        {
            BaseContext.ValidateParameter.Validate(memberId);

            _modifyDeskMemberPostionServices.MemberInDock(memberId);

            BaseContext.UnitOfWork.Commit();
        }

        public void MemberOutDock(Int32 memberId, Int32 deskId)
        {
            BaseContext.ValidateParameter.Validate(memberId);

            _modifyDeskMemberPostionServices.MemberOutDock(memberId, deskId);

            BaseContext.UnitOfWork.Commit();
        }

        public void DockToFolder(Int32 memberId, Int32 folderId)
        {
            BaseContext.ValidateParameter.Validate(memberId).Validate(folderId);

            _modifyDeskMemberPostionServices.DockToFolder(memberId, folderId);

            BaseContext.UnitOfWork.Commit();
        }

        public void FolderToDock(Int32 memberId)
        {
            BaseContext.ValidateParameter.Validate(memberId);

            _modifyDeskMemberPostionServices.FolderToDock(memberId);

            BaseContext.UnitOfWork.Commit();
        }

        public void DeskToFolder(Int32 memberId, Int32 folderId)
        {
            BaseContext.ValidateParameter.Validate(memberId).Validate(folderId);

            _modifyDeskMemberPostionServices.DeskToFolder(memberId, folderId);

            BaseContext.UnitOfWork.Commit();
        }

        public void FolderToDesk(Int32 memberId, Int32 deskId)
        {
            BaseContext.ValidateParameter.Validate(memberId).Validate(deskId);

            _modifyDeskMemberPostionServices.FolderToDesk(memberId, deskId);

            BaseContext.UnitOfWork.Commit();
        }

        public void FolderToOtherFolder(Int32 memberId, Int32 folderId)
        {
            BaseContext.ValidateParameter.Validate(memberId).Validate(folderId);

            _modifyDeskMemberPostionServices.FolderToOtherFolder(memberId, folderId);

            BaseContext.UnitOfWork.Commit();
        }

        public void DeskToOtherDesk(Int32 memberId, Int32 deskId)
        {
            BaseContext.ValidateParameter.Validate(memberId).Validate(deskId);

            _modifyDeskMemberPostionServices.DeskToOtherDesk(memberId, deskId);

            BaseContext.UnitOfWork.Commit();
        }

        public void ModifyFolderInfo(String memberName, String memberIcon, Int32 memberId)
        {
            BaseContext.ValidateParameter.Validate(memberName).Validate(memberIcon).Validate(memberId);

            _modifyDeskMemberServices.ModifyFolderInfo(memberName, memberIcon, memberId);

            BaseContext.UnitOfWork.Commit();
        }

        public void RemoveMember(Int32 memberId)
        {
            BaseContext.ValidateParameter.Validate(memberId);

            _modifyDeskMemberServices.RemoveMember(memberId);

            BaseContext.UnitOfWork.Commit();
        }

        public void ModifyMemberInfo(MemberDto member)
        {
            BaseContext.ValidateParameter.Validate(member);

            _modifyDeskMemberServices.ModifyMemberInfo(member.ConvertToModel<MemberDto, Member>());

            BaseContext.UnitOfWork.Commit();
        }

        public void CreateNewFolder(String folderName, String folderImg, Int32 deskId)
        {
            BaseContext.ValidateParameter.Validate(folderName).Validate(folderImg).Validate(deskId);

            _createNewFolderServices.NewFolder(deskId, folderName, folderImg);

            BaseContext.UnitOfWork.Commit();
        }

        public void DockToOtherDesk(Int32 memberId, Int32 deskId)
        {
            BaseContext.ValidateParameter.Validate(memberId).Validate(deskId);

            _modifyDeskMemberPostionServices.DockToOtherDesk(memberId, deskId);

            BaseContext.UnitOfWork.Commit();
        }
    }
}
