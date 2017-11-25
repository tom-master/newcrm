using System;
using System.Linq;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Domain.Repositories.IRepository.System;
using NewCRM.Repository.StorageProvider;
using System.Text;

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

            var apps = DatabaseQuery.Find(FilterFactory.Create<App>(app => app.AppTypeId == appTypeId)).ToList();
            if (apps.Any())
            {
                apps.ForEach(app =>
                {
                    app.ModifyAppType(appTypeId);
                });
            }

            var internalAppType = DatabaseQuery.FindOne(FilterFactory.Create<AppType>(appType => appType.Id == appTypeId));
            internalAppType.Remove();
        }
    }
}
