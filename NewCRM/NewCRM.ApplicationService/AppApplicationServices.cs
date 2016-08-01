using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Domain.Entities.DomainModel.System;
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


        public IDictionary<Int32, IList<dynamic>> GetUserApp(Int32 userId)
        {
            _validateParameter.Validate(userId);
            return _appServices.GetApp(userId);
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

        public List<AppDto> GetAllApps(
            Int32 userId, Int32 appTypeId, Int32 orderId, String searchText, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            _validateParameter.Validate(userId, true).Validate(appTypeId, true).Validate(orderId).Validate(searchText).Validate(pageIndex,true).Validate(pageSize);
            return _appServices.GetAllApps(userId, appTypeId, orderId, searchText, pageIndex, pageSize, out totalCount).ConvertToDto<App, AppDto>().ToList();
        }


    }
}
