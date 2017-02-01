using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Mvc;
using NewCRM.Application.Interface;
using NewCRM.Dto.Dto;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class AppManagerController : BaseController
    {

        private readonly IAppApplicationServices _appApplicationServices;

        [ImportingConstructor]
        public AppManagerController(IAppApplicationServices appApplicationServices)
        {
            _appApplicationServices = appApplicationServices;
        }


        #region 页面

        // GET: AppManage
        public ActionResult Index()
        {
            ViewData["AppTypes"] = _appApplicationServices.GetAppTypes();

            ViewData["AppStyles"] = _appApplicationServices.GetAllAppStyles().ToList();

            ViewData["AppStates"] = _appApplicationServices.GetAllAppStates().Where(w => w.Name == "未审核" || w.Name == "已发布").ToList();

            return View();
        }


        public ActionResult AppAudit(Int32 appId)
        {
            AppDto appResult = null;
            if (appId != 0)// 如果appId为0则是新创建app
            {
                appResult = _appApplicationServices.GetApp(appId);

                ViewData["AppState"] = appResult.AppAuditState;
            }

            ViewData["AppTypes"] = _appApplicationServices.GetAppTypes();

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

            var appResults = _appApplicationServices.GetAccountAllApps(0,searchText, appTypeId, appStyleId, appState, pageIndex, pageSize, out totalCount);

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
            _appApplicationServices.Pass(appId);

            return Json(new
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
            _appApplicationServices.Deny(appId);

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
            _appApplicationServices.SetTodayRecommandApp(appId);

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
            _appApplicationServices.RemoveApp(appId);

            return Json(new { sucess = 1 });
        }
    }
}