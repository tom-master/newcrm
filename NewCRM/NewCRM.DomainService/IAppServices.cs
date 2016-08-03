using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Services.Impl;

namespace NewCRM.Domain.Services
{
    public interface IAppServices
    {
        /// <summary>
        /// 获取用户桌面的成员
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IDictionary<Int32, IList<dynamic>> GetUserDeskMembers(Int32 userId);

        /// <summary>
        /// 修改app的排列方向
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="direction"></param>
        void ModifyAppDirection(Int32 userId, String direction);

        /// <summary>
        /// 修改app图标的大小
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newSize"></param>
        void ModifyAppIconSize(Int32 userId, Int32 newSize);

        /// <summary>
        /// 修改app垂直距离
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newSize"></param>
        void ModifyAppVerticalSpacing(Int32 userId, Int32 newSize);

        /// <summary>
        /// 修改app水平距离
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newSize"></param>
        void ModifyAppHorizontalSpacing(Int32 userId, Int32 newSize);

        /// <summary>
        /// 获取应用的类型
        /// </summary>
        /// <returns></returns>
        List<AppType> GetAppTypes();

        /// <summary>
        /// 获取今日推荐
        /// </summary>
        /// <returns></returns>
        TodayRecommendAppModel GetTodayRecommend(Int32 userId);

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
        List<App> GetAllApps(Int32 userId, Int32 appTypeId, Int32 orderId, String searchText, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);

        /// <summary>
        /// 获取app
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        App GetApp(Int32 appId);

        /// <summary>
        /// app打分
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appId"></param>
        /// <param name="starCount"></param>
        void ModifyAppStar(Int32 userId, Int32 appId, Int32 starCount);

        /// <summary>
        /// 用户安装app
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

    }
}
