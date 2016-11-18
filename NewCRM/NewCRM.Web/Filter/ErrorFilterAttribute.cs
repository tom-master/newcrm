using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace NewCRM.Web.Filter
{
    public sealed class ErrorFilterAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
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

            exceptionMessage = Regex.Replace(exceptionMessage, @"[\r\n]", "");

            if (filterContext.RequestContext.HttpContext.Request.HttpMethod.ToLower() != "post")
            {
                filterContext.Result = new ContentResult()
                {
                    Content = @"<script>setTimeout(function(){window.top.ZENG.msgbox.show('" + exceptionMessage + "', 5,3000);},0)</script>",
                    ContentEncoding = Encoding.UTF8
                };
            }
            else
            {
                filterContext.Result = new JsonResult()
                {
                    ContentEncoding = Encoding.UTF8,
                    Data = new { js = @"<script>setTimeout(function(){window.top.ZENG.msgbox.show('" + exceptionMessage + "', 5,3000);},0)</script>" }
                };
            }
        }
    }
}