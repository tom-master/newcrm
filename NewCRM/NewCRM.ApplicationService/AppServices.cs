using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Factory.DomainSpecification;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.Application.Services.Interface;
using NewCRM.Domain.Repositories.IRepository.Agent;
using NewCRM.Domain.Repositories.IRepository.System;

namespace NewCRM.Application.Services
{
    public class AppServices : BaseServiceContext, IAppServices
    {

        private readonly IInstallAppServices _installAppServices;
        private readonly IModifyAppInfoServices _modifyAppInfoServices;
        private readonly IModifyAppTypeServices _modifyAppTypeServices;

        private readonly IAccountRepository _accountRepository;
        private readonly IAppRepository _appRepository;
        private readonly IAppTypeRepository _appTypeRepository;


        public AppServices(IInstallAppServices installAppServices, IModifyAppInfoServices modifyAppInfoServices, IModifyAppTypeServices modifyAppTypeServices, IAccountRepository accountRepository, IAppRepository appRepository, IAppTypeRepository appTypeRepository)
        {
            _installAppServices = installAppServices;
            _modifyAppInfoServices = modifyAppInfoServices;
            _modifyAppTypeServices = modifyAppTypeServices;

            _accountRepository = accountRepository;
            _appRepository = appRepository;
            _appTypeRepository = appTypeRepository;
        }

        public IDictionary<String, IList<dynamic>> GetDeskMembers(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            var desks = CacheQuery.Find(FilterFactory.Create((Desk desk) => desk.AccountId == accountId));
            var deskDictionary = new Dictionary<String, IList<dynamic>>();

            foreach (var desk in desks)
            {
                var deskMembers = new List<dynamic>();
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
                deskDictionary.Add(desk.DeskNumber.ToString(), deskMembers);
            }

            return deskDictionary;

        }

        public List<AppTypeDto> GetAppTypes()
        {
            return CacheQuery.Find(FilterFactory.Create((AppType appType) => true)).ConvertToDtos<AppType, AppTypeDto>().ToList();
        }

        public TodayRecommendAppDto GetTodayRecommend(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

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

            var members = CacheQuery.Find(FilterFactory.Create((Desk desk) => desk.AccountId == accountId)).SelectMany((a, b) => a.Members);

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

        public Tuple<Int32, Int32> GetAccountDevelopAppCountAndNotReleaseAppCount(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            var accountApps = DatabaseQuery.Find(FilterFactory.Create<App>(app => app.AccountId == accountId)).ToArray();
            var accountDevAppCount = accountApps.Length;
            var accountUnReleaseAppCount = accountApps.Count(app => app.AppReleaseState == AppReleaseState.UnRelease);

            return new Tuple<Int32, Int32>(accountDevAppCount, accountUnReleaseAppCount);

        }

        public List<AppDto> GetAllApps(Int32 accountId, Int32 appTypeId, Int32 orderId, String searchText, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(accountId, true).Validate(orderId).Validate(searchText).Validate(pageIndex, true).Validate(pageSize);

            var filter = FilterFactory.Create<App>(app => app.AppAuditState == AppAuditState.Pass && app.AppReleaseState == AppReleaseState.Release);

            #region 条件筛选

            if (appTypeId != 0 && appTypeId != -1)//全部app
            {
                filter.And(app => app.AppTypeId == appTypeId);
            }
            else
            {
                if (appTypeId == -1)//用户制作的app
                {
                    filter.And(app => app.AccountId == accountId);
                }
            }

            switch (orderId)
            {
                case 1:
                    {
                        filter.OrderByDescending(app => app.AddTime);
                        break;
                    }
                case 2:
                    {
                        filter.OrderByDescending(app => app.UseCount);
                        break;
                    }
                case 3:
                    {
                        filter.OrderByDescending(app => app.AppStars.Sum(s => s.StartNum) * 1.0);
                        break;
                    }
            }

            if ((searchText + "").Length > 0)//关键字搜索
            {
                filter.And(app => app.Name.Contains(searchText));
            }

            #endregion


            var appTypes = GetAppTypes();
            var appDtoResult = DatabaseQuery.PageBy(filter, pageIndex, pageSize, out totalCount).Select(app => new
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
                app.Id
            }).ConvertDynamicToDtos<AppDto>().ToList();

            appDtoResult.ForEach(appDto => appDto.IsInstall = IsInstallApp(accountId, appDto.Id));

            return appDtoResult;

        }

