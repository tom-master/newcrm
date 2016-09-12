using System;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Domain.Interface;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services
{
    [Export(typeof(IModifyDeskMemberPostionServices))]
    internal sealed class ModifyDeskMemberPostionServices : BaseService.BaseService, IModifyDeskMemberPostionServices
    {

        public void MemberInDock(Int32 accountId, Int32 memberId)
        {
            var accountConfig = GetAccountInfoService(accountId).Config;

            foreach (var desk in accountConfig.Desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);

                if (memberResult != null)
                {
                    memberResult.InDock();

                    Repository.Create<Desk>().Update(desk);

                    break;
                }
            }


        }

        public void MemberOutDock(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            var accountConfig = GetAccountInfoService(accountId).Config;

            var realDeskId = GetRealDeskIdService(deskId, accountConfig);

            foreach (var desk in accountConfig.Desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);

                if (memberResult != null)
                {
                    memberResult.OutDock().ToOtherDesk(realDeskId);

                    Repository.Create<Desk>().Update(desk);

                    break;
                }
            }


        }

        public void DockToFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            var configResult = GetAccountInfoService(accountId).Config;

            var desk = configResult.Desks.FirstOrDefault(c => c.DeskNumber == configResult.DefaultDeskNumber);

            InternalDeskMember(memberId, desk).OutDock().InFolder(folderId);

            Repository.Create<Desk>().Update(desk);


        }

        public void FolderToDock(Int32 accountId, Int32 memberId)
        {
            var configResult = GetAccountInfoService(accountId).Config;

            var desk = configResult.Desks.FirstOrDefault(c => c.DeskNumber == configResult.DefaultDeskNumber);

            InternalDeskMember(memberId, desk).InDock().OutFolder();

            Repository.Create<Desk>().Update(desk);


        }

        public void DeskToFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            var accountConfig = GetAccountInfoService(accountId).Config;

            foreach (var desk in accountConfig.Desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);

                if (memberResult != null)
                {
                    memberResult.InFolder(folderId);

                    Repository.Create<Desk>().Update(desk);

                    break;
                }
            }


        }

        public void FolderToDesk(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            var accountConfig = GetAccountInfoService(accountId).Config;

            var realDeskId = GetRealDeskIdService(deskId, accountConfig);

            foreach (var desk in accountConfig.Desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);

                if (memberResult != null)
                {
                    if (memberResult.DeskId == realDeskId)
                    {
                        memberResult.OutFolder();
                    }
                    else
                    {
                        memberResult.OutFolder().ToOtherDesk(realDeskId);
                    }

                    Repository.Create<Desk>().Update(desk);

                    break;
                }
            }


        }

        public void FolderToOtherFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            var configResult = GetAccountInfoService(accountId).Config;

            var desk = configResult.Desks.FirstOrDefault(c => c.DeskNumber == configResult.DefaultDeskNumber);

            InternalDeskMember(memberId, desk).OutFolder().InFolder(folderId);

            Repository.Create<Desk>().Update(desk);

        }

        public void DeskToOtherDesk(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            var accountConfig = GetAccountInfoService(accountId).Config;

            var realDeskId = GetRealDeskIdService(deskId, accountConfig);

            foreach (var desk in accountConfig.Desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);

                if (memberResult != null)
                {
                    memberResult.ToOtherDesk(realDeskId);

                    Repository.Create<Desk>().Update(desk);

                    break;
                }
            }

        }
    }
}
