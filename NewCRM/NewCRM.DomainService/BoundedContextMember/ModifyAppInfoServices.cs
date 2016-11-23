using System;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Domain.Services.Service;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    [Export(typeof(IModifyAppInfoServices))]
    internal class ModifyAppInfoServices : BaseService, IModifyAppInfoServices
    {

        public void ModifyAppStar(Int32 accountId, Int32 appId, Int32 starCount)
        {
            var accountResult = GetAccountInfoService(accountId);

            if (!accountResult.Config.Desks.Any(desk => desk.Members.Any(member => member.AppId == appId)))
            {
                throw new BusinessException($"请安装这个应用后再打分");
            }

            var appResult = QueryFactory.Create<App>().FindOne(SpecificationFactory.Create<App>(app => app.Id == appId));

            appResult.AddStar(accountId, starCount);

            Repository.Create<App>().Update(appResult);
        }

        public void ModifyAccountAppInfo(Int32 accountId, App app)
        {
            var appResult = QueryFactory.Create<App>().FindOne(SpecificationFactory.Create<App>(internalApp => internalApp.Id == app.Id && internalApp.AccountId == accountId));

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

            if (app.AppAuditState == AppAuditState.UnAuditState)//未审核
            {
                appResult.DontSentAudit();
            }
            else if (app.AppAuditState == AppAuditState.Wait)
            {
                appResult.SentAudit();
            }

            Repository.Create<App>().Update(appResult);

        }
    }
}
