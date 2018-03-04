using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NewCRM.Application.Services.Interface;
using NewCRM.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;
using NewLib;

namespace NewCRM.Web.Controllers
{
    public class AppMarketController : BaseController
    {
        private readonly IAppServices _appServices;

        public AppMarketController(IAppServices appServices)
        {
            _appServices = appServices;
        }

        #region 页面

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Account.IsAdmin)
            {
                ViewData["AppTypes"] = _appServices.GetAppTypes();
            }
            else
            {
                ViewData["AppTypes"] = _appServices.GetAppTypes().Where(w => w.Name != "系统").ToList();
            }
            ViewData["TodayRecommendApp"] = _appServices.GetTodayRecommend(Account.Id);
            ViewData["AccountName"] = Account.Name;
            ViewData["AccountApp"] = _appServices.GetAccountDevelopAppCountAndNotReleaseAppCount(Account.Id);

            return View();
        }

        /// <summary>
        /// app详情
        /// </summary>
        /// <returns></returns>
        public ActionResult AppDetail(Int32 appId)
        {
            #region 参数验证
            Parameter.Validate(appId);
            #endregion

            ViewData["IsInstallApp"] = _appServices.IsInstallApp(Account.Id, appId);
            var result = _appServices.GetApp(appId);
            ViewData["AccountName"] = result.AccountName;

            return View(result);
        }

        /// <summary>
        /// 用户app管理
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountAppManage()
        {
            ViewData["AppTypes"] = _appServices.GetAppTypes();
            ViewData["AppStyles"] = _appServices.GetAllAppStyles().ToList();
            ViewData["AppStates"] = _appServices.GetAllAppStates().ToList();

            return View();
        }

        /// <summary>
        /// 我的应用
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountAppManageInfo(Int32 appId)
        {
            AppDto result = null;
            if (appId != 0)// 如果appId为0则是新创建app
            {
                result = _appServices.GetApp(appId);
                ViewData["AppState"] = result.AppAuditState;
            }
            ViewData["AppTypes"] = _appServices.GetAppTypes();
            ViewData["AccountId"] = Account.Id;
            return View(result);
        }


        #endregion

        /// <summary>
        /// 获取所有的app
        /// </summary>
        [HttpGet]
        public ActionResult GetApps(Int32 appTypeId, Int32 orderId, String searchText, Int32 pageIndex, Int32 pageSize)
        {
            var response = new ResponseModels<IList<AppDto>>();

            var result = _appServices.GetAllApps(Account.Id, appTypeId, orderId, searchText, pageIndex, pageSize, out var totalCount);
            if (result != null)
            {
                response.TotalCount = totalCount;
                response.IsSuccess = true;
                response.Message = "app列表获取成功";
                response.Model = result;
            }
            else
            {
                response.Message = "app列表获取失败";
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary> 
        /// 给app打分
        /// </summary>
        [HttpPost]
        public ActionResult ModifyAppStart(Int32 appId, Int32 starCount)
        {
            #region 参数验证
            Parameter.Validate(appId).Validate(starCount);
            #endregion

            var response = new ResponseModel();
            _appServices.ModifyAppStar(Account.Id, appId, starCount);
            response.IsSuccess = true;
            response.Message = "打分成功";

            return Json(response);
        }

        /// <summary>
        /// 安装app
        /// </summary>
        [HttpPost]
        public ActionResult InstallApp(Int32 appId, Int32 deskNum)
        {
            #region 参数验证
            Parameter.Validate(appId).Validate(deskNum);
            #endregion
             
            var response = new ResponseModel();
            _appServices.InstallApp(Account.Id, appId, deskNum);
            response.IsSuccess = true;
            response.Message = "安装成功";

            return Json(response);
        }
         
        /// <summary>
        /// 获取开发者（用户）的app
        /// </summary>
        [HttpGet]
        public ActionResult GetAccountApps(String searchText, Int32 appTypeId, Int32 appStyleId, String appState, Int32 pageIndex, Int32 pageSize)
        {
            var response = new ResponseModels<IList<AppDto>>();
            var result = _appServices.GetAccountAllApps(Account.Id, searchText, appTypeId, appStyleId, appState, pageIndex, pageSize, out var totalCount);
            if (result != null)
            {
                response.TotalCount = totalCount; 
                response.IsSuccess = true;
                response.Message = "app列表获取成功";
                response.Model = result;
            }
            else
            {
                response.Message = "app列表获取失败";
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改app信息
        /// </summary>
        [HttpPost]
        public ActionResult ModifyAppInfo(FormCollection forms)
        {
            #region 参数验证
            Parameter.Validate(forms);
            #endregion

            var response = new ResponseModel();
            _appServices.ModifyAccountAppInfo(Account.Id, WrapperAppDto(forms));
            response.IsSuccess = true;
            response.Message = "修改app信息成功";

            return Json(response);
        }

        /// <summary>
        /// 更新图标
        /// </summary>
        [HttpPost]
        public ActionResult UploadIcon(Int32 appId, String newIcon)
        {
            #region 参数验证
            Parameter.Validate(appId).Validate(newIcon);
            #endregion

            var response = new ResponseModel<String>();
            _appServices.ModifyAppIcon(Account.Id, appId, newIcon);

            response.IsSuccess = true;
            response.Message = "更新图标成功";
            response.Model = ProfileManager.FileUrl + newIcon;

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 创建新的app
        /// </summary>
        [HttpPost]
        public ActionResult CreateApp(FormCollection forms)
        {
            #region 参数验证
            Parameter.Validate(forms);
            #endregion

            var response = new ResponseModel();

            var appDto = WrapperAppDto(forms);
            appDto.AccountId = Account.Id;
            _appServices.CreateNewApp(appDto);

            response.IsSuccess = true;
            response.Message = "app创建成功";

            return Json(response);
        }

        /// <summary>
        /// 审核通过后发布app
        /// </summary>
        [HttpPost]
        public ActionResult ReleaseApp(Int32 appId)
        {
            #region 参数验证
            Parameter.Validate(appId);
            #endregion

            var response = new ResponseModel();
            _appServices.ReleaseApp(appId);
            response.IsSuccess = true;
            response.Message = "app发布成功";

            return Json(response);
        }

        /// <summary>
        /// 删除用户开发的app
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
            response.Message = "删除用户开发的app成功";

            return Json(response);
        }

        #region private method
        /// <summary>
        /// 封装从页面传入的forms表单到AppDto类型
        /// </summary>
        /// <returns></returns>
        private static AppDto WrapperAppDto(FormCollection forms)
        {
            var appDto = new AppDto
            {
                IconUrl = forms["val_icon"],
                Name = forms["val_name"],
                AppTypeId = Int32.Parse(forms["val_app_category_id"]),
                AppUrl = forms["val_url"],
                Width = Int32.Parse(forms["val_width"]),
                Height = Int32.Parse(forms["val_height"]),
                AppStyle = Int32.Parse(forms["val_type"]),
                IsResize = Int32.Parse(forms["val_isresize"]) == 1,
                IsOpenMax = Int32.Parse(forms["val_isopenmax"]) == 1,
                IsFlash = Int32.Parse(forms["val_isflash"]) == 1,
                Remark = forms["val_remark"],
                AppAuditState = Int32.Parse(forms["val_verifytype"]),
                AppReleaseState = 2, //未发布
                IsIconByUpload = Int32.Parse(forms["isIconByUpload"]) == 1
            };

            if ((forms["val_Id"] + "").Length > 0)
            {
                appDto.Id = Int32.Parse(forms["val_Id"]);
            }

            return appDto;
        }

        #endregion
    }
}