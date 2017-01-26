using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Domain.ValueObject;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    [Export(typeof(IModifyDeskMemberServices))]
    internal sealed class ModifyDeskMemberServices : IModifyDeskMemberServices
    {

        [Import]
        public BaseServiceContext BaseContext { get; set; }

        public void ModifyFolderInfo(String memberName, String memberIcon, Int32 memberId)
        {

            foreach (var desk in GetDesks())
            {
                var memberResult = BaseContext.GetMember(memberId, desk);

                if (memberResult != null)
                {
                    memberResult.ModifyName(memberName).ModifyIcon(memberIcon);

                    BaseContext.Repository.Create<Desk>().Update(desk);

                    break;
                }
            }

        }

        public void ModifyMemberInfo(Member member)
        {
            foreach (var desk in GetDesks())
            {
                var memberResult = BaseContext.GetMember(member.Id, desk);

                if (memberResult != null)
                {
                    memberResult.ModifyIcon(member.IconUrl)
                    .ModifyName(member.Name)
                    .ModifyWidth(member.Width)
                    .ModifyHeight(member.Height)
                    .ModifyIsResize(member.IsResize)
                    .ModifyIsOpenMax(member.IsOpenMax)
                    .ModifyIsFlash(member.IsFlash);

                    BaseContext.Repository.Create<Desk>().Update(desk);

                    break;
                }
            }

        }

        public void RemoveMember(Int32 memberId)
        {
            var desks =GetDesks();

            App appResult = null;

            foreach (var desk in desks)
            {
                var memberResult = BaseContext.GetMember(memberId, desk);

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
                        appResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create<App>(app => app.Id == memberResult.AppId));

                        appResult.SubtractUseCount();

                        appResult.SubtractStar(BaseContext.GetAccountId());

                    }

                    memberResult.Remove();

                    BaseContext.Repository.Create<Desk>().Update(desk);

                    if (appResult != null)
                    {
                        BaseContext.Repository.Create<App>().Update(appResult);
                    }
                    break;
                }
            }

        }

        private IEnumerable<Desk> GetDesks()
        {
            return BaseContext.Query.Find((Desk desk) => desk.AccountId == BaseContext.GetAccountId());
        }
    }
}
