using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.Repository.StorageProvider;

namespace NewCRM.Domain.Services.BoundedContextMember
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
                            a.IsSetbar
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
                    FROM dbo.Members AS a WHERE a.AccountId={accountId} AND a.Id={memberId} {where} AND a.IsDeleted=0";
                return dataStore.SqlGetDataTable(sql).AsSignal<Member>();
            }
        }
    }
}
