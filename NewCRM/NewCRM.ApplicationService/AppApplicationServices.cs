using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustemException;
using NewCRM.QueryServices.DomainSpecification;

namespace NewCRM.Application.Services
{
    [Export(typeof(IAppApplicationServices))]
    internal class AppApplicationServices : BaseApplicationServices, IAppApplicationServices
    {

        public IDictionary<Int32, IList<dynamic>> GetAccountDeskMembers(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            var accountConfig = Query.CreateQuery<Account>().Find(new Specification<Account>(account => account.Id == accountId)).FirstOrDefault()?.Config;

            IDictionary<Int32, IList<dynamic>> desks = new Dictionary<Int32, IList<dynamic>>();

            #region accountConfig

            foreach (var desk in accountConfig.Desks)
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
            #endregion

            return desks;
        }

        public void ModifyAppDirection(Int32 accountId, String direction)
        {
            ValidateParameter.Validate(accountId).Validate(direction);
            AccountContext.ConfigServices.ModifyAppDirection(accountId, direction);
        }

        public void ModifyAppIconSize(Int32 accountId, Int32 newSize)
        {
            ValidateParameter.Validate(accountId).Validate(newSize);
            AccountContext.ConfigServices.ModifyAppIconSize(accountId, newSize);
        }

        public void ModifyAppVerticalSpacing(Int32 accountId, Int32 newSize)
        {
            ValidateParameter.Validate(accountId).Validate(newSize);
            AccountContext.ConfigServices.ModifyAppVerticalSpacing(accountId, newSize);
        }

        public void ModifyAppHorizontalSpacing(Int32 accountId, Int32 newSize)
        {
            ValidateParameter.Validate(accountId).Validate(newSize);
            AccountContext.ConfigServices.ModifyAppHorizontalSpacing(accountId, newSize);
        }

        public List<AppTypeDto> GetAppTypes()
        {
            return Query.CreateQuery<AppType>().Find(new Specification<AppType>(appType => true)).ConvertToDtos<AppType, AppTypeDto>().ToList();
        }

        public TodayRecommendAppDto GetTodayRecommend(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            var topApp = Query.CreateQuery<App>().Find(new Specification<App>(app => app.AppAuditState == AppAuditState.Pass && app.AppReleaseState == AppReleaseState.Release)).OrderByDescending(app => app.UseCount).Select(app => new
            {
                app.UseCount,
                AppStars = app.AppStars.Any() ? (app.AppStars.Sum(s => s.StartNum) * 1.0) / (app.AppStars.Count * 1.0) : 0.0,
                app.Id,
                app.Name,
                app.IconUrl,
                app.Remark,
                app.AppStyle
            }).FirstOrDefault();

            var accountDesks = Query.CreateQuery<Account>().Find(new Specification<Account>(account => account.Id == accountId)).FirstOrDefault()?.Config.Desks;

            var isInstall = accountDesks.Any(accountDesk => accountDesk.Members.Any(member => member.AppId == topApp.Id));


            return DtoConfiguration.ConvertDynamicToDto<TodayRecommendAppDto>(new
            {
                AppId = topApp.Id,
                topApp.Name,
                topApp.UseCount,
                AppIcon = topApp.IconUrl,
                StartCount = topApp.AppStars,
                IsInstall = isInstall,
                topApp.Remark,
                Style = topApp.AppStyle.ToString().ToLower()
            });


        }

        public Tuple<Int32, Int32> GetAccountDevelopAppCountAndNotReleaseAppCount(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            var accountApps = Query.CreateQuery<App>().Find(new Specification<App>(app => app.AccountId == accountId));

            var accountDevAppCount = accountApps.Count();

            var accountUnReleaseAppCount = accountApps.Count(app => app.AppReleaseState == AppReleaseState.UnRelease);

            return new Tuple<Int32, Int32>(accountDevAppCount, accountUnReleaseAppCount);

        }

        public List<AppDto> GetAllApps(Int32 accountId, Int32 appTypeId, Int32 orderId, String searchText, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(accountId, true).Validate(orderId).Validate(searchText).Validate(pageIndex, true).Validate(pageSize);

            ISpecification<App> appSpecification = new Specification<App>(app => true);

            #region 条件筛选

            if (appTypeId != 0 && appTypeId != -1)//全部app
            {
                appSpecification.And(new Specification<App>(app => app.AppTypeId == appTypeId));
            }
            else
            {
                if (appTypeId == -1)//用户制作的app
                {
                    appSpecification.And(new Specification<App>(app => app.AccountId == accountId));
                }
            }

            if (orderId == 1)//最新应用
            {
                appSpecification.OrderByDescending(new Specification<App>(app => app.AddTime));
            }
            else if (orderId == 2)//使用最多
            {
                appSpecification.OrderByDescending(new Specification<App>(app => app.UseCount));
            }
            else if (orderId == 3)//评价最高
            {
                appSpecification.OrderByDescending(new Specification<App>(app => app.AppStars));
            }

            if ((searchText + "").Length > 0)//关键字搜索
            {
                appSpecification.And(new Specification<App>(app => app.Name.Contains(searchText)));
            }

            #endregion

            var appDtoResult = Query.CreateQuery<App>().PageBy(appSpecification, pageIndex, pageSize, out totalCount).Select(app => new
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
            }).ConvertDynamicToDtos<AppDto>().ToList();

