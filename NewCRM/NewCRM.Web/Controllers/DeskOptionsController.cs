using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using NewCRM.Application.Services.Interface;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;
using System.Collections.Generic;

namespace NewCRM.Web.Controllers
{
    public class DeskOptionsController : BaseController
    {

        private readonly IWallpaperApplicationServices _wallpaperServices;

        private readonly ISkinApplicationServices _skinServices;

        private readonly IDeskApplicationServices _deskServices;

        private readonly IAppApplicationServices _appServices;

        private readonly IAccountApplicationServices _accountServices;

        public DeskOptionsController(IWallpaperApplicationServices wallpaperServices,
            ISkinApplicationServices skinServices,
            IDeskApplicationServices deskServices,
            IAppApplicationServices appServices,
            IAccountApplicationServices accountServices)
        {
            _wallpaperServices = wallpaperServices;
            _skinServices = skinServices;
            _deskServices = deskServices;
            _appServices = appServices;
            _accountServices = accountServices;
        }


        #region 页面

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult SystemWallPaper(Int32 accountId)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            ViewData["AccountConfig"] = _accountServices.GetConfig(accountId);
            ViewData["Wallpapers"] = _wallpaperServices.GetWallpaper();

            return View();
        }

        /// <summary>
        /// 自定义壁纸
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomWallPaper(Int32 accountId)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            ViewData["AccountConfig"] = _accountServices.GetConfig(accountId);

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
        public ActionResult DeskSet(Int32 accountId)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            ViewData["AccountConfig"] = _accountServices.GetConfig(accountId);
            ViewData["Desks"] = _accountServices.GetConfig(accountId).DefaultDeskCount;

