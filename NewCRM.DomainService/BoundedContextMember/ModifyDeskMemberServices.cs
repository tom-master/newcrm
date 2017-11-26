using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Repository.StorageProvider;

namespace NewCRM.Domain.Services.BoundedContextMember
{

    public sealed class ModifyDeskMemberServices : BaseServiceContext, IModifyDeskMemberServices
    {
        public void ModifyFolderInfo(Int32 accountId, String memberName, String memberIcon, Int32 memberId)
        {
            ValidateParameter.Validate(accountId).Validate(memberName).Validate(memberIcon).Validate(memberId);
            using (var dataStore = new DataStore())
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

            foreach (var desk in GetDesks(accountId))
            {
                var result = desk.Members.FirstOrDefault(a => a.Id == memberId);
                if (result != null)
                {
                    result.ModifyIcon(newIcon);
                    _deskRepository.Update(desk);
                    break;
                }
            }
        }

        public void ModifyMemberInfo(Int32 accountId, Member member)
        {
            ValidateParameter.Validate(accountId).Validate(member);

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

                    _deskRepository.Update(desk);

                    break;
                }
            }
        }

        public void RemoveMember(Int32 accountId, Int32 memberId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId);
            using (var dataStore = new DataStore())
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

                    if (isFolder)
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
                catch (Exception ex)
                {
                    dataStore.Rollback();
                    throw;
                }
            }
        }
    }
}
