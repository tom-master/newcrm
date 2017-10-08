using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using NewCRM.Application.Services.Interface;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;
using System.Collections.Generic;

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
        public ActionResult Index(Int32 accountId, String accountName, Boolean isAdmin)
        {
            #region 参数验证
            Parameter.Validate(accountId).Validate(accountName);
            #endregion

            if (isAdmin)
            {
                ViewData["AppTypes"] = _appServices.GetAppTypes();
            }
            else
            {
                ViewData["AppTypes"] = _appServices.GetAppTypes().Where(w => w.Name != "系统").ToList();
            }

            ViewData["TodayRecommendApp"] = _appServices.GetTodayRecommend(accountId);
            ViewData["AccountName"] = accountName;
            ViewData["AccountApp"] = _appServices.GetAccountDevelopAppCountAndNotReleaseAppCount(accountId);

            return View();
        }

        /// <summary>
        /// app详情
        /// </summary>
        /// <returns></returns>
        public ActionResult AppDetail(Int32 accountId, Int32 appId, String accountName)
        {
            #region 参数验证
            Parameter.Validate(accountId).Validate(accountName).Validate(appId);
            #endregion

            ViewData["IsInstallApp"] = _appServices.IsInstallApp(accountId, appId);
            ViewData["AccountName"] = accountName;

            var result = _appServices.GetApp(appId);
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
            #region 参数验证
            Parameter.Validate(appId);
            #endregion

            AppDto result = null;
            if (appId != 0)// 如果appId为0则是新创建app
            {
                result = _appServices.GetApp(appId);
                ViewData["AppState"] = result.AppAuditState;
            }
            ViewData["AppTypes"] = _appServices.GetAppTypes();

            return View(result);
        }


        #endregion

        /// <summary>
        /// 获取所有的app
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllApps(Int32 accountId, Int32 appTypeId, Int32 orderId, String searchText, Int32 pageIndex, Int32 pageSize)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModels<IList<AppDto>>();

            Int32 totalCount;
            var result = _appServices.GetAllApps(accountId, appTypeId, orderId, searchText, pageIndex, pageSize, out totalCount);
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
        /// <returns></returns>
        public ActionResult ModifyAppStart(Int32 accountId, Int32 appId, Int32 starCount)
        {
            #region 参数验证
            Parameter.Validate(accountId).Validate(appId).Validate(starCount);
            #endregion

            var response = new ResponseModel();
            _appServices.ModifyAppStar(accountId, appId, starCount);
            response.IsSuccess = true;
            response.Message = "打分成功";

            return Json(response);
        }

        /// <summary>
        /// 安装app
        /// </summary>
        /// <returns></returns>
        public ActionResult InstallApp(Int32 accountId, Int32 appId, Int32 deskNum)
        {
            #region 参数验证
            Parameter.Validate(accountId).Validate(appId).Validate(deskNum);
            #endregion

            var response = new ResponseModel();
            _appServices.InstallApp(accountId, appId, deskNum);
            response.IsSuccess = true;
            response.Message = "打分成功";

            return Json(response);
        }

        /// <summary>
        /// 获取开发者（用户）的app
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAccountAllApps(Int32 accountId, String searchText, Int32 appTypeId, Int32 appStyleId, String appState, Int32 pageIndex, Int32 pageSize)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModels<IList<AppDto>>();
            Int32 totalCount = 0;
            var result = _appServices.GetAccountAllApps(accountId, searchText, appTypeId, appStyleId, appState, pageIndex, pageSize, out totalCount);
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
        /// <param name="forms"></param>
        /// <returns></returns>
        public ActionResult ModifyAppInfo(FormCollection forms)
        {
            #region 参数验证
            Parameter.Validate(forms);
            #endregion

            var response = new ResponseModel();
            _appServices.ModifyAccountAppInfo(Int32.Parse(forms["accountId"]), WrapperAppDto(forms));
            response.IsSuccess = true;
            response.Message = "修改app信息成功";

            return Json(response);
        }

        /// <summary>
        /// 更新图标
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadIcon()
        {
            var response = new ResponseModel();
            if (Request.Files.Count != 0)
            {
                var icon = Request.Files[0];

                var fileUpLoadHelper = new FileUpLoadHelper(ConfigurationManager.AppSettings["UploadIconPath"], false, false);
                if (fileUpLoadHelper.SaveFile(icon))
                {
                    response.IsSuccess = true;
                    response.Model = fileUpLoadHelper.FilePath + fileUpLoadHelper.OldFileName;
                    response.Message = "更新图标成功";
                }
                else
                {
                    response.Message = "上传失败";
                }
            }
            else
            {
                response.Message = "请选择一张图片后再进行上除按";
            }
            return Json(response);
        }

        /// <summary>
        /// 创建新的app
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateNewApp(FormCollection forms)
        {
            #region 参数验证
            Parameter.Validate(forms);
            #endregion

            var response = new ResponseModel();

            var appDto = WrapperAppDto(forms);
            appDto.AccountId = Int32.Parse(forms["accountId"]);

            _appServices.CreateNewApp(appDto);
            response.IsSuccess = true;
            response.Message = "app创建成功";

            return Json(response);
        }

        /// <summary>
        /// 审核通过后发布app
        /// </summary>
        /// <returns></returns>
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
        /// <returns></returns>
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
                AppReleaseState = 2 //未发布
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