using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Schema;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Domain.Services;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustomHelper;

namespace NewCRM.Application.Services
{
    [Export(typeof(IAppApplicationServices))]
    internal class AppApplicationServices : IAppApplicationServices
    {
        private readonly Parameter _validateParameter = new Parameter();

        [Import]
        private IAppServices _appServices;

        public IDictionary<Int32, IList<dynamic>> GetUserDeskMembers(Int32 userId)
        {
            _validateParameter.Validate(userId);

            return _appServices.GetUserDeskMembers(userId);
        }

        public void ModifyAppDirection(Int32 userId, String direction)
        {
            _validateParameter.Validate(userId).Validate(direction);
            _appServices.ModifyAppDirection(userId, direction);
        }

        public void ModifyAppIconSize(Int32 userId, Int32 newSize)
        {
            _validateParameter.Validate(userId).Validate(newSize);
            _appServices.ModifyAppIconSize(userId, newSize);
        }

        public void ModifyAppVerticalSpacing(Int32 userId, Int32 newSize)
        {
            _validateParameter.Validate(userId).Validate(newSize);
            _appServices.ModifyAppVerticalSpacing(userId, newSize);
        }

        public void ModifyAppHorizontalSpacing(Int32 userId, Int32 newSize)
        {
            _validateParameter.Validate(userId).Validate(newSize);
            _appServices.ModifyAppHorizontalSpacing(userId, newSize);
        }

        public List<AppTypeDto> GetAppTypes()
        {
            return _appServices.GetAppTypes().ConvertToDto<AppType, AppTypeDto>().ToList();
        }

        public TodayRecommendAppDto GetTodayRecommend(Int32 userId)
        {
            _validateParameter.Validate(userId);

            var todayRecommendAppResult = _appServices.GetTodayRecommend(userId);

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
            _validateParameter.Validate(userId);
            return _appServices.GetUserDevAppAndUnReleaseApp(userId);
        }

        public List<AppDto> GetAllApps(Int32 userId, Int32 appTypeId, Int32 orderId, String searchText, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            _validateParameter.Validate(userId, true).Validate(orderId).Validate(searchText).Validate(pageIndex, true).Validate(pageSize);

            var appDtoResult = _appServices.GetAllApps(userId, appTypeId, orderId, searchText, pageIndex, pageSize, out totalCount).ConvertDynamicToDtos<AppDto>();

            appDtoResult.ForEach(appDto => appDto.IsInstall = IsInstallApp(userId, appDto.Id));

            return appDtoResult;
        }

        public AppDto GetApp(Int32 appId)
        {
            _validateParameter.Validate(appId);

            return DtoConfiguration.ConvertDynamicToDto<AppDto>(_appServices.GetApp(appId));
        }

        public void ModifyAppStar(Int32 userId, Int32 appId, Int32 starCount)
        {
            _validateParameter.Validate(userId).Validate(appId).Validate(starCount, true);

            _appServices.ModifyAppStar(userId, appId, starCount);
        }

        public void InstallApp(Int32 userId, Int32 appId, Int32 deskNum)
        {
            _validateParameter.Validate(userId).Validate(appId).Validate(deskNum);
            _appServices.InstallApp(userId, appId, deskNum);
        }

        public Boolean IsInstallApp(Int32 userId, Int32 appId)
        {
            _validateParameter.Validate(userId).Validate(appId);
            return _appServices.IsInstallApp(userId, appId);
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
            _validateParameter.Validate(userId).Validate(appName).Validate(appTypeId, true).Validate(appStyleId, true).Validate(pageIndex).Validate(pageSize);

            return _appServices.GetUserAllApps(userId, appName, appTypeId, appStyleId, appState, pageIndex, pageSize, out totalCount).ConvertDynamicToDtos<AppDto>();

        }

        public void ModifyUserAppInfo(Int32 userId, AppDto appDto)
        {
            _validateParameter.Validate(userId).Validate(appDto);

            _appServices.ModifyUserAppInfo(userId, appDto.ConvertToModel<AppDto, App>());
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
