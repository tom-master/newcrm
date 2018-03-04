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
    public class AppTypesController : BaseController
    {
        private readonly IAppServices _appServices;

        public AppTypesController(IAppServices appServices) => _appServices = appServices;

        #region 类目管理

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() => View();

        /// <summary>
        /// 创建新的类目
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateNewAppType(Int32 appTypeId = 0)
        {
            AppTypeDto result = null;
            if (appTypeId != 0)
            {
                result = _appServices.GetAppTypes().FirstOrDefault(appType => appType.Id == appTypeId);
            }

            return View(result);
        }

        #endregion

        /// <summary>
        /// 获取所有app类型
        /// </summary>
        [HttpGet]
        public ActionResult GetAppTypes(Int32 pageIndex, Int32 pageSize, String searchText)
        {
            var response = new ResponseModels<IList<AppTypeDto>>();
            var result = _appServices.GetAppTypes().Where(appType => searchText.Length == 0 || appType.Name.Contains(searchText)).OrderByDescending(d => d.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            response.Message = "app类型获取成功";
            response.IsSuccess = true;
            response.Model = result;
            response.TotalCount = result.Count;

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 删除app类型
        /// </summary>
        [HttpPost]
        public ActionResult DeleteAppType(Int32 appTypeId)
        {
            #region 参数验证
            Parameter.Validate(appTypeId);
            #endregion

            var response = new ResponseModel();
            _appServices.RemoveAppType(appTypeId);
            response.IsSuccess = true;
            response.Message = "删除app类型成功";

            return Json(response);
        }

        /// <summary>
        /// 创建新的app类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateAppType(FormCollection forms, Int32 appTypeId = 0)
        {
            #region 参数验证
            Parameter.Validate(forms);
            #endregion
            var response = new ResponseModel();
            var appTypeDto = WrapperAppTypeDto(forms);
            if (appTypeId == 0)
            {
                _appServices.CreateNewAppType(appTypeDto);
            }
            else
            {
                _appServices.ModifyAppType(appTypeDto, appTypeId);
            }
            response.IsSuccess = true;
            response.Message = "app类型创建成功";

            return Json(response);
        }


        /// <summary>
        /// 检查应用类型名称
        /// </summary>
        [HttpPost]
        public ActionResult CheckAppTypeName(String param)
        {
            Parameter.Validate(param);
            var result = _appServices.CheckAppTypeName(param);
            return Json(!result ? new { status = "y", info = "" } : new { status = "n", info = "类型名称已存在" });
        }

        #region private method

        /// <summary>
        /// 封装从页面传入的forms表单到AppTypeDto类型
        /// </summary>
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