using System;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    [Export(typeof(IModifyAppInfoServices))]
    internal sealed class ModifyAppInfoServices : IModifyAppInfoServices
    {

        [Import]
        public BaseServiceContext BaseContext { get; set; }


        public void ModifyAppStar(Int32 appId, Int32 starCount)
        {
            if (!BaseContext.Query.Find(BaseContext.FilterFactory.Create<Desk>(d => d.Members.Any(m => m.AppId == appId) && d.AccountId == BaseContext.GetAccountId())).Any())
            {
                throw new BusinessException($"请安装这个应用后再打分");
            }

            var appResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create<App>(app => app.Id == appId));

            appResult.AddStar(BaseContext.GetAccountId(), starCount);

            BaseContext.Repository.Create<App>().Update(appResult);
        }

        public void ModifyAccountAppInfo(App app)
        {
            var appResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create<App>(internalApp => internalApp.Id == app.Id && internalApp.AccountId == BaseContext.GetAccountId()));

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

            BaseContext.Repository.Create<App>().Update(appResult);
        }
    }
}
