using System;
using System.Web.Mvc;
using System.Text;
using NewCRM.Dto.Dto;

namespace NewCRM.Web.Controllers.ControllerHelper
{
    public class BaseController : Controller
    {
        public static UserDto CurrentUser { get; set; }

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
            filterContext.Result = JavaScript("ZENG.msgbox.show('" + filterContext.Exception.Message + "', 5, 3000);");
        }
    }
}
