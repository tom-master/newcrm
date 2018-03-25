using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewLib.Data.Mapper.InternalDataStore;

namespace NewCRM.Domain.Services.BoundedContext
{
	public class MemberContext: BaseServiceContext, IMemberContext
	{
		public async Task<List<Member>> GetMembersAsync(Int32 accountId)
		{
			Parameter.Validate(accountId);
			return await Task.Run<List<Member>>(() =>
			{
				using (var dataStore = new DataStore())
				{
					var sql = $@"SELECT 
                            a.MemberType,
                            a.Id,
                            a.AppId,
                            a.Name,
                            a.IconUrl,
                            a.Width,
                            a.Height,
                            a.IsOnDock,
                            a.IsDraw,
                            a.IsOpenMax,
                            a.IsSetbar,
                            a.DeskIndex,
                            a.FolderId,
                            a.IsIconByUpload
                            FROM dbo.Members AS a WHERE a.AccountId=@AccountId AND a.IsDeleted=0";
					var parameters = new List<SqlParameter>
					{
						new SqlParameter("@AccountId",accountId)
					};
					return dataStore.Find<Member>(sql, parameters);
				}
			});
		}

		public async Task<Member> GetMemberAsync(Int32 accountId, Int32 memberId, Boolean isFolder)
		{
			Parameter.Validate(accountId).Validate(memberId);
			return await Task.Run(() =>
			{
				using (var dataStore = new DataStore())
				{
					var where = new StringBuilder();
					var parameters = new List<SqlParameter>();
					if (isFolder)
					{
						parameters.Add(new SqlParameter("@Id", memberId));
						parameters.Add(new SqlParameter("@MemberType", (Int32)MemberType.Folder));
						where.Append($@" AND a.Id=@Id AND a.MemberType=@MemberType");
					}
					else
					{
						parameters.Add(new SqlParameter("@Id", memberId));
						where.Append($@" AND a.AppId=@Id");
					}

					var sql = $@"SELECT 
                    a.MemberType,
                    a.AppId,
                    a.AppUrl,
                    a.DeskIndex,
                    a.FolderId,
                    a.Height,
                    a.IconUrl,
                    a.Id,
                    a.IsDraw,
                    a.IsFlash,
                    a.IsFull,
                    a.IsLock,
                    a.IsMax,
                    a.IsOnDock,
                    a.IsOpenMax,
                    a.IsResize,
                    a.IsSetbar,
                    a.Name,
                    a.Width,
                    a.AccountId,
                    a.IsIconByUpload
                    FROM dbo.Members AS a WHERE a.AccountId=@AccountId {where} AND a.IsDeleted=0";
					parameters.Add(new SqlParameter("@AccountId", accountId));
					return dataStore.FindOne<Member>(sql, parameters);
				}
			});
		}

		public async Task<Boolean> CheckMemberNameAsync(String name)
		{
			Parameter.Validate(name);
			return await Task.Run(() =>
			{
				using (var dataStore = new DataStore())
				{
					var sql = $@"SELECT COUNT(*) FROM dbo.Members AS a WHERE a.Name=@name AND a.IsDeleted=0";
					var parameters = new List<SqlParameter>
					{
						new SqlParameter("@name",name)
					};

					return dataStore.FindSingleValue<Int32>(sql, parameters) > 0;
				}
			});
		}

		public async Task ModifyFolderInfoAsync(Int32 accountId, String memberName, String memberIcon, Int32 memberId)
		{
			Parameter.Validate(accountId).Validate(memberName).Validate(memberIcon).Validate(memberId);
			await Task.Run(() =>
			{
				using (var dataStore = new DataStore())
				{
					#region sql
					{
						var sql = $@"UPDATE Members SET Name=@name,IconUrl=@url WHERE Id=@Id AND AccountId=@AccountId AND IsDeleted=0";
						var parameters = new List<SqlParameter>
				  {
						new SqlParameter("@Id",memberId),
						new SqlParameter("@AccountId",accountId),
						new SqlParameter("@name",memberName),
						new SqlParameter("@url",memberIcon)
				  };
						dataStore.SqlExecute(sql, parameters);
					}
					#endregion
				}
			});
		}

		public async Task ModifyMemberIconAsync(Int32 accountId, Int32 memberId, String newIcon)
		{
			Parameter.Validate(accountId).Validate(memberId).Validate(newIcon);
			await Task.Run(() =>
			{
				using (var dataStore = new DataStore())
				{
					var sql = $@"UPDATE dbo.Members SET IconUrl=@url WHERE Id=@Id AND AccountId=@AccountId AND IsDeleted=0";
					var parameters = new List<SqlParameter>
				   {
						new SqlParameter("@Id",memberId),
						new SqlParameter("@AccountId",accountId),
						new SqlParameter("@url",newIcon)
				   };
					dataStore.SqlExecute(sql, parameters);
				}
			});
		}