            appDtoResult.ForEach(appDto => appDto.IsInstall = IsInstallApp(accountId, appDto.Id));

            return appDtoResult;
        }

        public AppDto GetApp(Int32 appId)
        {
            ValidateParameter.Validate(appId);

            return DtoConfiguration.ConvertDynamicToDto<AppDto>(Query.CreateQuery<App>().Find(new Specification<App>(app => app.Id == appId)).Where(app => app.Id == appId).Select(app => new
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
            }).FirstOrDefault());
        }

        public void ModifyAppStar(Int32 accountId, Int32 appId, Int32 starCount)
        {
            ValidateParameter.Validate(accountId).Validate(appId).Validate(starCount, true);

            AppServices.ModifyAppStar(accountId, appId, starCount);
        }

        public void InstallApp(Int32 accountId, Int32 appId, Int32 deskNum)
        {
            ValidateParameter.Validate(accountId).Validate(appId).Validate(deskNum);
            AppServices.InstallApp(accountId, appId, deskNum);
        }

        public Boolean IsInstallApp(Int32 accountId, Int32 appId)
        {
            ValidateParameter.Validate(accountId).Validate(appId);

            var accountResult = Query.CreateQuery<Account>().Find(new Specification<Account>(account => account.Id == accountId)).FirstOrDefault();

            return accountResult.Config.Desks.Any(desk => desk.Members.Any(member => member.AppId == appId));

        }

        public IEnumerable<AppStyleDto> GetAllAppStyles()
        {
            var descriptions = GetEnumDescriptions(typeof(AppStyle));
            foreach (var description in descriptions)
            {
                yield return new AppStyleDto
                {
                    Id = description.Id,
                    Name = description.Value,
                    Type = description.Type
                };
            }
        }

        public IEnumerable<AppStateDto> GetAllAppStates()
        {
            var appStates = new List<dynamic>();

            var appAuditStates = GetEnumDescriptions(typeof(AppAuditState));
            appStates.AddRange(appAuditStates);

            var appReleaseStates = GetEnumDescriptions(typeof(AppReleaseState));
            appStates.AddRange(appReleaseStates);

            foreach (var appState in appStates)
            {
                yield return new AppStateDto
                {
                    Id = appState.Id,
                    Name = appState.Value,
                    Type = appState.Type
                };
            }
        }

        public List<AppDto> GetAccountAllApps(Int32 accountId, String appName, Int32 appTypeId, Int32 appStyleId, String appState, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(accountId).Validate(appName).Validate(appTypeId, true).Validate(appStyleId, true).Validate(pageIndex).Validate(pageSize);

            ISpecification<App> appSpecification = new Specification<App>(app => true);

            #region 条件筛选

            //应用名称
            if ((appName + "").Length > 0)
            {
                appSpecification.And(new Specification<App>(app => app.Name.Contains(appName)));
            }

            //应用所属类型
            if (appTypeId != 0)
            {
                appSpecification.And(new Specification<App>(app => app.AppTypeId == appTypeId));
            }

            //应用样式
            if (appStyleId != 0)
            {
                var enumConst = Enum.GetName(typeof(AppStyle), 1);

                AppStyle appStyle;

                if (Enum.TryParse(enumConst, true, out appStyle))
                {
                    appSpecification.And(new Specification<App>(app => app.AppStyle == appStyle));

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
                        appSpecification.And(new Specification<App>(app => app.AppReleaseState == appReleaseState));
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
                        appSpecification.And(new Specification<App>(app => app.AppAuditState == appAuditState));
                    }
                    else
                    {
                        throw new BusinessException($"无法识别的应用审核状态{enumConst}");
                    }
                }
            }

            #endregion

            return Query.CreateQuery<App>().PageBy(appSpecification, pageIndex, pageSize, out totalCount).Select(app => new
            {
                app.Name,
                app.AppStyle,
                AppType = app.AppType.Name,
                app.UseCount,
                app.Id,
                app.IconUrl
            }).ConvertDynamicToDtos<AppDto>().ToList();

        }

        public void ModifyAccountAppInfo(Int32 accountId, AppDto appDto)
        {
            ValidateParameter.Validate(accountId).Validate(appDto);

            AppServices.ModifyAccountAppInfo(accountId, appDto.ConvertToModel<AppDto, App>());
        }

        public void CreateNewApp(AppDto app)
        {
            ValidateParameter.Validate(app);
            AppServices.CreateNewApp(app.ConvertToModel<AppDto, App>());
        }

        #region private method
        /// <summary>
        /// 获取传入的枚举类型的字面量的描述
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        private IEnumerable<dynamic> GetEnumDescriptions(Type enumType)
        {
            return enumType.GetFields().Where(field => field.CustomAttributes.Any()).Select(s => new { s.CustomAttributes.ToArray()[0].ConstructorArguments[0].Value, Id = s.GetRawConstantValue(), Type = enumType.Name }).Cast<dynamic>().ToList();
        }
        #endregion
    }
}
