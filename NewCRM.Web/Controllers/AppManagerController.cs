using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NewCRM.Application.Services.Interface;
using NewCRM.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    public class AppManagerController : BaseController
    {
        private readonly IAppServices _appServices;

        public AppManagerController(IAppServices appServices) => _appServices = appServices;

        #region 页面

        /// <summary>
        /// 首页
        /// </summary>
        [HttpGet]
        public ActionResult Index()
        {
            ViewData["AppTypes"] = _appServices.GetAppTypes();
            ViewData["AppStyles"] = _appServices.GetAllAppStyles().ToList();
            ViewData["AppStates"] = _appServices.GetAllAppStates().Where(w => w.Name == "未审核" || w.Name == "已发布").ToList();

            return View();
        }

        /// <summary>
        /// app审核
        /// </summary>
        [HttpGet]
        public ActionResult AppAudit(Int32 appId)
        {
            AppDto appResult = null;
            if (appId != 0)// 如果appId为0则是新创建app
            {
                appResult = _appServices.GetApp(appId);
                ViewData["AppState"] = appResult.AppAuditState;
            }

            ViewData["AppTypes"] = _appServices.GetAppTypes();

            return View(appResult);
        }

        #endregion

        /// <summary>
        /// 获取所有的app
        /// </summary>
        [HttpGet]
        public ActionResult GetApps(String searchText, Int32 appTypeId, Int32 appStyleId, String appState, Int32 pageIndex, Int32 pageSize)
        {
            var response = new ResponseModels<IList<AppDto>>();
            var result = _appServices.GetAccountAllApps(0, searchText, appTypeId, appStyleId, appState, pageIndex, pageSize, out var totalCount);

            foreach (var appDto in result)
            {
                appDto.IsCreater = appDto.AccountId == Account.Id;
            }

            response.TotalCount = totalCount;
            response.IsSuccess = true;
            response.Message = "获取app列表成功";
            response.Model = result;
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// app审核通过
        /// </summary>
        [HttpPost]
        public ActionResult Pass(Int32 appId)
        {
            #region 参数验证	
            Parameter.Validate(appId);
            #endregion

            var response = new ResponseModel();
            _appServices.Pass(appId);
            response.IsSuccess = true;
            response.Message = "app审核通过";

            return Json(response);
        }

        /// <summary>
        /// app审核不通过
        /// </summary>
        [HttpPost]
        public ActionResult Deny(Int32 appId)
        {
            #region 参数验证	
            Parameter.Validate(appId);
            #endregion

            var response = new ResponseModel();
            _appServices.Deny(appId);
            response.IsSuccess = true;
            response.Message = "app审核不通过";

            return Json(response);
        }

        /// <summary>
        /// 设置app为今日推荐
        /// </summary>
        [HttpPost]
        public ActionResult Recommend(Int32 appId)
        {
            #region 参数验证	
            Parameter.Validate(appId);
            #endregion

            var response = new ResponseModel();
            _appServices.SetTodayRecommandApp(appId);
            response.IsSuccess = true;
            response.Message = "设置成功";

            return Json(response);
        }

        /// <summary>
        /// 删除app
        /// </summary>
        [HttpPost]
        public ActionResult RemoveApp(Int32 appId)
        {
            #region 参数验证	
            Parameter.Validate(appId);
            #endregion

            var response = new ResponseModel();
            _appServices.RemoveApp(appId);
            response.IsSuccess = true;
            response.Message = "删除app成功";

            return Json(response);
        }


        /// <summary>
        /// 检查应用名称
        /// </summary>
        [HttpPost]
        public ActionResult CheckAppName(String param)
        {
            Parameter.Validate(param);

            var result = AccountServices.CheckAppName(param);
            return Json(!result ? new { status = "y", info = "" } : new { status = "n", info = "应用名称已存在" });
        }

        /// <summary>
        /// 检查应用Url
        /// </summary>
        [HttpPost]
        public ActionResult CheckAppUrl(String param)
        {
            Parameter.Validate(param);

            var result = AccountServices.CheckAppUrl(param);
            return Json(!result ? new { status = "y", info = "" } : new { status = "n", info = "应用Url已存在" });
        }
    }
}