        public List<AppDto> GetAccountAllApps(Int32 accountId, String searchText, Int32 appTypeId, Int32 appStyleId, String appState, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(accountId, true).Validate(searchText).Validate(appTypeId, true).Validate(appStyleId, true).Validate(pageIndex).Validate(pageSize);
            var filter = FilterFactory.Create<App>();

            #region 条件筛选

            if (accountId != default(Int32))
            {
                filter.And(app => app.AccountId == accountId);
            }

            //应用名称
            if ((searchText + "").Length > 0)
            {
                filter.And(app => app.Name.Contains(searchText));
            }

            //应用所属类型
            if (appTypeId != 0)
            {
                filter.And(app => app.AppTypeId == appTypeId);
            }

            //应用样式
            if (appStyleId != 0)
            {
                var appStyle = EnumExtensions.ParseToEnum<AppStyle>(appStyleId);

                filter.And(app => app.AppStyle == appStyle);
            }

            if ((appState + "").Length > 0)
            {
                //app发布状态
                var stats = appState.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (stats[0] == "AppReleaseState")
                {
                    var appReleaseState = EnumExtensions.ParseToEnum<AppReleaseState>(Int32.Parse(stats[1]));

                    filter.And(app => app.AppReleaseState == appReleaseState);
                }

                //app应用审核状态
                if (stats[0] == "AppAuditState")
                {
                    var appAuditState = EnumExtensions.ParseToEnum<AppAuditState>(Int32.Parse(stats[1]));

                    filter.And(app => app.AppAuditState == appAuditState);
                }
            }

            #endregion

            var appTypes = GetAppTypes();
            return DatabaseQuery.PageBy(filter, pageIndex, pageSize, out totalCount).Select(app => new
            {
                app.Name,
                app.AppStyle,
                AppTypeName = appTypes.FirstOrDefault(appType => appType.Id == app.AppTypeId).Name,
                app.UseCount,
                app.Id,
                app.IconUrl,
                app.AppAuditState,
                app.IsRecommand,
                IsCreater = app.AccountId == accountId
            }).ConvertDynamicToDtos<AppDto>().ToList();
        }

        public AppDto GetApp(Int32 appId)
        {
            ValidateParameter.Validate(appId);

            var filter = FilterFactory.Create<App>(app => app.Id == appId);
            var appResult = DatabaseQuery.FindOne(filter);
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

        public Boolean IsInstallApp(Int32 accountId, Int32 appId)
        {
            ValidateParameter.Validate(accountId).Validate(appId);

            var members = DatabaseQuery.Find(FilterFactory.Create((Desk desk) => desk.AccountId == accountId)).SelectMany((a, b) => a.Members);
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

        public void ModifyAppDirection(Int32 accountId, String direction)
        {
            ValidateParameter.Validate(accountId).Validate(direction);

            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == accountId));
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

            _accountRepository.Update(accountResult);
            UnitOfWork.Commit();
        }

        public void ModifyAppIconSize(Int32 accountId, Int32 newSize)
        {
            ValidateParameter.Validate(accountId).Validate(newSize);

            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == accountId));
            accountResult.Config.ModifyDisplayIconLength(newSize);

