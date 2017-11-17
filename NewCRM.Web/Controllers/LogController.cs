using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NewCRM.Application.Services.Interface;
using NewCRM.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    public class LogController : BaseController
    {
        private readonly ILoggerServices _loggerServices;
        
        public LogController(ILoggerServices loggerServices)
        {
            _loggerServices = loggerServices;
        }


        #region 页面
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        /// <summary>
        /// 获取日志列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllLog(Int32 loglevel, Int32 pageIndex, Int32 pageSize)
        {
            var response = new ResponseModels<IList<LogDto>>();
            var totalCount = 0;
            var result = _loggerServices.GetAllLog(loglevel, pageIndex, pageSize, out totalCount);
            response.IsSuccess = true;
            response.Message = "获取日志列表成功";
            response.Model = result;
            response.TotalCount = totalCount;

            return Json(response, JsonRequestBehavior.AllowGet);
        } 
    }  
}