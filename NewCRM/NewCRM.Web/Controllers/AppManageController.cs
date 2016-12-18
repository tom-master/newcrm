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
    public class AppManageController : BaseController
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

        #endregion


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

        public ActionResult Pass(Int32 appId)
        {
            AppApplicationServices.Pass(appId);

            return Json(
                new
                {
                    success = 1
                });
        }

        public ActionResult Deny(Int32 appId)
        {
            AppApplicationServices.Deny(appId);

            return Json(
                new
                {
                    success = 1
                });
        }

        public ActionResult Recommend(Int32 appId)
        {
            AppApplicationServices.SetTodayRecommandApp(appId);

            return Json(
                new
                {
                    success = 1
                });
        }

        public ActionResult DeleteApp(Int32 appId)
        {
            AppApplicationServices.RemoveApp(appId);

            return Json(new { sucess = 1 });
        }
    }
}