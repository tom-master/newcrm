using System;
using System.Collections.Generic;
using NewCRM.Dto;
namespace NewCRM.Application.Services.Interface
{
    public interface IAppServices
    {
        #region have return value

        /// <summary>
        /// 获取桌面的成员
        /// </summary>
        /// <returns></returns>
        IDictionary<String, IList<dynamic>> GetDeskMembers(Int32 accountId);

        /// <summary>
        /// 获取所有的app类型
        /// </summary>
        /// <returns></returns>
        List<AppTypeDto> GetAppTypes();

        /// <summary>
        /// 获取今日推荐
        /// </summary>
        /// <returns></returns>
        TodayRecommendAppDto GetTodayRecommend(Int32 accountId);

        /// <summary>
        /// 获取用户开发的app和未发布的app
        /// </summary>
        /// <returns></returns>
        Tuple<Int32, Int32> GetAccountDevelopAppCountAndNotReleaseAppCount(Int32 accountId);

        /// <summary>
        /// 获取所有的app
        /// </summary>
        /// <returns></returns>
        List<AppDto> GetAllApps(Int32 accountId, Int32 appTypeId, Int32 orderId, String searchText, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);

        /// <summary>
        /// 获取用户的app
        /// </summary>
        /// <returns></returns>
        List<AppDto> GetAccountAllApps(Int32 accountId, String searchText, Int32 appTypeId, Int32 appStyleId, String appState, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);

        /// <summary>
        /// 根据appId获取App
        /// </summary>
        /// <returns></returns>
        AppDto GetApp(Int32 appId);

        /// <summary>
        /// 当前用户是否安装了这个app
        /// </summary>
        /// <returns></returns>
        Boolean IsInstallApp(Int32 accountId, Int32 appId);

        /// <summary>
        /// 获取所有的app样式
        /// </summary>
        /// <returns></returns>
        IEnumerable<AppStyleDto> GetAllAppStyles();

        /// <summary>
        /// 获取所有的app状态
        /// </summary>
        /// <returns></returns>
        IEnumerable<AppStateDto> GetAllAppStates();

        /// <summary>
        /// 获取系统app
        /// </summary>
        /// <returns></returns>
        List<AppDto> GetSystemApp(IEnumerable<Int32> appIds = default(IEnumerable<Int32>));

        #endregion

        #region not have return value

        /// <summary>
        /// 修改开发者（用户）的app信息
        /// </summary>
        void ModifyAccountAppInfo(Int32 accountId, AppDto appDto);

        /// <summary>
        /// 开发者（用户）创建新的app
        /// </summary>
        void CreateNewApp(AppDto app);

        /// <summary>
        /// 删除指定的应用类型
        /// </summary>
        void RemoveAppType(Int32 appTypeId);

        /// <summary>
        /// 创建新的app类型
        /// </summary>
        void CreateNewAppType(AppTypeDto appTypeDto);

        /// <summary>
        /// 修改app类型
        /// </summary>
        void ModifyAppType(AppTypeDto appTypeDto, Int32 appTypeId);

        /// <summary>
        /// app审核通过
        /// </summary>
        void Pass(Int32 appId);

        /// <summary>
        /// app审核不通过
        /// </summary>
        void Deny(Int32 appId);

        /// <summary>
        /// 设置今日推荐app
        /// </summary>
        void SetTodayRecommandApp(Int32 appId);

        /// <summary>
        /// 移除app
        /// </summary>
        void RemoveApp(Int32 appId);

        /// <summary>
        /// 发布应用
        /// </summary>
        void ReleaseApp(Int32 appId);

        /// <summary>
        /// app打分
        /// </summary>
        void ModifyAppStar(Int32 accountId, Int32 appId, Int32 starCount);

        /// <summary>
        /// 安装app
        /// </summary>
        void InstallApp(Int32 accountId, Int32 appId, Int32 deskNum);

        /// <summary>
        /// 修改app排列方向
        /// </summary>
        void ModifyAppDirection(Int32 accountId, String direction);

        /// <summary>
        /// 修改app图标大小
        /// </summary>
        void ModifyAppIconSize(Int32 accountId, Int32 newSize);

        /// <summary>
        /// 修改app垂直间距
        /// </summary>
        void ModifyAppVerticalSpacing(Int32 accountId, Int32 newSize);

        /// <summary>
        /// 修改app水平间距
        /// </summary>
        void ModifyAppHorizontalSpacing(Int32 accountId, Int32 newSize);

        /// <summary>
        /// 修改app图标
        /// </summary>
        void ModifyAppIcon(Int32 accountId, Int32 appId, String newIcon);

        #endregion
    }
}
