using System;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using System.Text;
using System.Threading;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Dto.Dto;

namespace NewCRM.Web.Controllers.ControllerHelper
{
    public class BaseController : Controller
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



        protected static UserDto CurrentUser { get; set; }

        protected override JsonResult Json(Object data, String contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new Jsons
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            String exceptionMessage = "";

            if (filterContext.Exception.Message.Length > 50)
            {
                exceptionMessage = filterContext.Exception.Message.Substring(0, 50) + "...";
            }
            else
            {
                exceptionMessage = filterContext.Exception.Message;
            }
            filterContext.Result = Content(@"<script>setTimeout(function(){window.top.ZENG.msgbox.show('"+ exceptionMessage + "', 5,3000);},0)</script>");
        }
    }
}
