using System;
using System.Linq;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Domain.Repositories.IRepository.System;

namespace NewCRM.Domain.Services.BoundedContextMember
{

    public sealed class InstallAppServices : BaseServiceContext, IInstallAppServices
    {
        private readonly IAppRepository _appRepository;
        private readonly IDeskRepository _deskRepository;

        public InstallAppServices(IAppRepository appRepository, IDeskRepository deskRepository)
        {
            _appRepository = appRepository;
            _deskRepository = deskRepository;
        }

        public void Install(Int32 accountId, Int32 appId, Int32 deskNum)
        {
            ValidateParameter.Validate(accountId).Validate(appId).Validate(deskNum);

            var desks = DatabaseQuery.Find(FilterFactory.Create((Desk desk) => desk.AccountId == accountId));
            var realDeskId = desks.FirstOrDefault(desk => desk.DeskNumber == deskNum).Id;
            var appResult = DatabaseQuery.FindOne(FilterFactory.Create<App>(app => app.AppAuditState == AppAuditState.Pass && app.AppReleaseState == AppReleaseState.Release && app.Id == appId));

            if (appResult == null)
            {
                throw new BusinessException($"应用添加失败，请刷新重试");
            }

            var newMember = new Member(appResult.Name, appResult.IconUrl, appResult.AppUrl, appResult.Id, appResult.Width, appResult.Height, appResult.IsLock, appResult.IsMax, appResult.IsFull, appResult.IsSetbar, appResult.IsOpenMax, appResult.IsFlash, appResult.IsDraw, appResult.IsResize);
            foreach (var desk in desks)
            {
                if (desk.Id != realDeskId)
                {
                    continue;
                }
                desk.Members.Add(newMember);
                _deskRepository.Update(desk);

                appResult.AddUseCount();
                _appRepository.Update(appResult);
                break;
            }
        }
    }
}
