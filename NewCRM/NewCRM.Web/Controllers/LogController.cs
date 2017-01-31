using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewCRM.Application.Interface;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class LogController : BaseController
    {
        private readonly ILoggerApplicationServices _loggerApplicationServices;

        [ImportingConstructor]
        public LogController(ILoggerApplicationServices loggerApplicationServices)
        {
            _loggerApplicationServices = loggerApplicationServices;
        }


        // GET: Log
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetAllLog(Int32 pageIndex, Int32 pageSize)
        {

            return null;
        }
    }
}