		public async Task ModifyMemberInfoAsync(Int32 accountId, Member member)
		{
			Parameter.Validate(accountId).Validate(member);
			await Task.Run(() =>
			{
				using (var dataStore = new DataStore())
				{
					var sql = $@"UPDATE dbo.Members SET IsIconByUpload=@IsIconByUpload,IconUrl=@IconUrl,Name=@Name,Width=@Width,Height=@Height,IsResize=@IsResize,IsOpenMax=@IsOpenMax,IsFlash=@IsFlash WHERE Id=@Id AND AccountId=@AccountId AND IsDeleted=0";
					var parameters = new List<SqlParameter>
				   {
						new SqlParameter("@IsIconByUpload",(member.IsIconByUpload ? 1 : 0)),
						new SqlParameter("@IconUrl",member.IconUrl),
						new SqlParameter("@Name",member.Name),
						new SqlParameter("@Width",member.Width),
						new SqlParameter("@Height",member.Height),
						new SqlParameter("@IsResize",member.IsResize.ParseToInt32()),
						new SqlParameter("@IsOpenMax",member.IsOpenMax.ParseToInt32()),
						new SqlParameter("@IsFlash",member.IsFlash.ParseToInt32()),
						new SqlParameter("@Id",member.Id),
						new SqlParameter("@AccountId",accountId)
				   };
					dataStore.SqlExecute(sql, parameters);
				}
			});
		}

		public async Task UninstallMemberAsync(Int32 accountId, Int32 memberId)
		{
			Parameter.Validate(accountId).Validate(memberId);
			await Task.Run(() =>
			{
				using (var dataStore = new DataStore())
				{
					dataStore.OpenTransaction();
					try
					{
						var isFolder = false;

						#region 判断是否为文件夹
						{
							var sql = $@"SELECT a.MemberType FROM dbo.Members AS a WHERE a.Id=@Id AND a.AccountId=@AccountId AND a.IsDeleted=0";
							var parameters = new List<SqlParameter>
					   {
							new SqlParameter("@Id", memberId),
							new SqlParameter("@AccountId", accountId)
					   };
							isFolder = (dataStore.FindSingleValue<Int32>(sql, parameters)) == (Int32)MemberType.Folder;
						}
						#endregion

						if (isFolder)
						{
							#region 将文件夹内的成员移出
							{
								var sql = $@"UPDATE dbo.Members SET FolderId=0 WHERE AccountId=@AccountId AND IsDeleted=0 AND FolderId=@FolderId";
								var parameters = new List<SqlParameter>
						   {
								new SqlParameter("@FolderId", memberId),
								new SqlParameter("@AccountId", accountId)
						   };
								dataStore.SqlExecute(sql, parameters);
							}
							#endregion
						}
						else
						{
							var appId = 0;

							#region 获取appId
							{
								var sql = $@"SELECT a.AppId FROM dbo.Members AS a WHERE a.Id=@Id AND a.AccountId=@AccountId AND a.IsDeleted=0";
								var parameters = new List<SqlParameter>
						   {
								new SqlParameter("@Id", memberId),
								new SqlParameter("@AccountId", accountId)
						   };
								appId = dataStore.FindSingleValue<Int32>(sql, parameters);
							}
							#endregion

							#region app使用量-1
							{
								var sql = $@"UPDATE dbo.App SET UseCount=UseCount-1 WHERE Id=@Id AND AccountId=@AccountId AND IsDeleted=0";
								var parameters = new List<SqlParameter>
						   {
								new SqlParameter("@Id", appId),
								new SqlParameter("@AccountId", accountId)
						   };
								dataStore.SqlExecute(sql, parameters);
							}
							#endregion
						}

						#region 移除成员
						{
							var sql = $@"UPDATE dbo.Members SET IsDeleted=1 WHERE Id=@Id AND AccountId=@AccountId";
							var parameters = new List<SqlParameter>
					   {
							new SqlParameter("@Id", memberId),
							new SqlParameter("@AccountId", accountId)
					   };
							dataStore.SqlExecute(sql, parameters);
						}
						#endregion

						dataStore.Commit();
					}
					catch (Exception)
					{
						dataStore.Rollback();
						throw;
					}
				}
			});
		}
	}
}
