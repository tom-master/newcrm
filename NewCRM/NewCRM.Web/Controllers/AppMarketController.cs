using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using NewCRM.Application.Interface;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    public class AppMarketController : BaseController
    {
        private readonly IAppApplicationServices _appApplicationServices;

        
        public AppMarketController(IAppApplicationServices appApplicationServices)
        {
            _appApplicationServices = appApplicationServices;
        }

        #region 页面

        #region 应用市场

        /// <summary>
        /// 应用市场
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (AccountDto.IsAdmin)
            {
                ViewData["AppTypes"] = _appApplicationServices.GetAppTypes();
            }
            else
            {
                ViewData["AppTypes"] = _appApplicationServices.GetAppTypes().Where(w => w.Name != "系统").ToList();
            }

            ViewData["TodayRecommendApp"] = _appApplicationServices.GetTodayRecommend();

            ViewData["AccountName"] = AccountDto.Name;

            ViewData["AccountApp"] = _appApplicationServices.GetAccountDevelopAppCountAndNotReleaseAppCount();

            return View();
        }

        /// <summary>
        /// app详情
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public ActionResult AppDetail(Int32 appId)
        {
            ViewData["IsInstallApp"] = _appApplicationServices.IsInstallApp(appId);

            ViewData["AccountName"] = AccountDto.Name;

            var singleAppResult = _appApplicationServices.GetApp(appId);

            return View(singleAppResult);
        }

        /// <summary>
        /// 用户app管理
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountAppManage()
        {
            ViewData["AppTypes"] = _appApplicationServices.GetAppTypes();

            ViewData["AppStyles"] = _appApplicationServices.GetAllAppStyles().ToList();

            ViewData["AppStates"] = _appApplicationServices.GetAllAppStates().ToList();

            return View();
        }

        /// <summary>
        /// 我的应用
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public ActionResult AccountAppManageInfo(Int32 appId)
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

            var appResults = _appApplicationServices.GetAllApps(appTypeId, orderId, searchText, pageIndex, pageSize, out totalCount);

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

            _appApplicationServices.ModifyAppStar(appId, starCount);
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
            _appApplicationServices.InstallApp(appId, deskNum);
            return Json(new { success = 1 });
        }

        /// <summary>
        /// 获取开发者（用户）的app
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="appTypeId"></param>
        /// <param name="appStyleId"></param>
        /// <param name="appState"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult GetAccountAllApps(String searchText, Int32 appTypeId, Int32 appStyleId, String appState, Int32 pageIndex, Int32 pageSize)
        {
            Int32 totalCount;

            var appResults = _appApplicationServices.GetAccountAllApps(AccountDto.Id, searchText, appTypeId, appStyleId, appState, pageIndex, pageSize, out totalCount);

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
            var appDto = WrapperAppDto(forms);
            _appApplicationServices.ModifyAccountAppInfo(appDto);

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

        /// <summary>
        /// 创建新的app
        /// </summary>
        /// <param name="forms"></param>
        /// <returns></returns>
        public ActionResult CreateNewApp(FormCollection forms)
        {
            var appDto = WrapperAppDto(forms);

            appDto.AccountId = AccountDto.Id;

            _appApplicationServices.CreateNewApp(appDto);

            return Json(new
            {
                success = 1
            });
        }

        /// <summary>
        /// 审核通过后发布app
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public ActionResult ReleaseApp(Int32 appId)
        {
            _appApplicationServices.ReleaseApp(appId);

            return Json(new { success = 1 });
        }

        /// <summary>
        /// 删除用户开发
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public ActionResult RemoveApp(Int32 appId)
        {
            _appApplicationServices.RemoveApp(appId);

            return Json(new
            {
                success = 1
            });
        }

        #region private method
        /// <summary>
        /// 封装从页面传入的forms表单到AppDto类型
        /// </summary>
        /// <param name="forms"></param>
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