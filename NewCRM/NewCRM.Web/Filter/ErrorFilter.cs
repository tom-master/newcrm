using System;
using System.Text;
using System.Web.Mvc;
using NewCRM.Application.Interface;
using NewCRM.Dto.Dto;

namespace NewCRM.Web.Filter
{
    public sealed class ErrorFilter : IExceptionFilter
    {

        public void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            var logger = new LogDto
            {
                Action = filterContext.RouteData.Values["action"].ToString(),
                Controller = filterContext.RouteData.Values["controller"].ToString(),
                ExceptionMessage = filterContext.Exception.Message.Length > 20 ? "操作失败，请查看日志" : filterContext.Exception.Message,
                Track = filterContext.Exception.StackTrace,
                LogLevelEnum = 4,
                Id = new Random().Next(1, Int32.MaxValue)
            };

            DependencyResolver.Current.GetService<ILoggerApplicationServices>().AddLogger(logger);

            if (filterContext.RequestContext.HttpContext.Request.HttpMethod.ToLower() != "post")
            {
                filterContext.Result = new ContentResult
                {
                    Content = @"<script>setTimeout(function(){window.top.ZENG.msgbox.show('操作失败，请查看日志', 5,3000);},0)</script>",
                    ContentEncoding = Encoding.UTF8
                };
            }
            else
            {
                filterContext.Result = new JsonResult
                {
                    ContentEncoding = Encoding.UTF8,
                    Data = new { js = @"<script>setTimeout(function(){window.top.ZENG.msgbox.show('操作失败，请查看日志', 5,3000);},0)</script>" }
                };
            }
        }
    }
}