            _accountRepository.Update(accountResult);
            UnitOfWork.Commit();
        }

        public void ModifyAppVerticalSpacing(Int32 accountId, Int32 newSize)
        {
            ValidateParameter.Validate(accountId).Validate(newSize);

            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == accountId));
            accountResult.Config.ModifyAppVerticalSpacingLength(newSize);

            _accountRepository.Update(accountResult);
            UnitOfWork.Commit();
        }

        public void ModifyAppHorizontalSpacing(Int32 accountId, Int32 newSize)
        {
            ValidateParameter.Validate(accountId).Validate(newSize);

            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == accountId));
            accountResult.Config.ModifyAppHorizontalSpacingLength(newSize);

            _accountRepository.Update(accountResult);
            UnitOfWork.Commit();
        }

        public void ModifyAppStar(Int32 accountId, Int32 appId, Int32 starCount)
        {
            ValidateParameter.Validate(accountId).Validate(appId).Validate(starCount, true);

            _modifyAppInfoServices.ModifyAppStar(accountId, appId, starCount);
            UnitOfWork.Commit();
        }

        public void InstallApp(Int32 accountId, Int32 appId, Int32 deskNum)
        {
            ValidateParameter.Validate(accountId).Validate(appId).Validate(deskNum);

            _installAppServices.Install(accountId, appId, deskNum);
            UnitOfWork.Commit();
        }

        public void ModifyAccountAppInfo(Int32 accountId, AppDto appDto)
        {
            ValidateParameter.Validate(accountId).Validate(appDto);

            _modifyAppInfoServices.ModifyAccountAppInfo(accountId, appDto.ConvertToModel<AppDto, App>());
            UnitOfWork.Commit();
        }

        public void CreateNewApp(AppDto appDto)
        {
            ValidateParameter.Validate(appDto);

            var app = appDto.ConvertToModel<AppDto, App>();
            var internalApp = new App(app.Name, app.IconUrl, app.AppUrl, app.Width, app.Height, app.AppTypeId, app.AppAuditState,AppReleaseState.UnRelease, app.AppStyle, app.AccountId,
                app.Remark, app.IsMax, app.IsFull, app.IsSetbar, app.IsOpenMax, app.IsFlash, app.IsDraw, app.IsResize);

            _appRepository.Add(internalApp);
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
            ValidateParameter.Validate(appTypeDto);
            var appType = appTypeDto.ConvertToModel<AppTypeDto, AppType>();

            _appTypeRepository.Add(new AppType(appType.Name));
            UnitOfWork.Commit();

        }

        public void ModifyAppType(AppTypeDto appTypeDto, Int32 appTypeId)
        {
            ValidateParameter.Validate(appTypeDto).Validate(appTypeId);

            var internalAppTypeFilter = FilterFactory.Create<AppType>(appType => appType.Id == appTypeId);
            var internalAppType = DatabaseQuery.FindOne(internalAppTypeFilter);

            internalAppType.ModifyAppTypeName(appTypeDto.Name);
            _appTypeRepository.Update(internalAppType);

            UnitOfWork.Commit();

        }

        public void Pass(Int32 appId)
        {
            ValidateParameter.Validate(appId);

            var appDto = GetApp(appId);
            var app = appDto.ConvertToModel<AppDto, App>();
            app.Pass();

            _appRepository.Update(app);
            UnitOfWork.Commit();
        }

        public void Deny(Int32 appId)
        {
            ValidateParameter.Validate(appId);

            var appDto = GetApp(appId);
            var app = appDto.ConvertToModel<AppDto, App>();
            app.Deny();

            _appRepository.Update(app);
            UnitOfWork.Commit();
        }

        public void SetTodayRecommandApp(Int32 appId)
        {
            ValidateParameter.Validate(appId);

            //取消之前的今日推荐
            var beforeRecommandApp = DatabaseQuery.FindOne(FilterFactory.Create<App>(app => app.IsRecommand));
            beforeRecommandApp.CancelTodayRecommandApp();
            _appRepository.Update(beforeRecommandApp);

            //重新设置今日推荐
            var afterRecommandApp = DatabaseQuery.FindOne(FilterFactory.Create<App>(app => app.Id == appId));
            afterRecommandApp.SetTodayRecommandApp();
            _appRepository.Update(afterRecommandApp);

            UnitOfWork.Commit();

        }

        public void RemoveApp(Int32 appId)
        {
            ValidateParameter.Validate(appId);

            var internalApp = DatabaseQuery.FindOne(FilterFactory.Create<App>(app => app.Id == appId));
            if (internalApp.AppStars.Any())
            {
                internalApp.AppStars.ToList().ForEach(appStar => appStar.RemoveStar());
            }

            internalApp.Remove();
            _appRepository.Update(internalApp);

            UnitOfWork.Commit();
        }

        public void ReleaseApp(Int32 appId)
        {
            ValidateParameter.Validate(appId);

            var internalApp = DatabaseQuery.FindOne(FilterFactory.Create<App>(app => app.Id == appId));
            internalApp.Release();

            _appRepository.Update(internalApp);
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

