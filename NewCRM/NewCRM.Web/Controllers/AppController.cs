using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustemException;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    [Export]
    public class AppController : BaseController
    {
        [Import]
        private IAppApplicationServices _appApplicationServices;

        #region 页面

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
        /// 用户app管理
        /// </summary>
        /// <returns></returns>
        public ActionResult UserAppManage()
        {
            ViewData["AppTypes"] = _appApplicationServices.GetAppTypes();

            ViewData["AppStyles"] = _appApplicationServices.GetAllAppStyles().ToList();

            ViewData["AppStates"] = _appApplicationServices.GetAllAppStates().ToList();

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public ActionResult UserAppManageInfo(Int32 appId)
        {
            var appResult = _appApplicationServices.GetApp(appId);

            ViewData["AppTypes"] = _appApplicationServices.GetAppTypes();

            ViewData["AppState"] = appResult.AppAuditState;

            return View(appResult);
        }

        #endregion

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
            Int32 totalCount;

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

        /// <summary>
        /// 获取开发者（用户）的app
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="appTypeId"></param>
        /// <param name="appStyleId"></param>
        /// <param name="appState"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult GetUserAllApps(String appName, Int32 appTypeId, Int32 appStyleId, String appState, Int32 pageIndex, Int32 pageSize)
        {
            Int32 totalCount;

            var appResults = _appApplicationServices.GetUserAllApps(CurrentUser.Id, appName, appTypeId, appStyleId, appState, pageIndex, pageSize, out totalCount);

            return Json(new
            {
                apps = appResults,
                totalCount
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 修改app信息
        /// </summary>
        /// <param name="forms"></param>
        /// <returns></returns>
        public ActionResult ModifyAppInfo(FormCollection forms)
        {
            //app样式枚举
            AppStyle appStyle;

            if (!Enum.TryParse(forms["val_type"], true, out appStyle))
            {
                throw new BusinessException($"{forms["val_type"]}不是有效的枚举值");
            }

            //app审核枚举
            AppAuditState appAuditState;
            if (!Enum.TryParse(forms["val_verifytype"], true, out appAuditState))
            {
                throw new BusinessException($"{forms["val_verifytype"]}不是有效的枚举值");
            }


            var appDto = new AppDto
            {
                Id = Int32.Parse(forms["val_Id"]),
                IconUrl = forms["val_icon"],
                Name = forms["val_name"],
                AppTypeId = Int32.Parse(forms["val_app_category_id"]),
                AppUrl = forms["val_url"],
                Width = Int32.Parse(forms["val_width"]),
                Height = Int32.Parse(forms["val_height"]),
                AppStyle = appStyle,
                IsResize = Int32.Parse(forms["val_isresize"]) == 1,
                IsOpenMax = Int32.Parse(forms["val_isopenmax"]) == 1,
                IsFlash = Int32.Parse(forms["val_isflash"]) == 1,
                Remark = forms["val_remark"],
                AppAuditState = appAuditState,
                AppReleaseState = AppReleaseState.UnRelease
            };
            _appApplicationServices.ModifyUserAppInfo(CurrentUser.Id, appDto);


            return Json(new { success = 1 });
        }

        /// <summary>
        /// 更新图标
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadIcon()
        {
            if (Request.Files.Count != 0)
            {
                var icon = Request.Files[0];

                var fileUpLoadHelper = new FileUpLoadHelper(ConfigurationManager.AppSettings["UploadIconPath"], false, false);
                if (fileUpLoadHelper.SaveFile(icon))
                {
                    return Json(new { iconPath = fileUpLoadHelper.FilePath + fileUpLoadHelper.OldFileName });
                }
                return Json(new { msg = "上传失败" });
            }
            return Json(new { msg = "请上传一个图片" });
        }
    }
}