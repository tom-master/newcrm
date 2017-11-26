using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Application.Services.Interface;
using NewCRM.Domain;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Dto;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Application.Services
{
    public class AppServices : BaseServiceContext, IAppServices
    {
        private readonly IInstallAppServices _installAppServices;
        private readonly IModifyAppInfoServices _modifyAppInfoServices;
        private readonly IMemberContext _memberServices;
        private readonly IAppTypeServices _appTypeServices;
        private readonly IRecommendAppServices _recommendAppServices;
        private readonly IAppContext _appContext;
        private readonly IDeskContext _deskContext;

        public AppServices(IInstallAppServices installAppServices,
            IModifyAppInfoServices modifyAppInfoServices,
            IMemberContext memberServices,
            IAppTypeServices appTypeServices,
            IRecommendAppServices recommendAppServices,
            IAppContext appContext,
            IDeskContext deskContext)
        {
            _installAppServices = installAppServices;
            _modifyAppInfoServices = modifyAppInfoServices;
            _memberServices = memberServices;
            _appTypeServices = appTypeServices;
            _recommendAppServices = recommendAppServices;
            _appContext = appContext;
            _deskContext = deskContext;
        }

        public IDictionary<String, IList<dynamic>> GetDeskMembers(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            var result = _memberServices.GetDeskMembers(accountId);
            var deskGroup = result.GroupBy(a => a.DeskIndex);
            var deskDictionary = new Dictionary<String, IList<dynamic>>();
            foreach (var desk in deskGroup)
            {
                var members = desk.ToList();
                var deskMembers = new List<dynamic>();
                foreach (var member in desk.ToList())
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
                deskDictionary.Add(desk.ToString(), deskMembers);
            }

            return deskDictionary;
        }

        public List<AppTypeDto> GetAppTypes()
        {
            return _appTypeServices.GetAppTypes().Select(s => new AppTypeDto
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
        }

        public TodayRecommendAppDto GetTodayRecommend(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            var result = _recommendAppServices.GetTodayRecommend(accountId);
            if (result == null)
            {
                return new TodayRecommendAppDto();
            }

            return new TodayRecommendAppDto
            {
                Id = result.Id,
                Name = result.Name,
                UseCount = result.UseCount,
                AppIcon = result.IconUrl,
                StartCount = result.AppStars,
                IsInstall = result.IsInstall,
                Remark = result.Remark,
                Style = result.AppStyle
            };
        }

        public Tuple<Int32, Int32> GetAccountDevelopAppCountAndNotReleaseAppCount(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            var result = _installAppServices.GetAccountDevelopAppCountAndNotReleaseAppCount(accountId);
            return result;

        }

        public List<AppDto> GetAllApps(Int32 accountId, Int32 appTypeId, Int32 orderId, String searchText, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(accountId, true).Validate(orderId).Validate(searchText).Validate(pageIndex, true).Validate(pageSize);

            var result = _appContext.GetApps(accountId, appTypeId, orderId, searchText, pageIndex, pageSize, out totalCount);
            return result.Select(app => new AppDto
            {
                AppTypeId = app.AppTypeId,
                AccountId = app.AccountId,
                AddTime = app.AddTime.ToString("yyyy-MM-dd"),
                UseCount = app.UseCount,
                StartCount = app.StarCount,
                Name = app.Name,
                IconUrl = app.IconUrl,
                Remark = app.Remark,
                AppStyle = (Int32)app.AppStyle,
                Id = app.Id
            }).ToList();
        }

        public List<AppDto> GetAccountAllApps(Int32 accountId, String searchText, Int32 appTypeId, Int32 appStyleId, String appState, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(accountId, true).Validate(searchText).Validate(appTypeId, true).Validate(appStyleId, true).Validate(pageIndex).Validate(pageSize);

            var result = _appContext.GetAccountApps(accountId, searchText, appTypeId, appStyleId, appState, pageIndex, pageSize, out totalCount);

            var appTypes = GetAppTypes();
            return result.Select(app => new AppDto
            {
                Name = app.Name,
                AppStyle = (Int32)app.AppStyle,
                AppTypeName = appTypes.FirstOrDefault(appType => appType.Id == app.AppTypeId).Name,
                UseCount = app.UseCount,
                Id = app.Id,
                IconUrl = app.IconUrl,
                AppAuditState = (Int32)app.AppAuditState,
                IsRecommand = app.IsRecommand,
                IsCreater = app.AccountId == accountId
            }).ToList();
        }

        public AppDto GetApp(Int32 appId)
        {
            ValidateParameter.Validate(appId);

            var result = _appContext.GetApp(appId);
            var appTypes = GetAppTypes();

            return new AppDto
            {
                Name = result.Name,
                IconUrl = result.IconUrl,
                Remark = result.Remark,
                UseCount = result.UseCount,
                StartCount = result.StarCount,
                AppTypeName = appTypes.FirstOrDefault(appType => appType.Id == result.AppTypeId).Name,
                AddTime = result.AddTime.ToString("yyyy-MM-dd"),
                AccountId = result.AccountId,
                Id = result.Id,
                IsResize = result.IsResize,
                IsOpenMax = result.IsOpenMax,
                IsFlash = result.IsFlash,
                AppStyle = (Int32)result.AppStyle,
                AppUrl = result.AppUrl,
                Width = result.Width,
                Height = result.Height,
                AppAuditState = (Int32)result.AppAuditState,
                AppReleaseState = (Int32)result.AppReleaseState,
                AppTypeId = result.AppTypeId
            };
        }

        public Boolean IsInstallApp(Int32 accountId, Int32 appId)
        {
            ValidateParameter.Validate(accountId).Validate(appId);
            var result = _appContext.IsInstallApp(accountId, appId);
            return result;
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
            var result = _appContext.GetSystemApp(appIds);
            return result.Select(app => new AppDto
            {
                Id = app.Id,
                Name = app.Name,
                IconUrl = app.IconUrl
            }).ToList();
        }

        public void ModifyAppDirection(Int32 accountId, String direction)
        {
            ValidateParameter.Validate(accountId).Validate(direction);

            if (direction.ToLower() == "x")
            {
                _deskContext.ModifyMemberDirectionToX(accountId);
            }
            else if (direction.ToLower() == "y")
            {
                _deskContext.ModifyMemberDirectionToY(accountId);
            }
            else
            {
                throw new BusinessException($"未能识别的App排列方向:{direction.ToLower()}");
            }

        }

        public void ModifyAppIconSize(Int32 accountId, Int32 newSize)
        {
            ValidateParameter.Validate(accountId).Validate(newSize);
            _deskContext.ModifyMemberDisplayIconSize(accountId, newSize);
        }

        public void ModifyAppVerticalSpacing(Int32 accountId, Int32 newSize)
        {
            ValidateParameter.Validate(accountId).Validate(newSize);
            _deskContext.ModifyMemberHorizontalSpacing(accountId, newSize);
        }

        public void ModifyAppHorizontalSpacing(Int32 accountId, Int32 newSize)
        {
            ValidateParameter.Validate(accountId).Validate(newSize);
            _deskContext.ModifyMemberVerticalSpacing(accountId, newSize);
        }

        public void ModifyAppStar(Int32 accountId, Int32 appId, Int32 starCount)
        {
            ValidateParameter.Validate(accountId).Validate(appId).Validate(starCount, true);

            var isInstall = _appContext.IsInstallApp(accountId, appId);
            if (!isInstall)
            {
                throw new BusinessException("您还没有安装这个App，因此不能打分");
            }
            _appContext.ModifyAppStar(accountId, appId, starCount);
        }

        public void InstallApp(Int32 accountId, Int32 appId, Int32 deskNum)
        {
            ValidateParameter.Validate(accountId).Validate(appId).Validate(deskNum);
            _installAppServices.Install(accountId, appId, deskNum);
        }

        public void ModifyAccountAppInfo(Int32 accountId, AppDto appDto)
        {
            ValidateParameter.Validate(accountId).Validate(appDto);
            _modifyAppInfoServices.ModifyAccountAppInfo(accountId, appDto.ConvertToModel<AppDto, App>());
        }

        public void CreateNewApp(AppDto appDto)
        {
            ValidateParameter.Validate(appDto);

            var app = appDto.ConvertToModel<AppDto, App>();
            var internalApp = new App(app.Name, app.IconUrl, app.AppUrl, app.Width, app.Height, app.AppTypeId, app.AppAuditState, AppReleaseState.UnRelease, app.AppStyle, app.AccountId,
                app.Remark, app.IsMax, app.IsFull, app.IsSetbar, app.IsOpenMax, app.IsFlash, app.IsDraw, app.IsResize);
            _appContext.CreateNewApp(internalApp);

        }

        public void RemoveAppType(Int32 appTypeId)
        {
            ValidateParameter.Validate(appTypeId);
            _modifyAppInfoServices.DeleteAppType(appTypeId);
        }

        public void CreateNewAppType(AppTypeDto appTypeDto)
        {
            ValidateParameter.Validate(appTypeDto);
            var appType = appTypeDto.ConvertToModel<AppTypeDto, AppType>();
            _modifyAppInfoServices.CreateNewAppType(appType);
        }

        public void ModifyAppType(AppTypeDto appTypeDto, Int32 appTypeId)
        {
            ValidateParameter.Validate(appTypeDto).Validate(appTypeId);
            _modifyAppInfoServices.ModifyAppType(appTypeDto.Name, appTypeId);
        }

        public void Pass(Int32 appId)
        {
            ValidateParameter.Validate(appId);
            _appContext.Pass(appId);
        }

        public void Deny(Int32 appId)
        {
            ValidateParameter.Validate(appId);
            _appContext.Deny(appId);
        }

        public void SetTodayRecommandApp(Int32 appId)
        {
            ValidateParameter.Validate(appId);
            _appContext.SetTodayRecommandApp(appId);
        }

        public void RemoveApp(Int32 appId)
        {
            ValidateParameter.Validate(appId);
            _appContext.RemoveApp(appId);
        }

        public void ReleaseApp(Int32 appId)
        {
            ValidateParameter.Validate(appId);

            _appContext.ReleaseApp(appId);
        }

        public void ModifyAppIcon(Int32 accountId, Int32 appId, String newIcon)
        {
            ValidateParameter.Validate(accountId).Validate(appId).Validate(newIcon);
            _modifyAppInfoServices.ModifyAppIcon(accountId, appId, newIcon);
        }



        #region private method

        // <summary>
        // <summary> 获取传入的枚举类型的字面量的描述
        // <summary> </summary>
        // <summary> <param name="enumType"></param>
        // <summary> <returns></returns>
        private static IEnumerable<dynamic> GetEnumDescriptions(Type enumType) => enumType.GetFields().Where(field => field.CustomAttributes.Any()).Select(s => new { s.CustomAttributes.ToArray()[0].ConstructorArguments[0].Value, Id = s.GetRawConstantValue(), Type = enumType.Name }).Cast<dynamic>().ToList();
        #endregion
    }
}