            return View();
        }
        #endregion

        /// <summary>
        /// 设置壁纸显示模式
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyWallpaperDisplayModel(Int32 accountId, String wallPaperShowType )
        {
            #region 参数验证
            Parameter.Validate(accountId).Validate(wallPaperShowType);
            #endregion

            var response = new ResponseModel();
            _wallpaperServices.ModifyWallpaperMode(accountId, wallPaperShowType);
            response.IsSuccess = true;
            response.Message = "壁纸显示模式设置成功";

            return Json(response);
        }

        /// <summary>
        /// 设置壁纸
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyWallpaper(Int32 accountId, Int32 wallpaperId)
        {
            #region 参数验证
            Parameter.Validate(accountId).Validate(wallpaperId);
            #endregion

            var response = new ResponseModel();
            _wallpaperServices.ModifyWallpaper(accountId, wallpaperId);
            response.IsSuccess = true;
            response.Message = "设置壁纸成功";

            return Json(response);
        }

        /// <summary>
        /// 载入用户之前上传的壁纸
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllUploadWallPaper(Int32 accountId)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModel<IList<WallpaperDto>>();
            var result = _wallpaperServices.GetUploadWallpaper(accountId);
            response.IsSuccess = true;
            response.Message = "载入之前上传的壁纸成功";
            response.Model = result;

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除上传的壁纸
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteWallPaper(Int32 accountId, Int32 wallPaperId)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModel<IList<WallpaperDto>>();
            _wallpaperServices.RemoveWallpaper(accountId, wallPaperId);
            response.IsSuccess = true;
            response.Message = "删除壁纸成功";

            return Json(response);
        }

        /// <summary>
        /// 上传壁纸     
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadWallPaper(Int32 accountId)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModel<dynamic>();
            if (Request.Files.Count > 0)
            {
                var httpPostedFile = HttpContext.Request.Files[0];
                var wallpaperDtoResult = _wallpaperServices.GetUploadWallpaper(CalculateFile.Calculate(httpPostedFile.InputStream));

                if (wallpaperDtoResult != null)
                {
                    response.Message = "这张壁纸已经存在";
                    response.IsSuccess = true;
                }
                else
                {
                    var fileUpLoad = new FileUpLoadHelper(ConfigurationManager.AppSettings["UploadWallPaperPath"], false, false, true, true, 160, 115, ThumbnailMode.Auto, false, "");
                    var imgNd5 = CalculateFile.Calculate(httpPostedFile.InputStream);

                    if (fileUpLoad.SaveFile(httpPostedFile))
                    {
                        var shortUrl =
                            fileUpLoad.WebThumbnailFilePath.Substring(fileUpLoad.WebThumbnailFilePath.LastIndexOf("Script", StringComparison.OrdinalIgnoreCase)).Replace(@"\", "/").Insert(0, "/");

                        var wallpaperResult = _wallpaperServices.AddWallpaper(new WallpaperDto
                        {
                            Height = fileUpLoad.FileHeight,
                            Source = "Upload",
                            Title = fileUpLoad.OldFileName,
                            Url = fileUpLoad.FilePath + fileUpLoad.OldFileName,
                            AccountId = accountId,
                            Width = fileUpLoad.FileWidth,
                            Md5 = imgNd5,
                            ShortUrl = shortUrl
                        });

                        response.Message = "壁纸上传成功";
                        response.IsSuccess = true;
                        response.Model = new { Id = wallpaperResult.Item1, Url = wallpaperResult.Item2 };
                    }
                    else
                    {
                        response.Message = "壁纸上传失败";
                    }
                }
            }
            else
            {
                response.Message = "请先选择一张壁纸";
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 网络壁纸
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> WebWallPaper(Int32 accountId, String webUrl)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModel<Tuple<Int32, String>>();

            var result = _wallpaperServices.AddWebWallpaper(accountId, webUrl);
            response.IsSuccess = true;
            response.Message = "网络壁纸保存成功";
            response.Model = await result;

            return Json(response);
        }

        /// <summary>
        /// 获取全部的皮肤
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllSkin(Int32 accountId)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModel<dynamic>();

            var skinPath = Server.MapPath(ConfigurationManager.AppSettings["PlantFormSkinPath"]);
            var result = _skinServices.GetAllSkin(skinPath);
            response.IsSuccess = true;
            response.Message = "获取皮肤列表成功";
            response.Model = new { data = result, currentSkin = _accountServices.GetConfig(accountId).Skin };

            return Json(response);

        }

        /// <summary>
        /// 更换皮肤
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifySkin(Int32 accountId, String skin)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModel();

            _skinServices.ModifySkin(accountId, skin);
            response.IsSuccess = true;
            response.Message = "更换皮肤成功";

            return Json(response);
        }

        /// <summary>
        /// 更换默认显示的桌面
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyDefaultDesk(Int32 accountId, Int32 deskNum)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModel();
            _deskServices.ModifyDefaultDeskNumber(accountId, deskNum);
            response.IsSuccess = true;
            response.Message = "更换默认桌面成功";

            return Json(response);
        }

        /// <summary>
        /// 更换图标的排列方向
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyAppXy(Int32 accountId, String appXy)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModel();

            _appServices.ModifyAppDirection(accountId, appXy);
            response.IsSuccess = true;
            response.Message = "更换图标排列方向成功";

            return Json(response);
        }

        /// <summary>
        /// 更改图标大小
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyAppSize(Int32 accountId, Int32 appSize)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModel();
            _appServices.ModifyAppIconSize(accountId, appSize);
            response.IsSuccess = true;
            response.Message = "更改图标大小成功";

            return Json(response);
        }

        /// <summary>
        /// 更改应用图标的垂直间距
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyAppVertical(Int32 accountId, Int32 appVertical)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModel();
            _appServices.ModifyAppVerticalSpacing(accountId, appVertical);
            response.IsSuccess = true;
            response.Message = "更改图标垂直间距成功";

            return Json(response);
        }

        /// <summary>
        /// 更改图标的水平间距
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyAppHorizontal(Int32 accountId, Int32 appHorizontal)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModel();
            _appServices.ModifyAppHorizontalSpacing(accountId, appHorizontal);
            response.IsSuccess = true;
            response.Message = "更改图标水平间距成功";

            return Json(response);
        }

        /// <summary>
        /// 更改码头的位置
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyDockPosition(Int32 accountId, String pos, Int32 deskNum )
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModel();
            _deskServices.ModifyDockPosition(accountId, deskNum, pos);
            response.IsSuccess = true;
            response.Message = "更改码头的位置成功";

            return Json(response);
        }
    }
}