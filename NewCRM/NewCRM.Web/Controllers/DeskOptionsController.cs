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

		private readonly IWallpaperApplicationServices _wallpaperApplicationServices;

		private readonly ISkinApplicationServices _skinApplicationServices;

		private readonly IDeskApplicationServices _deskApplicationServices;

		private readonly IAppApplicationServices _appApplicationServices;

		private readonly IAccountApplicationServices _accountApplicationServices;

		public DeskOptionsController(IWallpaperApplicationServices wallpaperApplicationServices,
			ISkinApplicationServices skinApplicationServices,
			IDeskApplicationServices deskApplicationServices,
			IAppApplicationServices appApplicationServices,
			IAccountApplicationServices accountApplicationServices)
		{
			_wallpaperApplicationServices = wallpaperApplicationServices;
			_skinApplicationServices = skinApplicationServices;
			_deskApplicationServices = deskApplicationServices;
			_appApplicationServices = appApplicationServices;
			_accountApplicationServices = accountApplicationServices;
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

			ViewData["AccountConfig"] = _accountApplicationServices.GetConfig(accountId);
			ViewData["Wallpapers"] = _wallpaperApplicationServices.GetWallpaper();

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

			ViewData["AccountConfig"] = _accountApplicationServices.GetConfig(accountId);

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

			ViewData["AccountConfig"] = _accountApplicationServices.GetConfig(accountId);
			ViewData["Desks"] = _accountApplicationServices.GetConfig(accountId).DefaultDeskCount;

			return View();
		}
		#endregion

		/// <summary>
		/// 设置壁纸显示模式
		/// </summary>
		/// <returns></returns>
		public ActionResult ModifyWallpaperDisplayModel(Int32 accountId, String wallPaperShowType = "")
		{
			#region 参数验证
			Parameter.Validate(accountId).Validate(wallPaperShowType);
			#endregion

			var response = new ResponseModel();
			_wallpaperApplicationServices.ModifyWallpaperMode(accountId, wallPaperShowType);
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
			_wallpaperApplicationServices.ModifyWallpaper(accountId, wallpaperId);
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
			var result = _wallpaperApplicationServices.GetUploadWallpaper(accountId);
			response.IsSuccess = true;
			response.Message = "载入之前上传的壁纸成功";
			response.Model = result;

			return Json(response, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// 删除上传的壁纸
		/// </summary>
		/// <param name="wallPaperId"></param>
		/// <returns></returns>
		public ActionResult DeleteWallPaper(Int32 accountId, Int32 wallPaperId = 0)
		{
			#region 参数验证
			Parameter.Validate(accountId);
			#endregion

			var response = new ResponseModel<IList<WallpaperDto>>();
			_wallpaperApplicationServices.RemoveWallpaper(accountId, wallPaperId);
			response.IsSuccess = true;
			response.Message = "删除壁纸成功";

			return Json(response);
		}

		/// <summary>
		/// 上传壁纸     
		/// </summary>
		/// <returns></returns>c
		public ActionResult UploadWallPaper(Int32 accountId)
		{
			#region 参数验证
			Parameter.Validate(accountId);
			#endregion

			var response = new ResponseModel<dynamic>();
			if (Request.Files.Count > 0)
			{
				var httpPostedFile = HttpContext.Request.Files[0];
				var wallpaperDtoResult = _wallpaperApplicationServices.GetUploadWallpaper(CalculateFile.Calculate(httpPostedFile.InputStream));

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

						var wallpaperResult = _wallpaperApplicationServices.AddWallpaper(new WallpaperDto
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
		/// <param name="webUrl"></param>
		/// <returns></returns>
		public async Task<ActionResult> WebWallPaper(Int32 accountId, String webUrl = "")
		{
			#region 参数验证
			Parameter.Validate(accountId);
			#endregion

			var response = new ResponseModel<Tuple<Int32, String>>();
			var result = _wallpaperApplicationServices.AddWebWallpaper(accountId, webUrl);
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
			var skinPath = Server.MapPath(ConfigurationManager.AppSettings["PlantFormSkinPath"]);
			var data = _skinApplicationServices.GetAllSkin(skinPath);

			return Json(new { data, currentSkin = _accountApplicationServices.GetConfig(accountId).Skin });
		}

		/// <summary>
		/// 更换皮肤
		/// </summary>
		/// <param name="skin"></param>
		/// <returns></returns>
		public ActionResult ModifySkin(Int32 accountId,String skin = "")
		{
			_skinApplicationServices.ModifySkin(accountId, skin);

			return Json(new { success = 1 });
		}

		/// <summary>
		/// 更换默认显示的桌面
		/// </summary>
		/// <param name="deskNum"></param>
		/// <returns></returns>
		public ActionResult ModifyDefaultDesk(Int32 accountId, Int32 deskNum)
		{
			_deskApplicationServices.ModifyDefaultDeskNumber(accountId, deskNum);

			return Json(new { success = 1 });
		}

		/// <summary>
		/// 更换图标的排列方向
		/// </summary>
		/// <param name="appXy"></param>
		/// <returns></returns>
		public ActionResult ModifyAppXy(Int32 accountId, String appXy)
		{
			_appApplicationServices.ModifyAppDirection(accountId, appXy);

			return Json(new { success = 1 });
		}

		/// <summary>
		/// 更改图标大小
		/// </summary>
		/// <param name="appSize"></param>
		/// <returns></returns>
		public ActionResult ModifyAppSize(Int32 accountId, Int32 appSize)
		{
			_appApplicationServices.ModifyAppIconSize(accountId, appSize);

			return Json(new { success = 1 });
		}

		/// <summary>
		/// 更改应用图标的垂直间距
		/// </summary>
		/// <param name="appVertical"></param>
		/// <returns></returns>
		public ActionResult ModifyAppVertical(Int32 accountId, Int32 appVertical)
		{
			_appApplicationServices.ModifyAppVerticalSpacing(accountId, appVertical);

			return Json(new { success = 1 });
		}

		/// <summary>
		/// 修改图标的水平间距
		/// </summary>
		/// <param name="appHorizontal"></param>
		/// <returns></returns>
		public ActionResult ModifyAppHorizontal(Int32 accountId, Int32 appHorizontal)
		{
			_appApplicationServices.ModifyAppHorizontalSpacing(accountId, appHorizontal);

			return Json(new { success = 1 });
		}

		/// <summary>
		/// 更改码头的位置
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="deskNum"></param>
		/// <returns></returns>
		public ActionResult ModifyDockPosition(Int32 accountId, String pos = "", Int32 deskNum = 0)
		{
			_deskApplicationServices.ModifyDockPosition(accountId, deskNum, pos);

			return Json(new { success = 1 });
		}
	}
}