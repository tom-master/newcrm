using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    [Export]
    public class AppController : BaseController
    {
        [Import]
        private IAppApplicationServices _appApplicationServices;

        public ActionResult AppMarket()
        {
            ViewData["AppTypes"] = _appApplicationServices.GetAppTypes();

            ViewData["TodayRecommendApp"] = _appApplicationServices.GetTodayRecommend(CurrentUser.Id);

            ViewData["UserName"] = CurrentUser.Name;

            ViewData["UserApp"] = _appApplicationServices.GetUserDevAppAndUnReleaseApp(CurrentUser.Id);

            return View();
        }
    }
}