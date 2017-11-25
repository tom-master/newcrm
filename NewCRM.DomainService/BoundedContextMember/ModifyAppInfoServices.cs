using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Repository.StorageProvider;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    public sealed class ModifyAppInfoServices : BaseServiceContext, IModifyAppInfoServices
    {
        public void ModifyAppStar(Int32 accountId, Int32 appId, Int32 starCount)
        {
            ValidateParameter.Validate(accountId).Validate(appId).Validate(starCount);

            if (!DatabaseQuery.Find(FilterFactory.Create<Desk>(d => d.Members.Any(m => m.AppId == appId) && d.AccountId == accountId)).Any())
            {
                throw new BusinessException($"请安装这个应用后再打分");
            }

            var appResult = DatabaseQuery.FindOne(FilterFactory.Create<App>(app => app.Id == appId));
            appResult.AddStar(accountId, starCount);

            _appRepository.Update(appResult);
        }

        public void ModifyAccountAppInfo(Int32 accountId, App app)
        {
            ValidateParameter.Validate(accountId).Validate(accountId).Validate(app);
            using (var dataStore = new DataStore())
            {
                var set = new StringBuilder();
                set.Append($@" IconUrl={app.IconUrl},Name={app.Name},AppTypeId={app.AppTypeId},AppUrl={app.AppUrl},Width={app.Width},Height={app.Height},AppStyle={app.AppStyle},IsResize={app.IsResize},IsOpenMax={app.IsOpenMax},IsFlash={app.IsFlash},Remark={app.Remark} ");
                if (app.AppAuditState == AppAuditState.Wait)
                {
                    set.Append($@" ,AppAuditState={(Int32)AppAuditState.Wait} ");
                }
                else
                {
                    set.Append($@" ,AppAuditState={(Int32)AppAuditState.UnAuditState} ");
                }
                dataStore.SqlExecute(set.ToString());
            }
        }

        public void DeleteAppType(Int32 appTypeId)
        {
            ValidateParameter.Validate(appTypeId);
            using (var dataStore = new DataStore())
            {
                #region 前置条件验证
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.Apps AS a WHERE a.AppTypeId={appTypeId} AND a.IsDeleted=0";
                    if ((Int32)dataStore.SqlScalar(sql) > 0)
                    {
                        throw new BusinessException($@"当前分类下已有绑定app,不能删除当前分类");
                    }
                }
                #endregion

                #region 移除app分类
                {
                    var sql = $@"UPDATE dbo.AppTypes SET IsDeleted=1 WHERE Id={appTypeId}";
                    dataStore.SqlExecute(sql);
                }
                #endregion
            }
        }

        public void CreateNewAppType(AppType appType)
        {
            ValidateParameter.Validate(appType);
            using (var dataStore = new DataStore())
            {
                #region 前置条件验证
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.AppTypes AS a WHERE a.Name=@name AND a.IsDeleted=0";
                    var result = (Int32)dataStore.SqlScalar(sql, new List<SqlParameter> { new SqlParameter("@name", appType.Name) });
                    if (result > 0)
                    {
                        throw new BusinessException($@"分类:{appType.Name},已存在");
                    }
                }
                #endregion

                #region 添加app分类
                {
                    var sql = $@"INSERT dbo.AppTypes
                                (
                                    Name,
                                    Remark,
                                    IsDeleted,
                                    AddTime,
                                    LastModifyTime
                                )
                                VALUES
                                (   N'{appType.Name}',       -- Name - nvarchar(6)
                                    N'{appType.Remark}',       -- Remark - nvarchar(50)
                                    0,      -- IsDeleted - bit
                                    GETDATE(), -- AddTime - datetime
                                    GETDATE()  -- LastModifyTime - datetime
                                )";
                    dataStore.SqlExecute(sql);
                }
                #endregion
            }
        }

        public void ModifyAppType(String appTypeName, Int32 appTypeId)
        {
            ValidateParameter.Validate(appTypeName).Validate(appTypeId);
            using (var dataStore = new DataStore())
            {
                #region 前置条件验证
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.AppTypes AS a WHERE a.Name=@name AND a.IsDeleted=0";
                    var result = (Int32)dataStore.SqlScalar(sql, new List<SqlParameter> { new SqlParameter("@name", appTypeName) });
                    if (result > 0)
                    {
                        throw new BusinessException($@"分类:{appTypeName},已存在");
                    }
                }
                #endregion

                #region 更新app分类
                {
                    var sql = $@"UPDATE dbo.AppTypes SET Name=@name WHERE Id={appTypeId} AND IsDeleted=0";
                    dataStore.SqlExecute(sql, new List<SqlParameter> { new SqlParameter("@name", appTypeName) });
                }
                #endregion
            }
        }

        public void ModifyAppIcon(Int32 accountId, Int32 appId, String newIcon)
        {
            ValidateParameter.Validate(accountId).Validate(appId).Validate(newIcon);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Apps SET IconUrl=@url WHERE Id={appId} AND AccountId={accountId} AND AppAuditState={(Int32)AppAuditState.Pass} AND AppReleaseState={(Int32)AppReleaseState.Release} AND IsDeleted=0";

                dataStore.SqlExecute(sql, new List<SqlParameter> { new SqlParameter("@url", newIcon) });
            }
        }
    }
}
