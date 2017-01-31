using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Application.Interface;
using NewCRM.Domain;
using NewCRM.Domain.DomainSpecification;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Domain.ValueObject;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Application.Services
{
    [Export(typeof(IAppApplicationServices))]
    internal class AppApplicationServices : BaseServiceContext, IAppApplicationServices
    {

        private readonly IInstallAppServices _installAppServices;

        private readonly IModifyAppInfoServices _modifyAppInfoServices;

        private readonly IModifyAppTypeServices _modifyAppTypeServices;

        [ImportingConstructor]
        public AppApplicationServices(IInstallAppServices installAppServices,
            IModifyAppInfoServices modifyAppInfoServices,
            IModifyAppTypeServices modifyAppTypeServices)
        {
            _installAppServices = installAppServices;

            _modifyAppInfoServices = modifyAppInfoServices;

            _modifyAppTypeServices = modifyAppTypeServices;
        }

        public IDictionary<String, IList<dynamic>> GetDeskMembers()
        {
            var desks = CacheQuery.Find(FilterFactory.Create((Desk desk) => desk.AccountId == AccountId));

            IDictionary<String, IList<dynamic>> deskDictionary = new Dictionary<String, IList<dynamic>>();

            #region accountConfig

            foreach (var desk in desks)
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
                deskDictionary.Add(new KeyValuePair<String, IList<dynamic>>(desk.DeskNumber.ToString(), deskMembers));
            }
            #endregion

            return deskDictionary;

        }

        public List<AppTypeDto> GetAppTypes()
        {
            return CacheQuery.Find(FilterFactory.Create((AppType appType) => true)).ConvertToDtos<AppType, AppTypeDto>().ToList();
        }

        public TodayRecommendAppDto GetTodayRecommend()
        {

            var topApp = DatabaseQuery.Find(FilterFactory.Create<App>(app => app.AppAuditState == AppAuditState.Pass && app.AppReleaseState == AppReleaseState.Release && app.IsRecommand)).Select(app => new
            {
                app.UseCount,
                AppStars = CountAppStars(app),
                app.Id,
                app.Name,
                app.IconUrl,
                app.Remark,
                app.AppStyle
            }).FirstOrDefault();

            if (topApp == null)
            {
                return new TodayRecommendAppDto();
            }

            var members = CacheQuery.Find(FilterFactory.Create((Desk desk) => desk.AccountId == AccountId)).SelectMany((a, b) => a.Members);

            return DtoConfiguration.ConvertDynamicToDto<TodayRecommendAppDto>(new
            {
                topApp.Id,
                topApp.Name,
                topApp.UseCount,
                AppIcon = topApp.IconUrl,
                StartCount = topApp.AppStars,
                IsInstall = members.Any(member => member.AppId == topApp.Id),
                topApp.Remark,
                Style = topApp.AppStyle.ToString().ToLower()
            });
        }

        public Tuple<Int32, Int32> GetAccountDevelopAppCountAndNotReleaseAppCount()
        {
            var accountApps = DatabaseQuery.Find(FilterFactory.Create<App>(app => app.AccountId == AccountId)).ToArray();

            var accountDevAppCount = accountApps.Length;

            var accountUnReleaseAppCount = accountApps.Count(app => app.AppReleaseState == AppReleaseState.UnRelease);

            return new Tuple<Int32, Int32>(accountDevAppCount, accountUnReleaseAppCount);

        }

        public List<AppDto> GetAllApps(Int32 appTypeId, String searchText, Int32 pageIndex, Int32 pageSize, out Int32 totalCount, Int32 orderId = default(Int32), Int32 accountId = default(Int32), Int32 appStyleId = default(Int32), String appState = default(String))
        {
            ValidateParameter.Validate(orderId, true).Validate(searchText).Validate(pageIndex, true).Validate(pageSize);

            var appSpecification = FilterFactory.Create<App>(app => app.AppAuditState == AppAuditState.Pass && app.AppReleaseState == AppReleaseState.Release);

            #region 条件筛选

            if (appTypeId != 0 && appTypeId != -1)//全部app
            {
                appSpecification.And(app => app.AppTypeId == appTypeId && app.AppTypeId == appTypeId);
            }
            else
            {
                if (appTypeId == -1)//用户制作的app
                {
                    appSpecification.And(app => app.AccountId == AccountId);
                }
            }

            //应用所属类型
            if (appTypeId != default(Int32))
            {
                appSpecification.And(app => app.AppTypeId == appTypeId);
            }

            //应用样式
            if (appStyleId != default(Int32))
            {
                var enumConst = Enum.GetName(typeof(AppStyle), 1);

                AppStyle appStyle;

                if (Enum.TryParse(enumConst, true, out appStyle))
                {
                    appSpecification.And(app => app.AppStyle == appStyle);
                }
                else
                {
                    throw new BusinessException($"无法识别的应用样式：{enumConst}");
                }
            }

            if (appState != default(String))
            {
                //app发布状态
                var stats = appState.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (stats[0] == "AppReleaseState")
                {
                    var enumConst = Enum.GetName(typeof(AppReleaseState), Int32.Parse(stats[1]));

                    AppReleaseState appReleaseState;

                    if (Enum.TryParse(enumConst, true, out appReleaseState))
                    {
                        appSpecification.And(app => app.AppReleaseState == appReleaseState);
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
                        appSpecification.And(app => app.AppAuditState == appAuditState);
                    }
                    else
                    {
                        throw new BusinessException($"无法识别的应用审核状态{enumConst}");
                    }
                }
            }

            switch (orderId)
            {
                case 1:
                    {
                        appSpecification.OrderByDescending(app => app.AddTime);
                        break;
                    }

                case 2:
                    {
                        appSpecification.OrderByDescending(app => app.UseCount);
                        break;
                    }

                case 3:
                    {
                        appSpecification.OrderByDescending(app => app.AppStars.Sum(s => s.StartNum) * 1.0);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            if ((searchText + "").Length > 0)//关键字搜索
            {
                appSpecification.And(app => app.Name.Contains(searchText));
            }

            #endregion

            var appTypes = GetAppTypes();

            var appDtoResult = DatabaseQuery.PageBy(appSpecification, pageIndex, pageSize, out totalCount).Select(app => new
            {
                app.AppTypeId,
                app.AccountId,
                app.AddTime,
                app.UseCount,
                StartCount = CountAppStars(app),
                app.Name,
                app.IconUrl,
                app.Remark,
                app.AppStyle,
                AppTypeName = appTypes.FirstOrDefault(appType => appType.Id == app.AppTypeId).Name,
                app.Id,
                app.IsRecommand,
                IsCreater = app.AccountId == AccountId
            }).ConvertDynamicToDtos<AppDto>().ToList();

            appDtoResult.ForEach(appDto => appDto.IsInstall = IsInstallApp(appDto.Id));

            return appDtoResult;

        }

        public AppDto GetApp(Int32 appId)
        {
            ValidateParameter.Validate(appId);

            var specification = FilterFactory.Create<App>(app => app.Id == appId);

            var appResult = DatabaseQuery.FindOne(specification);


            var appTypes = GetAppTypes();

            return DtoConfiguration.ConvertDynamicToDto<AppDto>(new
            {
                appResult.Name,
                appResult.IconUrl,
                appResult.Remark,
                appResult.UseCount,
                StartCount = CountAppStars(appResult),
                AppTypeName = appTypes.FirstOrDefault(appType => appType.Id == appResult.AppTypeId).Name,
                appResult.AddTime,
                appResult.AccountId,
                appResult.Id,
                appResult.IsResize,
                appResult.IsOpenMax,
                appResult.IsFlash,
                appResult.AppStyle,
                appResult.AppUrl,
                appResult.Width,
                appResult.Height,
                appResult.AppAuditState,
                appResult.AppReleaseState,
                appResult.AppTypeId
            });

        }

        public Boolean IsInstallApp(Int32 appId)
        {
            ValidateParameter.Validate(appId);

            var members = DatabaseQuery.Find(FilterFactory.Create((Desk desk) => desk.AccountId == AccountId)).SelectMany((a, b) => a.Members);

            return members.Any(member => member.AppId == appId);
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

            appStates.AddRange(GetEnumDescriptions(typeof(AppAuditState)));

            appStates.AddRange(GetEnumDescriptions(typeof(AppReleaseState)));

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

        public List<AppDto> GetSystemApp(IEnumerable<Int32> appIds = default(IEnumerable<Int32>))
        {
            var filter = FilterFactory.Create((App app) => app.IsSystem);

            if (appIds != null)
            {
                filter.And(app => appIds.Contains(app.Id));
            }

            var appResult = DatabaseQuery.Find(filter);

            return appResult.Select(app => new AppDto
            {
                Id = app.Id,
                Name = app.Name,
                IconUrl = app.IconUrl
            }).ToList();
        }

        public void ModifyAppDirection(String direction)
        {
            ValidateParameter.Validate(direction);

            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == AccountId));

            if (direction.ToLower() == "x")
            {
                accountResult.Config.ModifyAppDirectionToX();
            }
            else if (direction.ToLower() == "y")
            {
                accountResult.Config.ModifyAppDirectionToY();
            }
            else
            {
                throw new BusinessException($"未能识别的App排列方向:{direction.ToLower()}");
            }

            Repository.Create<Account>().Update(accountResult);

            UnitOfWork.Commit();

        }

        public void ModifyAppIconSize(Int32 newSize)
        {
            ValidateParameter.Validate(newSize);

            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == AccountId));

            accountResult.Config.ModifyDisplayIconLength(newSize);

            Repository.Create<Account>().Update(accountResult);

            UnitOfWork.Commit();
        }

        public void ModifyAppVerticalSpacing(Int32 newSize)
        {
            ValidateParameter.Validate(newSize);

            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == AccountId));

            accountResult.Config.ModifyAppVerticalSpacingLength(newSize);

            Repository.Create<Account>().Update(accountResult);

            UnitOfWork.Commit();
        }

        public void ModifyAppHorizontalSpacing(Int32 newSize)
        {
            ValidateParameter.Validate(newSize);

            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == AccountId));

            accountResult.Config.ModifyAppHorizontalSpacingLength(newSize);

            Repository.Create<Account>().Update(accountResult);

            UnitOfWork.Commit();
        }

        public void ModifyAppStar(Int32 appId, Int32 starCount)
        {
            ValidateParameter.Validate(appId).Validate(starCount, true);

            _modifyAppInfoServices.ModifyAppStar(appId, starCount);

            UnitOfWork.Commit();
        }

        public void InstallApp(Int32 appId, Int32 deskNum)
        {
            ValidateParameter.Validate(appId).Validate(deskNum);

            _installAppServices.Install(appId, deskNum);

            UnitOfWork.Commit();
        }

        public void ModifyAccountAppInfo(AppDto appDto)
        {
            ValidateParameter.Validate(appDto);

            _modifyAppInfoServices.ModifyAccountAppInfo(appDto.ConvertToModel<AppDto, App>());

            UnitOfWork.Commit();
        }

        public void CreateNewApp(AppDto appDto)
        {
            ValidateParameter.Validate(appDto);

            var app = appDto.ConvertToModel<AppDto, App>();

            var internalApp = new App(app.Name, app.IconUrl, app.AppUrl, app.Width, app.Height, app.AppTypeId, app.AppAuditState, app.AppStyle, app.AccountId,
                app.Remark, app.IsMax, app.IsFull, app.IsSetbar, app.IsOpenMax, app.IsFlash, app.IsDraw, app.IsResize);

            Repository.Create<App>().Add(internalApp);

            UnitOfWork.Commit();
        }

        public void RemoveAppType(Int32 appTypeId)
        {
            ValidateParameter.Validate(appTypeId);

            _modifyAppTypeServices.DeleteAppType(appTypeId);

            UnitOfWork.Commit();
        }

        public void CreateNewAppType(AppTypeDto appTypeDto)
        {
            var appType = appTypeDto.ConvertToModel<AppTypeDto, AppType>();

            Repository.Create<AppType>().Add(new AppType(appType.Name));

            UnitOfWork.Commit();

        }

        public void ModifyAppType(AppTypeDto appTypeDto, Int32 appTypeId)
        {
            var internalAppTypeFilter = FilterFactory.Create<AppType>(appType => appType.Id == appTypeId);

            var internalAppType = DatabaseQuery.FindOne(internalAppTypeFilter);

            internalAppType.ModifyAppTypeName(appTypeDto.Name);

            Repository.Create<AppType>().Update(internalAppType);

            UnitOfWork.Commit();

        }

        public void Pass(Int32 appId)
        {
            var appDto = GetApp(appId);

            var app = appDto.ConvertToModel<AppDto, App>();

            app.Pass();

            Repository.Create<App>().Update(app);

            UnitOfWork.Commit();
        }

        public void Deny(Int32 appId)
        {
            var appDto = GetApp(appId);

            var app = appDto.ConvertToModel<AppDto, App>();

            app.Deny();

            Repository.Create<App>().Update(app);

            UnitOfWork.Commit();
        }

        public void SetTodayRecommandApp(Int32 appId)
        {
            //取消之前的今日推荐
            var beforeRecommandApp = DatabaseQuery.FindOne(FilterFactory.Create<App>(app => app.IsRecommand));

            beforeRecommandApp.CancelTodayRecommandApp();

            Repository.Create<App>().Update(beforeRecommandApp);

            //重新设置今日推荐
            var afterRecommandApp = DatabaseQuery.FindOne(FilterFactory.Create<App>(app => app.Id == appId));

            afterRecommandApp.SetTodayRecommandApp();

            Repository.Create<App>().Update(afterRecommandApp);

            UnitOfWork.Commit();


        }

        public void RemoveApp(Int32 appId)
        {
            var internalApp = DatabaseQuery.FindOne(FilterFactory.Create<App>(app => app.Id == appId));

            if (internalApp.AppStars.Any())
            {
                internalApp.AppStars.ToList().ForEach(appStar => appStar.RemoveStar());
            }

            internalApp.Remove();

            Repository.Create<App>().Update(internalApp);

            UnitOfWork.Commit();
        }

        public void ReleaseApp(Int32 appId)
        {
            var internalApp = DatabaseQuery.FindOne(FilterFactory.Create<App>(app => app.Id == appId));

            internalApp.Release();

            Repository.Create<App>().Update(internalApp);

            UnitOfWork.Commit();
        }

        #region private method

        // <summary>
        // <summary> 获取传入的枚举类型的字面量的描述
        // <summary> </summary>
        // <summary> <param name="enumType"></param>
        // <summary> <returns></returns>
        private static IEnumerable<dynamic> GetEnumDescriptions(Type enumType) => enumType.GetFields().Where(field => field.CustomAttributes.Any()).Select(s => new { s.CustomAttributes.ToArray()[0].ConstructorArguments[0].Value, Id = s.GetRawConstantValue(), Type = enumType.Name }).Cast<dynamic>().ToList();

        // <summary> <summary>
        // <summary> 计算app的星级
        // <summary> </summary>
        // <summary> <param name="app"></param>
        // <summary> <returns></returns>
        private static Double CountAppStars(App app) => app.AppStars.Any() ? (app.AppStars.Sum(s => s.StartNum) * 1.0) / (app.AppStars.Count * 1.0) : 0.0;

        #endregion
    }
}

