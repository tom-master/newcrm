using System;
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
    }
}
