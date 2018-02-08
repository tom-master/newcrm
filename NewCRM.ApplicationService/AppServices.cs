using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Application.Services.Interface;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewLib;

namespace NewCRM.Application.Services
{
    public class AppServices : BaseServiceContext, IAppServices
    {
        private readonly IAppContext _appContext;
        private readonly IDeskContext _deskContext;

        public AppServices(IAppContext appContext, IDeskContext deskContext)
        {
            _appContext = appContext;
            _deskContext = deskContext;
        }

        public List<AppTypeDto> GetAppTypes()
        {
            var result = GetCache(CacheKey.AppTypes(), () => _appContext.GetAppTypes());
            return result.Select(s => new AppTypeDto
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
        }

        public TodayRecommendAppDto GetTodayRecommend(Int32 accountId)
        {
            Parameter.Validate(accountId);

            var result = _appContext.GetTodayRecommend(accountId);
            result.AppIcon = result.IsIconByUpload ? ProfileManager.FileUrl + result.AppIcon : result.AppIcon;
            if (result == null)
            {
                return new TodayRecommendAppDto();
            }

            return result;
        }

        public Tuple<Int32, Int32> GetAccountDevelopAppCountAndNotReleaseAppCount(Int32 accountId)
        {
            Parameter.Validate(accountId);
            return _appContext.GetAccountDevelopAppCountAndNotReleaseAppCount(accountId);
        }

        public List<AppDto> GetAllApps(Int32 accountId, Int32 appTypeId, Int32 orderId, String searchText, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            Parameter.Validate(accountId, true).Validate(orderId).Validate(searchText).Validate(pageIndex, true).Validate(pageSize);

            var result = _appContext.GetApps(accountId, appTypeId, orderId, searchText, pageIndex, pageSize, out totalCount);
            return result.Select(app => new AppDto
            {
                AppTypeId = app.AppTypeId,
                AccountId = app.AccountId,
                AddTime = app.AddTime.ToString("yyyy-MM-dd"),
                UseCount = app.UseCount,
                StartCount = app.StarCount,
                Name = app.Name,
                IconUrl = app.IsIconByUpload ? ProfileManager.FileUrl + app.IconUrl : app.IconUrl,
                Remark = app.Remark,
                AppStyle = (Int32)app.AppStyle,
                Id = app.Id,
                IsInstall = app.IsInstall
            }).ToList();
        }

        public List<AppDto> GetAccountAllApps(Int32 accountId, String searchText, Int32 appTypeId, Int32 appStyleId, String appState, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            Parameter.Validate(accountId, true).Validate(searchText).Validate(appTypeId, true).Validate(appStyleId, true).Validate(pageIndex).Validate(pageSize);

            var result = _appContext.GetAccountApps(accountId, searchText, appTypeId, appStyleId, appState, pageIndex, pageSize, out totalCount);
            var appTypes = GetAppTypes();

            return result.Select(app => new AppDto
            {
                Name = app.Name,
                AppStyle = (Int32)app.AppStyle,
                AppTypeName = appTypes.FirstOrDefault(appType => appType.Id == app.AppTypeId).Name,
                UseCount = app.UseCount,
                Id = app.Id,
                IconUrl = app.IsIconByUpload ? ProfileManager.FileUrl + app.IconUrl : app.IconUrl,
                AppAuditState = (Int32)app.AppAuditState,
                IsRecommand = app.IsRecommand,
                AccountId = app.AccountId
            }).ToList();
        }

        public AppDto GetApp(Int32 appId)
        {
            Parameter.Validate(appId);

            var result = _appContext.GetApp(appId);
            var appTypes = GetCache(CacheKey.AppTypes(), () => GetAppTypes());

            return new AppDto
            {
                Name = result.Name,
                IconUrl = result.IsIconByUpload ? ProfileManager.FileUrl + result.IconUrl : result.IconUrl,
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
                AppTypeId = result.AppTypeId,
                AccountName = result.AccountName,
                IsIconByUpload = result.IsIconByUpload
            };
        }

        public Boolean IsInstallApp(Int32 accountId, Int32 appId)
        {
            Parameter.Validate(accountId).Validate(appId);
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

        public Boolean CheckAppTypeName(String appTypeName)
        {
            Parameter.Validate(appTypeName);
            return _appContext.CheckAppTypeName(appTypeName);
        }

        public void ModifyAppDirection(Int32 accountId, String direction)
        {
            Parameter.Validate(accountId).Validate(direction);

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
            RemoveOldKeyWhenModify(CacheKey.Config(accountId));
        }

        public void ModifyAppIconSize(Int32 accountId, Int32 newSize)
        {
            Parameter.Validate(accountId).Validate(newSize);
            _deskContext.ModifyMemberDisplayIconSize(accountId, newSize);
            RemoveOldKeyWhenModify(CacheKey.Config(accountId));
        }

        public void ModifyAppVerticalSpacing(Int32 accountId, Int32 newSize)
        {
            Parameter.Validate(accountId).Validate(newSize);
            _deskContext.ModifyMemberHorizontalSpacing(accountId, newSize);
            RemoveOldKeyWhenModify(CacheKey.Config(accountId));
        }

        public void ModifyAppHorizontalSpacing(Int32 accountId, Int32 newSize)
        {
            Parameter.Validate(accountId).Validate(newSize);
            _deskContext.ModifyMemberVerticalSpacing(accountId, newSize);
            RemoveOldKeyWhenModify(CacheKey.Config(accountId));
        }

        public void ModifyAppStar(Int32 accountId, Int32 appId, Int32 starCount)
        {
            Parameter.Validate(accountId).Validate(appId).Validate(starCount, true);

            var isInstall = _appContext.IsInstallApp(accountId, appId);
            if (!isInstall)
            {
                throw new BusinessException("您还没有安装这个App，因此不能打分");
            }
            _appContext.ModifyAppStar(accountId, appId, starCount);
        }

        public void InstallApp(Int32 accountId, Int32 appId, Int32 deskNum)
        {
            Parameter.Validate(accountId).Validate(appId).Validate(deskNum);
            _appContext.Install(accountId, appId, deskNum);
            RemoveOldKeyWhenModify(CacheKey.Desktop(accountId));
        }

        public void ModifyAccountAppInfo(Int32 accountId, AppDto appDto)
        {
            Parameter.Validate(accountId).Validate(appDto);
            _appContext.ModifyAccountAppInfo(accountId, appDto.ConvertToModel<AppDto, App>());
        }

        public void CreateNewApp(AppDto appDto)
        {
            Parameter.Validate(appDto);

            var app = appDto.ConvertToModel<AppDto, App>();
            var internalApp = new App(app.Name, app.IconUrl, app.AppUrl, app.Width, app.Height, app.AppTypeId, app.AppAuditState, AppReleaseState.UnRelease, app.AppStyle, app.AccountId,
                app.Remark, app.IsMax, app.IsFull, app.IsSetbar, app.IsOpenMax, app.IsFlash, app.IsDraw, app.IsResize);
            internalApp.IsIconByUpload = appDto.IsIconByUpload;
            _appContext.CreateNewApp(internalApp);

        }

        public void RemoveAppType(Int32 appTypeId)
        {
            Parameter.Validate(appTypeId);
            _appContext.DeleteAppType(appTypeId);
            RemoveOldKeyWhenModify(CacheKey.AppTypes());
        }

        public void CreateNewAppType(AppTypeDto appTypeDto)
        {
            Parameter.Validate(appTypeDto);
            var appType = appTypeDto.ConvertToModel<AppTypeDto, AppType>();
            _appContext.CreateNewAppType(appType);
            RemoveOldKeyWhenModify(CacheKey.AppTypes());
        }

        public void ModifyAppType(AppTypeDto appTypeDto, Int32 appTypeId)
        {
            Parameter.Validate(appTypeDto).Validate(appTypeId);
            _appContext.ModifyAppType(appTypeDto.Name, appTypeId);
            RemoveOldKeyWhenModify(CacheKey.AppTypes());
        }

        public void Pass(Int32 appId)
        {
            Parameter.Validate(appId);
            _appContext.Pass(appId);
        }

        public void Deny(Int32 appId)
        {
            Parameter.Validate(appId);
            _appContext.Deny(appId);
        }

        public void SetTodayRecommandApp(Int32 appId)
        {
            Parameter.Validate(appId);
            _appContext.SetTodayRecommandApp(appId);
        }

        public void RemoveApp(Int32 appId)
        {
            Parameter.Validate(appId);
            _appContext.RemoveApp(appId);
        }

        public void ReleaseApp(Int32 appId)
        {
            Parameter.Validate(appId);
            _appContext.ReleaseApp(appId);
        }

        public void ModifyAppIcon(Int32 accountId, Int32 appId, String newIcon)
        {
            Parameter.Validate(accountId).Validate(appId).Validate(newIcon);
            _appContext.ModifyAppIcon(accountId, appId, newIcon);
        }

        #region private method
        // <summary>
        // <summary> 获取传入的枚举类型的字面量的描述
        private static IEnumerable<dynamic> GetEnumDescriptions(Type enumType) => enumType.GetFields().Where(field => field.CustomAttributes.Any()).Select(s => new { s.CustomAttributes.ToArray()[0].ConstructorArguments[0].Value, Id = s.GetRawConstantValue(), Type = enumType.Name }).Cast<dynamic>().ToList();

        #endregion
    }
}

