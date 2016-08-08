﻿using System;
using System.Collections.Generic;
using NewCRM.Dto.Dto;

namespace NewCRM.Application.Services.IApplicationService
{
    public interface IAppApplicationServices
    {
        #region app
        /// <summary>
        /// 获取用户桌面的成员
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IDictionary<Int32, IList<dynamic>> GetUserDeskMembers(Int32 userId);

        /// <summary>
        /// 修改app排列方向
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="direction"></param>
        void ModifyAppDirection(Int32 userId, String direction);

        /// <summary>
        /// 修改app图标大小
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newSize"></param>
        void ModifyAppIconSize(Int32 userId, Int32 newSize);

        /// <summary>
        /// 修改app垂直间距
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newSize"></param>
        void ModifyAppVerticalSpacing(Int32 userId, Int32 newSize);

        /// <summary>
        /// 修改app水平间距
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newSize"></param>
        void ModifyAppHorizontalSpacing(Int32 userId, Int32 newSize);

        /// <summary>
        /// 获取所有的app类型
        /// </summary>
        /// <returns></returns>
        List<AppTypeDto> GetAppTypes();

        /// <summary>
        /// 获取今日推荐
        /// </summary>
        /// <returns></returns>
        TodayRecommendAppDto GetTodayRecommend(Int32 userId);

        /// <summary>
        /// 获取用户开发的app和未发布的app
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Tuple<Int32, Int32> GetUserDevAppAndUnReleaseApp(Int32 userId);

        /// <summary>
        /// 获取所有的app
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appTypeId"></param>
        /// <param name="orderId"></param>
        /// <param name="searchText"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<AppDto> GetAllApps(Int32 userId, Int32 appTypeId, Int32 orderId, String searchText, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);

        /// <summary>
        /// 根据appId获取App
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        AppDto GetApp(Int32 appId);

        /// <summary>
        /// app打分
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appId"></param>
        /// <param name="starCount"></param>
        void ModifyAppStar(Int32 userId, Int32 appId, Int32 starCount);

        /// <summary>
        /// 安装app
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appId"></param>
        /// <param name="deskNum"></param>
        void InstallApp(Int32 userId, Int32 appId, Int32 deskNum);

        /// <summary>
        /// 当前用户是否安装了这个app
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        Boolean IsInstallApp(Int32 userId, Int32 appId);

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
        /// <param name="userId"></param>
        /// <param name="appName"></param>
        /// <param name="appTypeId"></param>
        /// <param name="appStyleId"></param>
        /// <param name="appState"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>

        List<AppDto> GetUserAllApps(
            Int32 userId, String appName, Int32 appTypeId, Int32 appStyleId, String appState, Int32 pageIndex,
            Int32 pageSize,
            out Int32 totalCount);

        /// <summary>
        /// 修改开发者（用户）的app信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appDto"></param>
        void ModifyUserAppInfo(Int32 userId, AppDto appDto);

        #endregion
    }
}
