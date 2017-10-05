using System;
using System.Linq;
using System.Web.Mvc;
using NewCRM.Application.Services.Interface;
using NewCRM.Dto.Dto;
using NewCRM.Web.Controllers.ControllerHelper;
using NewCRM.Infrastructure.CommonTools;
using System.Collections.Generic;

namespace NewCRM.Web.Controllers
{
	public class AppTypesController : BaseController
	{
		private readonly IAppApplicationServices _appApplicationServices;

		public AppTypesController(IAppApplicationServices appApplicationServices)
		{
			_appApplicationServices = appApplicationServices;
		}

		#region 类目管理

		/// <summary>
		/// 首页
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// 创建新的类目
		/// </summary>
		/// <returns></returns>
		public ActionResult CreateNewAppType(Int32 appTypeId = 0)
		{
			AppTypeDto result = null;
			if (appTypeId != 0)
			{
				result = _appApplicationServices.GetAppTypes().FirstOrDefault(appType => appType.Id == appTypeId);
			}

			return View(result);
		}

		#endregion

		/// <summary>
		/// 获取所有app类型
		/// </summary>
		/// <returns></returns>
		public ActionResult GetAllAppTypes(Int32 pageIndex, Int32 pageSize, String searchText)
		{
			var response = new ResponseModels<IList<AppTypeDto>>();
			var result = _appApplicationServices.GetAppTypes().Where(appType => searchText.Length == 0 || appType.Name.Contains(searchText)).OrderByDescending(d => d.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
			response.Message = "app类型获取成功";
			response.IsSuccess = true;
			response.Model = result;
			response.TotalCount = result.Count;

			return Json(response, JsonRequestBehavior.AllowGet);

		}

		/// <summary>
		/// 删除app类型
		/// </summary>
		/// <param name="appTypeId"></param>
		/// <returns></returns>
		public ActionResult DeleteAppType(Int32 appTypeId)
		{
			#region 参数验证
			Parameter.Validate(appTypeId);
			#endregion

			var response = new ResponseModel();
			_appApplicationServices.RemoveAppType(appTypeId);
			response.IsSuccess = true;
			response.Message = "删除app类型成功";

			return Json(response);
		}

		/// <summary>
		/// 创建新的app类型
		/// </summary>
		/// <param name="forms"></param>
		/// <param name="appTypeId"></param>
		/// <returns></returns>
		public ActionResult NewAppType(FormCollection forms, Int32 appTypeId = 0)
		{
			#region 参数验证
			Parameter.Validate(forms);
			#endregion
			var response = new ResponseModel();
			var appTypeDto = WrapperAppTypeDto(forms);
			if (appTypeId == 0)
			{
				_appApplicationServices.CreateNewAppType(appTypeDto);
			}
			else
			{
				_appApplicationServices.ModifyAppType(appTypeDto, appTypeId);
			}
			response.IsSuccess = true;
			response.Message = "app类型创建成功";

			return Json(response);
		}

		#region private method

		/// <summary>
		/// 封装从页面传入的forms表单到AppTypeDto类型
		/// </summary>
		/// <param name="forms"></param>
		/// <returns></returns>
		private static AppTypeDto WrapperAppTypeDto(FormCollection forms)
		{
			var appTypeDto = new AppTypeDto
			{
				Name = forms["val_name"]
			};

			if ((forms["id"] + "").Length > 0)
			{
				appTypeDto.Id = Int32.Parse(forms["id"]);
			}

			return appTypeDto;

		}

		#endregion
	}
}