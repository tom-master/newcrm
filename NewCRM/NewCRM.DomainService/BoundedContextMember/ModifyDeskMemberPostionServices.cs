using System;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Domain.Services.Service;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    [Export(typeof(IModifyDeskMemberPostionServices))]
    internal sealed class ModifyDeskMemberPostionServices : BaseService, IModifyDeskMemberPostionServices
    {

        public void MemberInDock(Int32 memberId)
        {
            var desks = Query.Find(FilterFactory.Create((Desk desk) => desk.AccountId == AccountId));

            foreach (var desk in desks)
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

        public void MemberOutDock(Int32 memberId, Int32 deskId)
        {
            var desks = Query.Find(FilterFactory.Create((Desk desk) => desk.AccountId == AccountId));

            var realDeskId = desks.FirstOrDefault(desk => desk.DeskNumber == deskId).Id;

            foreach (var desk in desks)
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

        public void DockToFolder(Int32 memberId, Int32 folderId)
        {
            var desks = Query.Find(FilterFactory.Create((Desk desk) => desk.AccountId == AccountId));

            var deskResult = desks.FirstOrDefault(d => d.Members.Any(m => m.Id == memberId));

            InternalDeskMember(memberId, deskResult).OutDock().InFolder(folderId);

            Repository.Create<Desk>().Update(deskResult);
        }

        public void FolderToDock(Int32 memberId)
        {
            var desks = Query.Find(FilterFactory.Create((Desk desk) => desk.AccountId == AccountId));

            var deskResult = desks.FirstOrDefault(d => d.Members.Any(m => m.Id == memberId));

            InternalDeskMember(memberId, deskResult).InDock().OutFolder();

            Repository.Create<Desk>().Update(deskResult);
        }

        public void DeskToFolder(Int32 memberId, Int32 folderId)
        {
            var desks = Query.Find(FilterFactory.Create((Desk desk) => desk.AccountId == AccountId));

            foreach (var desk in desks)
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

        public void FolderToDesk(Int32 memberId, Int32 deskId)
        {
            var desks = Query.Find(FilterFactory.Create((Desk desk) => desk.AccountId == AccountId));

            var realDeskId = desks.FirstOrDefault(desk => desk.DeskNumber == deskId).Id;

            foreach (var desk in desks)
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

        public void FolderToOtherFolder(Int32 memberId, Int32 folderId)
        {
            var desks = Query.Find(FilterFactory.Create((Desk desk) => desk.AccountId == AccountId));

            var deskResult = desks.FirstOrDefault(d => d.Members.Any(m => m.Id == memberId));

            InternalDeskMember(memberId, deskResult).OutFolder().InFolder(folderId);

            Repository.Create<Desk>().Update(deskResult);

        }

        public void DeskToOtherDesk(Int32 memberId, Int32 deskId)
        {
            var desks = Query.Find(FilterFactory.Create((Desk desk) => desk.AccountId == AccountId));

            var realDeskId = desks.FirstOrDefault(desk => desk.DeskNumber == deskId).Id;

            foreach (var desk in desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);

                if (memberResult != null)
                {
                    //memberResult.ToOtherDesk(realDeskId);

                    var nextDesk = Query.FindOne(FilterFactory.Create((Desk deskd) => deskd.AccountId == AccountId && deskd.Id == realDeskId));

                    nextDesk.AddMember(memberResult);

                    //desk.SetMemberMoveToDeskId(realDeskId);

                    Repository.Create<Desk>().Update(desk);

                    Repository.Create<Desk>().Update(nextDesk);

                    break;
                }
            }
        }
    }
}
