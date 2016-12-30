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

        public void MemberInDock(Int32 accountId, Int32 memberId)
        {
            var desks = GetDesks(accountId);

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

        public void MemberOutDock(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            var desks = GetDesks(accountId);

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

        public void DockToFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            var desks = GetDesks(accountId);

            var desk = desks.FirstOrDefault(d => d.Members.Any(m => m.Id == memberId));

            InternalDeskMember(memberId, desk).OutDock().InFolder(folderId);

            Repository.Create<Desk>().Update(desk);
        }

        public void FolderToDock(Int32 accountId, Int32 memberId)
        {
            var desks = GetDesks(accountId);

            var desk = desks.FirstOrDefault(d => d.Members.Any(m => m.Id == memberId));

            InternalDeskMember(memberId, desk).InDock().OutFolder();

            Repository.Create<Desk>().Update(desk);
        }

        public void DeskToFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            var desks = GetDesks(accountId);

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

        public void FolderToDesk(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            var desks = GetDesks(accountId);

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

        public void FolderToOtherFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            var desks = GetDesks(accountId);

            var desk = desks.FirstOrDefault(d => d.Members.Any(m => m.Id == memberId));

            InternalDeskMember(memberId, desk).OutFolder().InFolder(folderId);

            Repository.Create<Desk>().Update(desk);

        }

        public void DeskToOtherDesk(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            var desks = GetDesks(accountId);

            var realDeskId = desks.FirstOrDefault(desk => desk.DeskNumber == deskId).Id;


            foreach (var desk in desks)
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
