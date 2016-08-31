using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Domain.Services.DomainSpecification;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services.Impl
{
    [Export(typeof(IAppServices))]
    internal class AppServices : BaseService, IAppServices
    {
        public List<AppType> GetAppTypes()
        {
            return AppTypeRepository.Entities.ToList();
        }

        public IDictionary<Int32, IList<dynamic>> GetAccountDeskMembers(Int32 accountId)
        {
            var accountConfig = GetAccountInfoService(accountId);

            IDictionary<Int32, IList<dynamic>> desks = new Dictionary<Int32, IList<dynamic>>();

            foreach (var desk in accountConfig.Config.Desks)
            {
                IList<dynamic> deskMembers = new List<dynamic>();

                var members = desk.Members.ToList();

                foreach (var member in members)
                {
                    if (member.MemberType == MemberType.Folder)
                    {
                        deskMembers.Add(new
                        {
                            type = member.MemberType.ToString().ToLower(),
                            memberId = member.Id,
                            appId = member.AppId,
                            name = member.Name,
                            icon = member.IconUrl,
                            width = member.Width,
                            height = member.Height,
                            isOnDock = member.IsOnDock,
                            isDraw = member.IsDraw,
                            isOpenMax = member.IsOpenMax,
                            isSetbar = member.IsSetbar,
                            apps = members.Where(m => m.FolderId == member.Id).Select(app => new
                            {
                                type = app.MemberType.ToString().ToLower(),
                                memberId = app.Id,
                                appId = app.AppId,
                                name = app.Name,
                                icon = app.IconUrl,
                                width = app.Width,
                                height = app.Height,
                                isOnDock = app.IsOnDock,
                                isDraw = app.IsDraw,
                                isOpenMax = app.IsOpenMax,
                                isSetbar = app.IsSetbar,
                            })
                        });
                    }
                    else
                    {
                        if (member.FolderId == 0)
                        {
                            var internalType = member.MemberType.ToString().ToLower();
                            deskMembers.Add(new
                            {
                                type = internalType,
                                memberId = member.Id,
                                appId = member.AppId,
                                name = member.Name,
                                icon = member.IconUrl,
                                width = member.Width,
                                height = member.Height,
                                isOnDock = member.IsOnDock,
                                isDraw = member.IsDraw,
                                isOpenMax = member.IsOpenMax,
                                isSetbar = member.IsSetbar
                            });
                        }
                    }
                }
                desks.Add(new KeyValuePair<Int32, IList<dynamic>>(desk.DeskNumber, deskMembers));
            }

            return desks;
        }

        public dynamic GetTodayRecommend(Int32 accountId)
        {
            var topApp = AppRepository.Entities.Where(app => app.AppAuditState == AppAuditState.Pass && app.AppReleaseState == AppReleaseState.Release).OrderByDescending(app => app.UseCount).Select(app => new
            {
                app.UseCount,
                AppStars = app.AppStars.Any() ? (app.AppStars.Sum(s => s.StartNum) * 1.0) / (app.AppStars.Count * 1.0) : 0.0,
                app.Id,
                app.Name,
                app.IconUrl,
                app.Remark,
                app.AppStyle
            }).FirstOrDefault();

            var accountDesks = GetAccountInfoService(accountId).Config.Desks;

            Boolean isInstall = accountDesks.Any(accountDesk => accountDesk.Members.Any(member => member.AppId == topApp.Id));

            return new
            {
                AppId = topApp.Id,
                topApp.Name,
                topApp.UseCount,
                AppIcon = topApp.IconUrl,
                StartCount = topApp.AppStars,
                IsInstall = isInstall,
                topApp.Remark,
                Style = topApp.AppStyle.ToString().ToLower()
            };
        }

        public Tuple<Int32, Int32> GetAccountDevelopAppCountAndNotReleaseAppCount(Int32 accountId)
        {
            var accountApps = AppRepository.Entities.Where(app => app.AccountId == accountId);

            var accountDevAppCount = accountApps.Count();

            var accountUnReleaseAppCount = accountApps.Count(app => app.AppReleaseState == AppReleaseState.UnRelease);

            return new Tuple<Int32, Int32>(accountDevAppCount, accountUnReleaseAppCount);
        }

        public List<dynamic> GetAllApps(Int32 accountId, Int32 appTypeId, Int32 orderId, String searchText, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {

            var apps = AppRepository.Entities.Where(app => app.AppAuditState == AppAuditState.Pass && app.AppReleaseState == AppReleaseState.Release);

            #region 条件筛选

            if (appTypeId != 0 && appTypeId != -1)//全部app
            {
                apps = apps.Where(app => app.AppTypeId == appTypeId);
            }
            else
            {
                if (appTypeId == -1)//用户制作的app
                {
                    apps = apps.Where(app => app.AccountId == accountId);
                }
            }

            if (orderId == 1)//最新应用
            {
                apps = apps.OrderByDescending(app => app.AddTime);
            }
            else if (orderId == 2)//使用最多
            {
                apps = apps.OrderByDescending(app => app.UseCount);
            }
            else if (orderId == 3)//评价最高
            {
                apps = apps.OrderByDescending(app => app.AppStars);
            }

            if ((searchText + "").Length > 0)//关键字搜索
            {
                apps = apps.Where(app => app.Name.Contains(searchText));
            }

            totalCount = apps.Count();


            #endregion

            return apps.OrderByDescending(o => o.AddTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(app => new
            {
                app.AppTypeId,
                app.AccountId,
                app.AddTime,
                app.UseCount,
                StartCount = app.AppStars.Any() ? (app.AppStars.Sum(s => s.StartNum) * 1.0) / (app.AppStars.Count * 1.0) : 0.0,
                app.Name,
                app.IconUrl,
                app.Remark,
                app.AppStyle,
                AppType = app.AppType.Name,
                app.Id
            }).ToList<dynamic>();
        }

        public dynamic GetApp(Int32 appId)
        {
            var appResult =
                AppRepository.Entities.Where(
                    app =>
                        app.Id == appId).Select(app => new
                        {
                            app.Name,
                            app.IconUrl,
                            app.Remark,
                            app.UseCount,
                            StartCount = app.AppStars.Any() ? (app.AppStars.Sum(s => s.StartNum) * 1.0) / (app.AppStars.Count * 1.0) : 0.0,
                            AppType = app.AppType.Name,
                            app.AddTime,
                            app.AccountId,
                            app.Id,
                            app.IsResize,
                            app.IsOpenMax,
                            app.IsFlash,
                            app.AppStyle,
                            app.AppUrl,
                            app.Width,
                            app.Height,
                            app.AppAuditState,
                            app.AppTypeId
                        }).FirstOrDefault();
            return appResult;
        }

        public void ModifyAppStar(Int32 accountId, Int32 appId, Int32 starCount)
        {
            var accountResult = GetAccountInfoService(accountId);

            if (!accountResult.Config.Desks.Any(desk => desk.Members.Any(member => member.AppId == appId)))
            {
                throw new BusinessException($"请安装这个应用后再打分");
            }

            var appResult = AppRepository.Entities.FirstOrDefault(app => app.Id == appId);

            appResult.AddStar(accountId, starCount);

            AppRepository.Update(appResult);
        }

        public void InstallApp(Int32 accountId, Int32 appId, Int32 deskNum)
        {
            var accountResult = GetAccountInfoService(accountId);

            var realDeskId = GetRealDeskIdService(deskNum, accountResult.Config);

            var appResult = AppRepository.Entities.Where(app => app.AppAuditState == AppAuditState.Pass && app.AppReleaseState == AppReleaseState.Release).FirstOrDefault(app => app.Id == appId);

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

        public Boolean IsInstallApp(Int32 accountId, Int32 appId)
        {
            var accountResult = GetAccountInfoService(accountId);

            return accountResult.Config.Desks.Any(desk => desk.Members.Any(member => member.AppId == appId));
        }

        public List<dynamic> GetAccountAllApps(Int32 accountId, String appName, Int32 appTypeId, Int32 appStyleId, String appState, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            var accountApps = AppRepository.Entities.Where(app => app.AccountId == accountId);

            #region 条件筛选

            //应用名称
            if ((appName + "").Length > 0)
            {
                accountApps = accountApps.Where(app => app.Name.Contains(appName));
            }

            //应用所属类型
            if (appTypeId != 0)
            {
                accountApps = accountApps.Where(app => app.AppTypeId == appTypeId);
            }

            //应用样式
            if (appStyleId != 0)
            {
                var enumConst = Enum.GetName(typeof(AppStyle), 1);

                AppStyle appStyle;

                if (Enum.TryParse(enumConst, true, out appStyle))
                {
                    accountApps = accountApps.Where(app => app.AppStyle == appStyle);
                }
                else
                {
                    throw new BusinessException($"无法识别的应用样式：{enumConst}");
                }
            }

            if ((appState + "").Length > 0)
            {
                //app发布状态
                var stats = appState.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (stats[0] == "AppReleaseState")
                {
                    var enumConst = Enum.GetName(typeof(AppReleaseState), Int32.Parse(stats[1]));

                    AppReleaseState appReleaseState;

                    if (Enum.TryParse(enumConst, true, out appReleaseState))
                    {
                        accountApps = accountApps.Where(app => app.AppReleaseState == appReleaseState);
                    }
                    else
                    {
                        throw new BusinessException($"无法识别的应用状态：{enumConst}");
                    }
                }

                //app应用审核状态
                if (stats[0] == "AppAuditState")
                {
                    var enumConst = Enum.GetName(typeof(AppAuditState), Int32.Parse(stats[1]));

                    AppAuditState appAuditState;

                    if (Enum.TryParse(enumConst, true, out appAuditState))
                    {
                        accountApps = accountApps.Where(app => app.AppAuditState == appAuditState);
                    }
                    else
                    {
                        throw new BusinessException($"无法识别的应用审核状态{enumConst}");
                    }
                }
            }

            totalCount = accountApps.Count();

            #endregion

            var result = accountApps.OrderByDescending(o => o.AddTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(
             app => new
             {
                 app.Name,
                 app.AppStyle,
                 AppType = app.AppType.Name,
                 app.UseCount,
                 app.Id,
                 app.IconUrl
             }).ToList<dynamic>();

            return result;
        }

        public void ModifyAccountAppInfo(Int32 accountId, App app)
        {
            var appResult = AppRepository.Entities.FirstOrDefault(internalApp => internalApp.Id == app.Id && internalApp.AccountId == accountId);

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
