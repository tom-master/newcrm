using System;
using System.Collections.Generic;
using NewCRM.Dto.Dto;

namespace NewCRM.Application.Interface
{
    public interface IAppApplicationServices
    {
        #region have return value

        /// <summary>
        /// 获取桌面的成员
        /// </summary>
        /// <returns></returns>
        IDictionary<String, IList<dynamic>> GetDeskMembers(Int32 deskId);

        /// <summary>
        /// 获取所有的app类型
        /// </summary>
        /// <returns></returns>
        List<AppTypeDto> GetAppTypes();

        /// <summary>
        /// 获取今日推荐
        /// </summary>
        /// <returns></returns>
        TodayRecommendAppDto GetTodayRecommend();

        /// <summary>
        /// 获取用户开发的app和未发布的app
        /// </summary>
        /// <returns></returns>
        Tuple<Int32, Int32> GetAccountDevelopAppCountAndNotReleaseAppCount();

        /// <summary>
        /// 获取所有的app
        /// </summary>
        /// <param name="appTypeId"></param>
        /// <param name="orderId"></param>
        /// <param name="searchText"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<AppDto> GetAllApps(Int32 appTypeId, Int32 orderId, String searchText, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);

        /// <summary>
        /// 根据appId获取App
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        AppDto GetApp(Int32 appId);

        /// <summary>
        /// 当前用户是否安装了这个app
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        Boolean IsInstallApp(Int32 appId);

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
        /// 获取开发者（用户）的app
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="appTypeId"></param>
        /// <param name="appStyleId"></param>
        /// <param name="appState"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<AppDto> GetAccountAllApps(
             String searchText, Int32 appTypeId, Int32 appStyleId, String appState, Int32 pageIndex,
            Int32 pageSize,
            out Int32 totalCount);

        /// <summary>
        /// 获取系统app
        /// </summary>
        /// <returns></returns>
        List<AppDto> GetSystemApp(IEnumerable<Int32> appIds);

        List<AppDto> GetSystemApp();

        #endregion

        #region not have return value

        /// <summary>
        /// 修改开发者（用户）的app信息
        /// </summary>

        /// <param name="appDto"></param>
        void ModifyAccountAppInfo(AppDto appDto);

        /// <summary>
        /// 开发者（用户）创建新的app
        /// </summary>
        /// <param name="app"></param>
        void CreateNewApp(AppDto app);

        /// <summary>
        /// 删除指定的应用类型
        /// </summary>
        /// <param name="appTypeId"></param>
        void RemoveAppType(Int32 appTypeId);

        /// <summary>
        /// 创建新的app类型
        /// </summary>
        /// <param name="appTypeDto"></param>
        void CreateNewAppType(AppTypeDto appTypeDto);

        /// <summary>
        /// 修改app类型
        /// </summary>
        /// <param name="appTypeDto"></param>
        /// <param name="appTypeId"></param>
        void ModifyAppType(AppTypeDto appTypeDto, Int32 appTypeId);

        /// <summary>
        /// app审核通过
        /// </summary>
        /// <param name="appId"></param>
        void Pass(Int32 appId);

        /// <summary>
        /// app审核不通过
        /// </summary>
        /// <param name="appId"></param>
        void Deny(Int32 appId);

        /// <summary>
        /// 设置今日推荐app
        /// </summary>
        /// <param name="appId"></param>
        void SetTodayRecommandApp(Int32 appId);

        /// <summary>
        /// 移除app
        /// </summary>
        /// <param name="appId"></param>
        void RemoveApp(Int32 appId);

        /// <summary>
        /// 发布应用
        /// </summary>
        /// <param name="appId"></param>
        void ReleaseApp(Int32 appId);

        /// <summary>
        /// app打分
        /// </summary>

        /// <param name="appId"></param>
        /// <param name="starCount"></param>
        void ModifyAppStar(Int32 appId, Int32 starCount);

        /// <summary>
        /// 安装app
        /// </summary>

        /// <param name="appId"></param>
        /// <param name="deskNum"></param>
        void InstallApp(Int32 appId, Int32 deskNum);

        /// <summary>
        /// 修改app排列方向
        /// </summary>

        /// <param name="direction"></param>
        void ModifyAppDirection(String direction);

        /// <summary>
        /// 修改app图标大小
        /// </summary>

        /// <param name="newSize"></param>
        void ModifyAppIconSize(Int32 newSize);

        /// <summary>
        /// 修改app垂直间距
        /// </summary>

        /// <param name="newSize"></param>
        void ModifyAppVerticalSpacing(Int32 newSize);

        /// <summary>
        /// 修改app水平间距
        /// </summary>

        /// <param name="newSize"></param>
        void ModifyAppHorizontalSpacing(Int32 newSize);

        #endregion
    }
}
