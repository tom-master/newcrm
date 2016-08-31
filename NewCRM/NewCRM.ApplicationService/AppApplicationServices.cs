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

        public IDictionary<Int32, IList<dynamic>> GetAccountDeskMembers(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            return AppServices.GetAccountDeskMembers(accountId);
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
            return AppServices.GetAppTypes().ConvertToDtos<AppType, AppTypeDto>().ToList();
        }

        public TodayRecommendAppDto GetTodayRecommend(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            return DtoConfiguration.ConvertDynamicToDto<TodayRecommendAppDto>(AppServices.GetTodayRecommend(accountId));
        }

        public Tuple<Int32, Int32> GetAccountDevelopAppCountAndNotReleaseAppCount(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);
            return AppServices.GetAccountDevelopAppCountAndNotReleaseAppCount(accountId);
        }

        public List<AppDto> GetAllApps(Int32 accountId, Int32 appTypeId, Int32 orderId, String searchText, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(accountId, true).Validate(orderId).Validate(searchText).Validate(pageIndex, true).Validate(pageSize);

            var appDtoResult = AppServices.GetAllApps(accountId, appTypeId, orderId, searchText, pageIndex, pageSize, out totalCount).ConvertDynamicToDtos<AppDto>().ToList();

            appDtoResult.ForEach(appDto => appDto.IsInstall = IsInstallApp(accountId, appDto.Id));

            return appDtoResult;
        }

        public AppDto GetApp(Int32 appId)
        {
            ValidateParameter.Validate(appId);

            return DtoConfiguration.ConvertDynamicToDto<AppDto>(AppServices.GetApp(appId));
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
            return AppServices.IsInstallApp(accountId, appId);
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

            return AppServices.GetAccountAllApps(accountId, appName, appTypeId, appStyleId, appState, pageIndex, pageSize, out totalCount).ConvertDynamicToDtos<AppDto>().ToList();
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
