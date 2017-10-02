using System;
using System.Linq;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomException;
using IModifyAppInfoServices = NewCRM.Domain.Services.Interface.IModifyAppInfoServices;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    internal sealed class ModifyAppInfoServices : BaseServiceContext, IModifyAppInfoServices
    {

        public void ModifyAppStar(Int32 accountId,Int32 appId, Int32 starCount)
        {
            if (!DatabaseQuery.Find(FilterFactory.Create<Desk>(d => d.Members.Any(m => m.AppId == appId) && d.AccountId == accountId)).Any())
            {
                throw new BusinessException($"请安装这个应用后再打分");
            }

            var appResult = DatabaseQuery.FindOne(FilterFactory.Create<App>(app => app.Id == appId));

            appResult.AddStar(accountId, starCount);

            Repository.Create<App>().Update(appResult);
        }

        public void ModifyAccountAppInfo(Int32 accountId, App app)
        {
            var appResult = DatabaseQuery.FindOne(FilterFactory.Create<App>(internalApp => internalApp.Id == app.Id && internalApp.AccountId == accountId));

            if (appResult == null)
            {
                throw new BusinessException("这个应用可能已被删除，请刷新后再试");
            }

            appResult.ModifyIconUrl(app.IconUrl)
                .ModifyName(app.Name)
                .ModifyAppType(app.AppTypeId)
                .ModifyUrl(app.AppUrl)
                .ModifyWidth(app.Width)
                .ModifyHeight(app.Height)
                .ModifyAppStyle(app.AppStyle)
                .ModifyIsResize(app.IsResize)
                .ModifyIsOpenMax(app.IsOpenMax)
                .ModifyIsFlash(app.IsFlash)
                .ModifyAppRemake(app.Remark);

            if (app.AppAuditState == AppAuditState.Wait)//未审核
            {
                appResult.DontSentAudit();
            }
            else if (app.AppAuditState == AppAuditState.UnAuditState)
            {
                appResult.SentAudit();
            }

            Repository.Create<App>().Update(appResult);
        }
    }
}
