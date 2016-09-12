using System;
using System.ComponentModel.Composition;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Interface;

namespace NewCRM.Domain.Services
{
    [Export(typeof(IModifyDeskMemberInfoServices))]
    internal sealed class ModifyDeskMemberInfoServices : BaseService.BaseService, IModifyDeskMemberInfoServices
    {
        public void ModifyFolderInfo(String memberName, String memberIcon, Int32 memberId, Int32 accountId)
        {
            var accountConfig = GetAccountInfoService(accountId).Config;

            foreach (var desk in accountConfig.Desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);

                if (memberResult != null)
                {
                    memberResult.ModifyName(memberName).ModifyIcon(memberIcon);

                    Repository.Create<Desk>().Update(desk);

                    break;
                }
            }

        }

        public void ModifyMemberInfo(Int32 accountId, Member member)
        {
            var accountResult = GetAccountInfoService(accountId);

            foreach (var desk in accountResult.Config.Desks)
            {
                var memberResult = InternalDeskMember(member.Id, desk);

                if (memberResult != null)
                {
                    memberResult.ModifyIcon(member.IconUrl)
                    .ModifyName(member.Name)
                    .ModifyWidth(member.Width)
                    .ModifyHeight(member.Height)
                    .ModifyIsResize(member.IsResize)
                    .ModifyIsOpenMax(member.IsOpenMax)
                    .ModifyIsFlash(member.IsFlash);

                    Repository.Create<Desk>().Update(desk);

                    break;
                }
            }

        }
    }
}
