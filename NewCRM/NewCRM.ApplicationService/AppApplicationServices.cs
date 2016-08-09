using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Dto;
using NewCRM.Dto.Dto;

namespace NewCRM.Application.Services
{
    [Export(typeof(IAppApplicationServices))]
    internal class AppApplicationServices : BaseApplicationServices, IAppApplicationServices
    {
      
        public IDictionary<Int32, IList<dynamic>> GetUserDeskMembers(Int32 userId)
        {
            ValidateParameter.Validate(userId);

            return AppServices.GetUserDeskMembers(userId);
        }

        public void ModifyAppDirection(Int32 userId, String direction)
        {
            ValidateParameter.Validate(userId).Validate(direction);
            AppServices.ModifyAppDirection(userId, direction);
        }

        public void ModifyAppIconSize(Int32 userId, Int32 newSize)
        {
            ValidateParameter.Validate(userId).Validate(newSize);
            AppServices.ModifyAppIconSize(userId, newSize);
        }

        public void ModifyAppVerticalSpacing(Int32 userId, Int32 newSize)
        {
            ValidateParameter.Validate(userId).Validate(newSize);
            AppServices.ModifyAppVerticalSpacing(userId, newSize);
        }

        public void ModifyAppHorizontalSpacing(Int32 userId, Int32 newSize)
        {
            ValidateParameter.Validate(userId).Validate(newSize);
            AppServices.ModifyAppHorizontalSpacing(userId, newSize);
        }

        public List<AppTypeDto> GetAppTypes()
        {
            return AppServices.GetAppTypes().ConvertToDto<AppType, AppTypeDto>().ToList();
        }

        public TodayRecommendAppDto GetTodayRecommend(Int32 userId)
        {
            ValidateParameter.Validate(userId);

            var todayRecommendAppResult = AppServices.GetTodayRecommend(userId);

            return new TodayRecommendAppDto
            {
                AppIcon = todayRecommendAppResult.AppIcon,
                Id = todayRecommendAppResult.AppId,
                IsInstall = todayRecommendAppResult.IsInstall,
                Name = todayRecommendAppResult.Name,
                Remark = todayRecommendAppResult.Remark,
                StartCount = todayRecommendAppResult.StartCount,
                UserCount = todayRecommendAppResult.UserCount,
                Style = todayRecommendAppResult.Style
            };
        }

        public Tuple<Int32, Int32> GetUserDevAppAndUnReleaseApp(Int32 userId)
        {
            ValidateParameter.Validate(userId);
            return AppServices.GetUserDevAppAndUnReleaseApp(userId);
        }

        public List<AppDto> GetAllApps(Int32 userId, Int32 appTypeId, Int32 orderId, String searchText, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(userId, true).Validate(orderId).Validate(searchText).Validate(pageIndex, true).Validate(pageSize);

            var appDtoResult = AppServices.GetAllApps(userId, appTypeId, orderId, searchText, pageIndex, pageSize, out totalCount).ConvertDynamicToDtos<AppDto>();

            appDtoResult.ForEach(appDto => appDto.IsInstall = IsInstallApp(userId, appDto.Id));

            return appDtoResult;
        }

        public AppDto GetApp(Int32 appId)
        {
            ValidateParameter.Validate(appId);

            return DtoConfiguration.ConvertDynamicToDto<AppDto>(AppServices.GetApp(appId));
        }

        public void ModifyAppStar(Int32 userId, Int32 appId, Int32 starCount)
        {
            ValidateParameter.Validate(userId).Validate(appId).Validate(starCount, true);

            AppServices.ModifyAppStar(userId, appId, starCount);
        }

        public void InstallApp(Int32 userId, Int32 appId, Int32 deskNum)
        {
            ValidateParameter.Validate(userId).Validate(appId).Validate(deskNum);
            AppServices.InstallApp(userId, appId, deskNum);
        }

        public Boolean IsInstallApp(Int32 userId, Int32 appId)
        {
            ValidateParameter.Validate(userId).Validate(appId);
            return AppServices.IsInstallApp(userId, appId);
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

        public List<AppDto> GetUserAllApps(
            Int32 userId, String appName, Int32 appTypeId, Int32 appStyleId, String appState, Int32 pageIndex, Int32 pageSize,
            out Int32 totalCount)
        {
            ValidateParameter.Validate(userId).Validate(appName).Validate(appTypeId, true).Validate(appStyleId, true).Validate(pageIndex).Validate(pageSize);

            return AppServices.GetUserAllApps(userId, appName, appTypeId, appStyleId, appState, pageIndex, pageSize, out totalCount).ConvertDynamicToDtos<AppDto>();

        }

        public void ModifyUserAppInfo(Int32 userId, AppDto appDto)
        {
            ValidateParameter.Validate(userId).Validate(appDto);

            AppServices.ModifyUserAppInfo(userId, appDto.ConvertToModel<AppDto, App>());
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
