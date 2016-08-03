using System;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    [Export]
    public class AppController : BaseController
    {
        [Import]
        private IAppApplicationServices _appApplicationServices;

        /// <summary>
        /// 应用市场
        /// </summary>
        /// <returns></returns>
        public ActionResult AppMarket()
        {
            ViewData["AppTypes"] = _appApplicationServices.GetAppTypes();

            ViewData["TodayRecommendApp"] = _appApplicationServices.GetTodayRecommend(CurrentUser.Id);

            ViewData["UserName"] = CurrentUser.Name;

            ViewData["UserApp"] = _appApplicationServices.GetUserDevAppAndUnReleaseApp(CurrentUser.Id);

            return View();
        }

        /// <summary>
        /// app详情
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public ActionResult AppDetail(Int32 appId)
        {
            ViewData["IsInstallApp"] = _appApplicationServices.IsInstallApp(CurrentUser.Id, appId);

            ViewData["UserName"] = CurrentUser.Name;

            var singleAppResult = _appApplicationServices.GetApp(appId);
            return View(singleAppResult);
        }

        /// <summary>
        /// 获取所有的app
        /// </summary>
        /// <param name="appTypeId"></param>
        /// <param name="orderId"></param>
        /// <param name="searchText"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult GetAllApps(Int32 appTypeId, Int32 orderId, String searchText, Int32 pageIndex, Int32 pageSize)
        {
            Int32 totalCount = 0;

            var appResults = _appApplicationServices.GetAllApps(CurrentUser.Id, appTypeId, orderId, searchText, pageIndex, pageSize, out totalCount);

            return Json(new
            {
                apps = appResults,
                totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 给app打分
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="starCount"></param>
        /// <returns></returns>
        public ActionResult ModifyAppStart(Int32 appId, Int32 starCount)
        {
            _appApplicationServices.ModifyAppStar(CurrentUser.Id, appId, starCount);
            return Json(new { success = 1 });
        }

        /// <summary>
        /// 安装app
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="deskNum"></param>
        /// <returns></returns>
        public ActionResult InstallApp(Int32 appId, Int32 deskNum)
        {
            _appApplicationServices.InstallApp(CurrentUser.Id, appId, deskNum);
            return Json(new { success = 1 });
        }
    }
}