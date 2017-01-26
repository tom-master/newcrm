using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    [Export(typeof(IModifyDeskMemberPostionServices))]
    internal sealed class ModifyDeskMemberPostionServices : IModifyDeskMemberPostionServices
    {

        [Import]
        public BaseServiceContext BaseContext { get; set; }

        public void MemberInDock(Int32 memberId)
        {
            var desks = GetDesks();

            foreach (var desk in desks)
            {
                var member = BaseContext.GetMember(memberId, desk);

                if (member != null)
                {
                    member.InDock();

                    BaseContext.Repository.Create<Desk>().Update(desk);

                    break;
                }
            }
        }

        private IEnumerable<Desk> GetDesks()
        {
            return BaseContext.Query.Find((Desk desk) => desk.AccountId == BaseContext.GetAccountId());
        }

        public void MemberOutDock(Int32 memberId, Int32 deskId)
        {
            var desks = GetDesks();

            var realDeskId = desks.FirstOrDefault(desk => desk.DeskNumber == deskId).Id;

            foreach (var desk in desks)
            {
                var member = BaseContext.GetMember(memberId, desk);

                if (member != null)
                {
                    member.OutDock().ToOtherDesk(realDeskId);

                    BaseContext.Repository.Create<Desk>().Update(desk);

                    break;
                }
            }
        }

        public void DockToFolder(Int32 memberId, Int32 folderId)
        {
            var desks = GetDesks();

            var deskResult = desks.FirstOrDefault(d => d.Members.Any(m => m.Id == memberId));

            BaseContext.GetMember(memberId, deskResult).OutDock().InFolder(folderId);

            BaseContext.Repository.Create<Desk>().Update(deskResult);
        }

        public void FolderToDock(Int32 memberId)
        {
            var desks = GetDesks();

            var deskResult = desks.FirstOrDefault(d => d.Members.Any(m => m.Id == memberId));

            BaseContext.GetMember(memberId, deskResult).InDock().OutFolder();

            BaseContext.Repository.Create<Desk>().Update(deskResult);
        }

        public void DeskToFolder(Int32 memberId, Int32 folderId)
        {
            var desks = GetDesks();

            foreach (var desk in desks)
            {
                var member = BaseContext.GetMember(memberId, desk);

                if (member != null)
                {
                    member.InFolder(folderId);

                    BaseContext.Repository.Create<Desk>().Update(desk);

                    break;
                }
            }
        }

        public void FolderToDesk(Int32 memberId, Int32 deskId)
        {
            var desks = GetDesks();

            var realDeskId = desks.FirstOrDefault(desk => desk.DeskNumber == deskId).Id;

            foreach (var desk in desks)
            {
                var member = BaseContext.GetMember(memberId, desk);

                if (member != null)
                {
                    if (member.DeskId == realDeskId)
                    {
                        member.OutFolder();
                    }
                    else
                    {
                        member.OutFolder().ToOtherDesk(realDeskId);
                    }

                    BaseContext.Repository.Create<Desk>().Update(desk);

                    break;
                }
            }
        }

        public void FolderToOtherFolder(Int32 memberId, Int32 folderId)
        {
            var deskResult = GetDesks().FirstOrDefault(d => d.Members.Any(m => m.Id == memberId));

            BaseContext.GetMember(memberId, deskResult).OutFolder().InFolder(folderId);

            BaseContext.Repository.Create<Desk>().Update(deskResult);

        }

        public void DeskToOtherDesk(Int32 memberId, Int32 deskId)
        {
            var desks = GetDesks();

            var realDeskId = desks.FirstOrDefault(desk => desk.DeskNumber == deskId).Id;

            foreach (var desk in desks)
            {
                var member = BaseContext.GetMember(memberId, desk);

                if (member != null)
                {

                    if (member.IsOnDock)
                    {
                        member.OutDock();
                    }

                    member.ToOtherDesk(realDeskId);

                    BaseContext.Repository.Create<Desk>().Update(desk);

                    break;
                }
            }
        }

        public void DockToOtherDesk(Int32 memberId, Int32 deskId)
        {
            var desks = GetDesks();

            var realDeskId = desks.FirstOrDefault(desk => desk.DeskNumber == deskId).Id;

            foreach (var desk in desks)
            {
                var member = BaseContext.GetMember(memberId, desk);

                if (member != null)
                {
                    member.OutDock().ToOtherDesk(realDeskId);

                    BaseContext.Repository.Create<Desk>().Update(desk);

                    break;
                }
            }
        }
    }
}
