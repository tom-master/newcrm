using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NewCRM.Application.Services.Interface;

namespace NewCRM.Web.Filter
{
    public class AuthFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var actionName = filterContext.RequestContext.RouteData.Values["action"].ToString();
            if (actionName != "CreateWindow")
            {
                return;
            }
            if (filterContext.HttpContext.Request.Cookies["Account"] == null)
            {
                ReturnMessage(filterContext, "登陆超时，请刷新页面后重新登陆");
                return;
            }
            //文件夹
            if (filterContext.RequestContext.HttpContext.Request.Form["type"] == "folder")
            {
                return;
            }
            var account = DependencyResolver.Current.GetService<IAccountServices>().GetAccount();
            var appId = Int32.Parse(filterContext.RequestContext.HttpContext.Request.Form["id"]);
            var isPermission = DependencyResolver.Current.GetService<ISecurityApplicationServices>().CheckPermissions(appId, account.Roles.Select(role => role.Id).ToArray());

            if (!isPermission)
            {
                ReturnMessage(filterContext, "对不起，您没有访问的权限！");
            }
        }

        private static void ReturnMessage(AuthorizationContext filterContext, String message)
        {
            var notPermissionMessage = @"<script>setTimeout(function(){window.top.ZENG.msgbox.show('" + message + "', 5,3000);},0)</script>";
            var isAjaxRequest = filterContext.RequestContext.HttpContext.Request.IsAjaxRequest();

            if (!isAjaxRequest)
            {
                filterContext.Result = new ContentResult
                {
                    ContentEncoding = Encoding.UTF8,
                    Content = notPermissionMessage
                };
            }
            else
            {
                filterContext.Result = new JsonResult
                {
                    ContentEncoding = Encoding.UTF8,
                    Data = new
                    {
                        js = notPermissionMessage
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
    }
}