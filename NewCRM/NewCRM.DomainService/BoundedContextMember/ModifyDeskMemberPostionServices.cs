using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;
using IModifyDeskMemberPostionServices = NewCRM.Domain.Services.Interface.IModifyDeskMemberPostionServices;

namespace NewCRM.Domain.Services.BoundedContextMember
{
	internal sealed class ModifyDeskMemberPostionServices : BaseServiceContext, IModifyDeskMemberPostionServices
	{
		public void MemberInDock(Int32 accountId, Int32 memberId)
		{
			var desks = GetDesks(accountId);

			foreach (var desk in desks)
			{
				var member = GetMember(memberId, desk);

				if (member != null)
				{
					member.InDock();

					Repository.Create<Desk>().Update(desk);

					break;
				}
			}
		}

		private IEnumerable<Desk> GetDesks(Int32 accountId)
		{
			return DatabaseQuery.Find(FilterFactory.Create((Desk desk) => desk.AccountId == accountId));
		}

		public void MemberOutDock(Int32 accountId, Int32 memberId, Int32 deskId)
		{
			var desks = GetDesks(accountId);

			var realDeskId = desks.FirstOrDefault(desk => desk.DeskNumber == deskId).Id;

			foreach (var desk in desks)
			{
				var member = GetMember(memberId, desk);

				if (member != null)
				{
					member.OutDock().ToOtherDesk(realDeskId);

					Repository.Create<Desk>().Update(desk);

					break;
				}
			}
		}

		public void DockToFolder(Int32 accountId, Int32 memberId, Int32 folderId)
		{
			var desks = GetDesks(accountId);

			var deskResult = desks.FirstOrDefault(d => d.Members.Any(m => m.Id == memberId));

			GetMember(memberId, deskResult).OutDock().InFolder(folderId);

			Repository.Create<Desk>().Update(deskResult);
		}

		public void FolderToDock(Int32 accountId, Int32 memberId)
		{
			var desks = GetDesks(accountId);

			var deskResult = desks.FirstOrDefault(d => d.Members.Any(m => m.Id == memberId));

			GetMember(memberId, deskResult).InDock().OutFolder();

			Repository.Create<Desk>().Update(deskResult);
		}

		public void DeskToFolder(Int32 accountId, Int32 memberId, Int32 folderId)
		{
			var desks = GetDesks(accountId);

			foreach (var desk in desks)
			{
				var member = GetMember(memberId, desk);

				if (member != null)
				{
					member.InFolder(folderId);

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
				var member = GetMember(memberId, desk);

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

					Repository.Create<Desk>().Update(desk);

					break;
				}
			}
		}

		public void FolderToOtherFolder(Int32 accountId, Int32 memberId, Int32 folderId)
		{
			var deskResult = GetDesks(accountId).FirstOrDefault(d => d.Members.Any(m => m.Id == memberId));

			GetMember(memberId, deskResult).OutFolder().InFolder(folderId);

			Repository.Create<Desk>().Update(deskResult);

		}

		public void DeskToOtherDesk(Int32 accountId, Int32 memberId, Int32 deskId)
		{
			var desks = GetDesks(accountId);

			var realDeskId = desks.FirstOrDefault(desk => desk.DeskNumber == deskId).Id;

			foreach (var desk in desks)
			{
				var member = GetMember(memberId, desk);

				if (member != null)
				{

					if (member.IsOnDock)
					{
						member.OutDock();
					}

					member.ToOtherDesk(realDeskId);

					Repository.Create<Desk>().Update(desk);

					break;
				}
			}
		}

		public void DockToOtherDesk(Int32 accountId, Int32 memberId, Int32 deskId)
		{
			var desks = GetDesks(accountId);

			var realDeskId = desks.FirstOrDefault(desk => desk.DeskNumber == deskId).Id;

			foreach (var desk in desks)
			{
				var member = GetMember(memberId, desk);

				if (member != null)
				{
					member.OutDock().ToOtherDesk(realDeskId);

					Repository.Create<Desk>().Update(desk);

					break;
				}
			}
		}
	}
}
