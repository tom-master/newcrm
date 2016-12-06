using System.ComponentModel.Composition;
using System.Text;
using System.Web.Mvc;
using NewCRM.Application.Interface;
using NewCRM.Dto.Dto;

namespace NewCRM.Web.Controllers.ControllerHelper
{

    public abstract class BaseController : Controller
    {
        [Import]
        protected IAppApplicationServices AppApplicationServices { get; set; }

        [Import]
        protected IDeskApplicationServices DeskApplicationServices { get; set; }

        [Import]
        protected IWallpaperApplicationServices WallpaperApplicationServices { get; set; }

        [Import]
        protected ISkinApplicationServices SkinApplicationServices { get; set; }

        [Import]
        protected IAccountApplicationServices AccountApplicationServices { get; set; }

        [Import]
        protected ISecurityApplicationServices SecurityApplicationServices { get; set; }


        /// <summary>
        /// 当前登陆的账户
        /// </summary>
        protected static AccountDto Account { get; set; }

        /// <summary>
        /// 当前用户的配置
        /// </summary>
        protected static ConfigDto AccountConfig { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var controllerName = filterContext.RequestContext.RouteData.Values["controller"].ToString();

            if (controllerName == "Index")
            {
                return;
            }

            var actionName = filterContext.RequestContext.RouteData.Values["action"].ToString();

            var isPermission = SecurityApplicationServices.CheckPermissions(Account.Id, actionName);

            if (!isPermission)
            {
                filterContext.Result = new JsonResult
                {
                    ContentEncoding = Encoding.UTF8,
                    Data = new { js = @"<script>setTimeout(function(){window.top.ZENG.msgbox.show('对不起，您没有访问的权限！', 5,3000);},0)</script>" }
                };
            }
        }
    }
}
