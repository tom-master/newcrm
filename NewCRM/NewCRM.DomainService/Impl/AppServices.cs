using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services.Impl
{
    [Export(typeof(IAppServices))]
    internal class AppServices : BaseService, IAppServices
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

            AppRepository.Update(appResult);
        }

        public void InstallApp(Int32 accountId, Int32 appId, Int32 deskNum)
        {
            var accountResult = GetAccountInfoService(accountId);

            var realDeskId = GetRealDeskIdService(deskNum, accountResult.Config);


            var appResult = QueryFactory.Create<App>().FindOne(SpecificationFactory.Create<App>(app => app.AppAuditState == AppAuditState.Pass && app.AppReleaseState == AppReleaseState.Release && app.Id == appId));

            if (appResult == null)
            {
                throw new BusinessException($"应用添加失败，请刷新重试");
            }

            var newMember = new Member(appResult.Name, appResult.IconUrl, appResult.AppUrl, appResult.Id, appResult.Width, appResult.Height, appResult.IsLock, appResult.IsMax, appResult.IsFull, appResult.IsSetbar, appResult.IsOpenMax, appResult.IsFlash, appResult.IsDraw, appResult.IsResize);

            foreach (var desk in accountResult.Config.Desks)
            {
                if (desk.Id == realDeskId)
                {
                    desk.Members.Add(newMember);
                    DeskRepository.Update(desk);

                    appResult.AddUseCount();
                    AppRepository.Update(appResult);

                    break;
                }
            }
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
                appResult = appResult.DontSentAudit();
            }
            else if (app.AppAuditState == AppAuditState.Wait)
            {
                appResult = appResult.SentAudit();
            }

            AppRepository.Update(appResult);
        }

        public void CreateNewApp(App app)
        {
            var internalApp = new App(
                app.Name, app.IconUrl, app.AppUrl, app.Width, app.Height, app.AppTypeId, app.AppAuditState, app.AppStyle, app.AccountId,
                app.Remark, app.IsMax, app.IsFull, app.IsSetbar, app.IsOpenMax, app.IsFlash, app.IsDraw, app.IsResize);

            AppRepository.Add(internalApp);
        }

    }
}
