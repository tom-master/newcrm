using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Domain.ValueObject;

namespace NewCRM.Domain.Services.BoundedContextMember
{

	internal sealed class ModifyDeskMemberServices : BaseServiceContext, IModifyDeskMemberServices
	{


		public void ModifyFolderInfo(Int32 accountId, String memberName, String memberIcon, Int32 memberId)
		{

			foreach (var desk in GetDesks(accountId))
			{
				var memberResult = GetMember(memberId, desk);

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
			foreach (var desk in GetDesks(accountId))
			{
				var memberResult = GetMember(member.Id, desk);

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
				var memberResult = GetMember(memberId, desk);

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
						appResult = DatabaseQuery.FindOne(FilterFactory.Create<App>(app => app.Id == memberResult.AppId));

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

		private IEnumerable<Desk> GetDesks(Int32 accountId)
		{
			return DatabaseQuery.Find(FilterFactory.Create((Desk desk) => desk.AccountId == accountId));
		}
	}
}
