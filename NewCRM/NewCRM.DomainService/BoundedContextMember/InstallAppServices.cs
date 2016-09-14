using System;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Domain.Interface;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Infrastructure.CommonTools.CustemException;


namespace NewCRM.Domain.Services.BoundedContextMember
{
    [Export(typeof(IInstallAppServices))]
    internal sealed class InstallAppServices : BaseService.BaseService, IInstallAppServices
    {
        public void Install(Int32 accountId, Int32 appId, Int32 deskNum)
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
                if (desk.Id != realDeskId)
                {
                    continue;
                }

                desk.Members.Add(newMember);
                Repository.Create<Desk>().Update(desk);

                appResult.AddUseCount();
                Repository.Create<App>().Update(appResult);

                break;
            }
        }
    }
}
