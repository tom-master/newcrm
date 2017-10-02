using System;
using System.Linq;
using System.Web.Mvc;
using NewCRM.Application.Services.Interface;
using NewCRM.Dto.Dto;
using NewCRM.Web.Controllers.ControllerHelper;

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
        /// 类目管理
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
            AppTypeDto appTypeDto = null;

            if (appTypeId != 0)
            {
                appTypeDto = _appApplicationServices.GetAppTypes().FirstOrDefault(appType => appType.Id == appTypeId);
            }

            return View(appTypeDto);
        }

        #endregion

        /// <summary>
        /// 获取所有app类型
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public ActionResult GetAllAppTypes(Int32 pageIndex, Int32 pageSize, String searchText)
        {
            var appTypes = _appApplicationServices.GetAppTypes().Where(appType => searchText.Length == 0 || appType.Name.Contains(searchText)).OrderByDescending(d => d.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return Json(new
            {
                totalCount = appTypes.Count,
                appTypes
            }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 删除app类型
        /// </summary>
        /// <param name="appTypeId"></param>
        /// <returns></returns>
        public ActionResult DeleteAppType(Int32 appTypeId)
        {
            _appApplicationServices.RemoveAppType(appTypeId);

            return Json(new { sucess = 1 });
        }

        /// <summary>
        /// 创建新的app类型
        /// </summary>
        /// <param name="forms"></param>
        /// <param name="appTypeId"></param>
        /// <returns></returns>
        public ActionResult NewAppType(FormCollection forms, Int32 appTypeId = 0)
        {
            var appTypeDto = WrapperAppTypeDto(forms);

            if (appTypeId == 0)
            {
                _appApplicationServices.CreateNewAppType(appTypeDto);
            }
            else
            {
                _appApplicationServices.ModifyAppType(appTypeDto, appTypeId);
            }

            return Json(new { success = 1 });
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