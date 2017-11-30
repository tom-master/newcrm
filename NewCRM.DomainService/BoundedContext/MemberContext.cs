using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.Repository.StorageProvider;

namespace NewCRM.Domain.Services.BoundedContext
{
    public class MemberContext : BaseServiceContext, IMemberContext
    {
        public List<Member> GetMembers(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);
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
                            a.DeskIndex
                            FROM dbo.Members AS a WHERE a.AccountId={accountId} AND a.IsDeleted=0";
                return dataStore.SqlGetDataTable(sql).AsList<Member>().ToList();
            }
        }

        public Member GetMember(Int32 accountId, Int32 memberId, Boolean isFolder)
        {
            ValidateParameter.Validate(accountId).Validate(memberId);
            using (var dataStore = new DataStore())
            {
                var where = new StringBuilder();
                if (isFolder)
                {
                    where.Append($@" AND a.MemberType={(Int32)MemberType.Folder}");
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
                    a.Width
                    FROM dbo.Members AS a WHERE a.AccountId={accountId} AND a.AppId={memberId} {where} AND a.IsDeleted=0";
                return dataStore.SqlGetDataTable(sql).AsSignal<Member>();
            }
        }

        public void ModifyFolderInfo(Int32 accountId, String memberName, String memberIcon, Int32 memberId)
        {
            ValidateParameter.Validate(accountId).Validate(memberName).Validate(memberIcon).Validate(memberId);
            using(var dataStore = new DataStore())
            {
                var sql = $@"UPDATE Members SET Name=@name,IconUrl=@url WHERE Id={memberId} AND AccountId={accountId} AND IsDeleted=0";
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@name",memberName),
                    new SqlParameter("@url",memberIcon)
                };
                dataStore.SqlExecute(sql);
            }
        }

        public void ModifyMemberIcon(Int32 accountId, Int32 memberId, String newIcon)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(newIcon);
            using(var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Members SET IconUrl=@url WHERE Id={memberId} AND AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql, new List<SqlParameter> { new SqlParameter("@url", newIcon) });
            }
        }

        public void ModifyMemberInfo(Int32 accountId, Member member)
        {
            ValidateParameter.Validate(accountId).Validate(member);
            using(var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Members SET IconUrl={member.IconUrl},Name={member.Name},Width={member.Width},Height={member.Height},IsResize={member.IsResize},IsOpenMax={member.IsOpenMax},IsFlash={member.IsFlash} WHERE Id={member.Id} AND AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void RemoveMember(Int32 accountId, Int32 memberId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId);
            using(var dataStore = new DataStore())
            {
                dataStore.OpenTransaction();
                try
                {
                    var isFolder = false;
                    #region 判断是否为文件夹
                    {
                        var sql = $@"SELECT a.MemberType FROM dbo.Members AS a WHERE a.Id={memberId} AND a.AccountId={accountId} AND a.IsDeleted=0";
                        isFolder = ((Int32)dataStore.SqlScalar(sql)) == (Int32)MemberType.Folder;
                    }
                    #endregion

                    if(isFolder)
                    {
                        #region 将文件夹内的成员移出
                        {
                            var sql = $@"UPDATE dbo.Members SET FolderId=0 WHERE AccountId={accountId} AND IsDeleted=0 AND FolderId={memberId}";
                            dataStore.SqlExecute(sql);
                        }
                        #endregion
                    }
                    else
                    {
                        var appId = 0;

                        #region 获取appId
                        {
                            var sql = $@"SELECT a.AppId FROM dbo.Members AS a WHERE a.Id={memberId} AND a.AccountId={accountId} AND a.IsDeleted=0";
                            appId = (Int32)dataStore.SqlScalar(sql);
                        }
                        #endregion

                        #region app使用量-1
                        {
                            var sql = $@"UPDATE dbo.Apps SET UseCount=UseCount-1 WHERE Id={appId} AND AccountId={accountId} AND IsDeleted=0";
                        }
                        #endregion
                    }

                    #region 移除成员
                    {
                        var sql = $@"UPDATE dbo.Members SET IsDeleted=1 WHERE Id={memberId} AND AccountId={accountId}";
                        dataStore.SqlExecute(sql);
                    }
                    #endregion

                    dataStore.Commit();
                }
                catch(Exception)
                {
                    dataStore.Rollback();
                    throw;
                }
            }
        }
    }
}
