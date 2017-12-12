using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using NewCRM.Application.Services.Interface;
using NewCRM.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    public class DeskOptionsController : BaseController
    {

        private readonly IWallpaperServices _wallpaperServices;
        private readonly ISkinServices _skinServices;
        private readonly IDeskServices _deskServices;
        private readonly IAppServices _appServices;

        public DeskOptionsController(IWallpaperServices wallpaperServices,
            ISkinServices skinServices,
            IDeskServices deskServices,
            IAppServices appServices)
        {
            _wallpaperServices = wallpaperServices;
            _skinServices = skinServices;
            _deskServices = deskServices;
            _appServices = appServices;
        }


        #region 页面

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult SystemWallPaper()
        {
            ViewData["AccountConfig"] = AccountServices.GetConfig(Account.Id);
            ViewData["Wallpapers"] = _wallpaperServices.GetWallpapers();

            return View();
        }

        /// <summary>
        /// 自定义壁纸
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomWallPaper()
        {
            ViewData["AccountConfig"] = AccountServices.GetConfig(Account.Id);
            return View();
        }

        /// <summary>
        /// 设置皮肤
        /// </summary>
        /// <returns></returns>
        public ActionResult SetSkin()
        {
            return View();
        }

        /// <summary>
        /// 程序设置
        /// </summary>
        /// <returns></returns>
        public ActionResult DeskSet()
        {
            ViewData["AccountConfig"] = AccountServices.GetConfig(Account.Id);
            ViewData["Desks"] = AccountServices.GetConfig(Account.Id).DefaultDeskCount;

            return View();
        }
        #endregion

        /// <summary>
        /// 设置壁纸显示模式
        /// </summary>
        [HttpPost]
        public ActionResult ModifyWallpaperDisplayModel(String wallPaperShowType)
        {
            #region 参数验证
            Parameter.Validate(wallPaperShowType);
            #endregion

            var response = new ResponseModel();
            _wallpaperServices.ModifyWallpaperMode(Account.Id, wallPaperShowType);
            response.IsSuccess = true;
            response.Message = "壁纸显示模式设置成功";

            return Json(response);
        }

        /// <summary>
        /// 设置壁纸
        /// </summary>
        [HttpPost]
        public ActionResult ModifyWallpaper(Int32 wallpaperId)
        {
            #region 参数验证
            Parameter.Validate(wallpaperId);
            #endregion

            var response = new ResponseModel();
            _wallpaperServices.ModifyWallpaper(Account.Id, wallpaperId);
            response.IsSuccess = true;
            response.Message = "设置壁纸成功";

            return Json(response);
        }

        /// <summary>
        /// 载入用户之前上传的壁纸
        /// </summary>
        [HttpGet]
        public ActionResult GetUploadWallPapers()
        {
            var response = new ResponseModel<IList<WallpaperDto>>();
            var result = _wallpaperServices.GetUploadWallpaper(Account.Id);
            response.IsSuccess = true;
            response.Message = "载入之前上传的壁纸成功";
            response.Model = result;

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除上传的壁纸
        /// </summary>
        [HttpPost]
        public ActionResult DeleteWallPaper(Int32 wallPaperId)
        {
            #region 参数验证
            Parameter.Validate(wallPaperId);
            #endregion

            var response = new ResponseModel<IList<WallpaperDto>>();
            _wallpaperServices.RemoveWallpaper(Account.Id, wallPaperId);
            response.IsSuccess = true;
            response.Message = "删除壁纸成功";

            return Json(response);
        }

        /// <summary>
        /// 上传壁纸     
        /// </summary>
        [HttpPost]
        public ActionResult UploadWallPaper(WallpaperDto wallpaper)
        {
            var response = new ResponseModel<dynamic>();

            var wallpaperResult = _wallpaperServices.AddWallpaper(new WallpaperDto
            {
                Title = wallpaper.Title.Substring(0, 9),
                Width = wallpaper.Width,
                Height = wallpaper.Height,
                Url = wallpaper.Url,
                Source = wallpaper.Source,
                AccountId = Account.Id,
                Md5 = wallpaper.Md5,
                ShortUrl = ""
            });

            response.Message = "壁纸上传成功";
            response.IsSuccess = true;
            response.Model = new { Id = wallpaperResult.Item1, Url = ProfileManager.FileUrl + wallpaperResult.Item2 };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 网络壁纸
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> WebWallPaper(String webUrl)
        {
            var response = new ResponseModel<Tuple<Int32, String>>();

            var result = _wallpaperServices.AddWebWallpaper(Account.Id, webUrl);
            response.IsSuccess = true;
            response.Message = "网络壁纸保存成功";
            response.Model = await result;

            return Json(response);
        }

        /// <summary>
        /// 获取全部的皮肤
        /// </summary>
        [HttpGet]
        public ActionResult GetSkins()
        {
            var response = new ResponseModel<dynamic>();

            var skinPath = Server.MapPath(ConfigurationManager.AppSettings["PlantFormSkinPath"]);
            var result = _skinServices.GetAllSkin(skinPath);
            response.IsSuccess = true;
            response.Message = "获取皮肤列表成功";
            response.Model = new { data = result, currentSkin = AccountServices.GetConfig(Account.Id).Skin };

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 更换皮肤
        /// </summary>
        [HttpPost]
        public ActionResult ModifySkin(String skin)
        {
            #region 参数验证
            Parameter.Validate(skin);
            #endregion

            var response = new ResponseModel();

            _skinServices.ModifySkin(Account.Id, skin);
            response.IsSuccess = true;
            response.Message = "更换皮肤成功";

            return Json(response);
        }

        /// <summary>
        /// 更换默认显示的桌面
        /// </summary>
        [HttpPost]
        public ActionResult ModifyDefaultDesk(Int32 deskNum)
        {
            #region 参数验证
            Parameter.Validate(deskNum);
            #endregion

            var response = new ResponseModel();
            _deskServices.ModifyDefaultDeskNumber(Account.Id, deskNum);
            response.IsSuccess = true;
            response.Message = "更换默认桌面成功";

            return Json(response);
        }

        /// <summary>
        /// 更换图标的排列方向
        /// </summary>
        [HttpPost]
        public ActionResult ModifyAppXy(String appXy)
        {
            #region 参数验证
            Parameter.Validate(appXy);
            #endregion

            var response = new ResponseModel();

            _appServices.ModifyAppDirection(Account.Id, appXy);
            response.IsSuccess = true;
            response.Message = "更换图标排列方向成功";

            return Json(response);
        }

        /// <summary>
        /// 更改图标大小
        /// </summary>
        [HttpPost]
        public ActionResult ModifyAppSize(Int32 appSize)
        {
            #region 参数验证
            Parameter.Validate(appSize);
            #endregion

            var response = new ResponseModel();
            _appServices.ModifyAppIconSize(Account.Id, appSize);
            response.IsSuccess = true;
            response.Message = "更改图标大小成功";

            return Json(response);
        }

        /// <summary>
        /// 更改应用图标的垂直间距
        /// </summary>
        [HttpPost]
        public ActionResult ModifyAppVertical(Int32 appVertical)
        {
            #region 参数验证
            Parameter.Validate(appVertical);
            #endregion

            var response = new ResponseModel();
            _appServices.ModifyAppVerticalSpacing(Account.Id, appVertical);
            response.IsSuccess = true;
            response.Message = "更改图标垂直间距成功";

            return Json(response);
        }

        /// <summary>
        /// 更改图标的水平间距
        /// </summary>
        [HttpPost]
        public ActionResult ModifyAppHorizontal(Int32 appHorizontal)
        {
            #region 参数验证
            Parameter.Validate(appHorizontal);
            #endregion

            var response = new ResponseModel();
            _appServices.ModifyAppHorizontalSpacing(Account.Id, appHorizontal);
            response.IsSuccess = true;
            response.Message = "更改图标水平间距成功";

            return Json(response);
        }

        /// <summary>
        /// 更改码头的位置
        /// </summary>
        [HttpPost]
        public ActionResult ModifyDockPosition(String pos, Int32 deskNum)
        {
            #region 参数验证
            Parameter.Validate(pos).Validate(deskNum);
            #endregion

            var response = new ResponseModel();
            _deskServices.ModifyDockPosition(Account.Id, deskNum, pos);
            response.IsSuccess = true;
            response.Message = "更改码头的位置成功";

            return Json(response);
        }

        /// <summary>
        /// 修改壁纸来源
        /// </summary>
        [HttpPost]
        public ActionResult ModifyWallpaperSource(String source)
        {
            #region 参数验证
            Parameter.Validate(source);
            #endregion

            var response = new ResponseModel();
            _deskServices.ModifyWallpaperSource(source, Account.Id);
            response.IsSuccess = true;
            response.Message = "更改壁纸来源成功";

            return Json(response);

        }
    }
}