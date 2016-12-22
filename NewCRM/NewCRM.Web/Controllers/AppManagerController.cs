using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewCRM.Dto.Dto;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    [Export]
    public class AppManagerController : BaseController
    {

        #region 页面

        // GET: AppManage
        public ActionResult Index()
        {
            ViewData["AppTypes"] = AppApplicationServices.GetAppTypes();

            ViewData["AppStyles"] = AppApplicationServices.GetAllAppStyles().ToList();

            ViewData["AppStates"] = AppApplicationServices.GetAllAppStates().Where(w => w.Name == "未审核" || w.Name == "已发布").ToList();

            return View();
        }


        public ActionResult AppAudit(Int32 appId)
        {
            AppDto appResult = null;
            if (appId != 0)// 如果appId为0则是新创建app
            {
                appResult = AppApplicationServices.GetApp(appId);

                ViewData["AppState"] = appResult.AppAuditState;
            }

            ViewData["AppTypes"] = AppApplicationServices.GetAppTypes();

            return View(appResult);
        }

        #endregion

        /// <summary>
        /// 获取所有的app
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="appTypeId"></param>
        /// <param name="appStyleId"></param>
        /// <param name="appState"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult GetAllApps(String searchText, Int32 appTypeId, Int32 appStyleId, String appState, Int32 pageIndex, Int32 pageSize)
        {
            Int32 totalCount;

            var appResults = AppApplicationServices.GetAccountAllApps(0, searchText, appTypeId, appStyleId, appState, pageIndex, pageSize, out totalCount);

            return Json(new
            {
                apps = appResults,
                totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// app审核通过
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public ActionResult Pass(Int32 appId)
        {
            AppApplicationServices.Pass(appId);

            return Json(
                new
                {
                    success = 1
                });
        }

        /// <summary>
        /// app审核不通过
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public ActionResult Deny(Int32 appId)
        {
            AppApplicationServices.Deny(appId);

            return Json(
                new
                {
                    success = 1
                });
        }

        /// <summary>
        /// 设置app为今日推荐
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public ActionResult Recommend(Int32 appId)
        {
            AppApplicationServices.SetTodayRecommandApp(appId);

            return Json(
                new
                {
                    success = 1
                });
        }

        /// <summary>
        /// 删除app
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public ActionResult DeleteApp(Int32 appId)
        {
            AppApplicationServices.RemoveApp(appId);

            return Json(new { sucess = 1 });
        }
    }
}