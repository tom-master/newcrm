using System;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Domain.Services.Service;
using NewCRM.Domain.ValueObject;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    [Export(typeof(IModifyDeskMemberServices))]
    internal sealed class ModifyDeskMemberServices : BaseService, IModifyDeskMemberServices
    {
        public void ModifyFolderInfo(String memberName, String memberIcon, Int32 memberId, Int32 accountId)
        {
            var desks = GetDesks(accountId);

            foreach (var desk in desks)
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
            var desks = GetDesks(accountId);

            foreach (var desk in desks)
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

        public void RemoveMember(Int32 accountId, Int32 memberId)
        {
            var desks = GetDesks(accountId);

            App appResult = null;

            foreach (var desk in desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);

                if (memberResult != null)
                {
                    if (memberResult.MemberType == MemberType.Folder)
                    {
                        //移除文件夹中的内容
                        foreach (var desk1 in desks)
                        {
                            desk1.Members.Where(d => d.FolderId == memberId).ToList().ForEach(m => m.Remove());
                        }
                    }
                    else
                    {
                        appResult = Query.FindOne(FilterFactory.Create<App>(app => app.Id == memberResult.AppId));

                        appResult.SubtractUseCount();

                        appResult.SubtractStar(accountId);

                    }

                    memberResult.Remove();

                    Repository.Create<Desk>().Update(desk);

                    if (appResult != null)
                    {
                        Repository.Create<App>().Update(appResult);
                    }
                    break;
                }
            }

        }

    }
}
