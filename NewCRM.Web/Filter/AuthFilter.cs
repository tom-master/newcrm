using NewCRM.Application.Services.Interface;
using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace NewCRM.Web.Filter
{
    public class AuthFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var actionName = filterContext.RequestContext.RouteData.Values["action"].ToString().ToLower();
            var controllerName = filterContext.RequestContext.RouteData.Values["controller"].ToString().ToLower();
            if ((controllerName == "login" && actionName == "index") || actionName == "landing" || actionName == "desktop")
            {
                return;
            }

            if (filterContext.HttpContext.Request.Cookies["memberID"] == null)
            {
                ReturnMessage(filterContext, "登陆超时，请刷新页面后重新登陆");
                return;
            }

            if (actionName != "createwindow")
            {
                return;
            }
            //文件夹
            if (filterContext.RequestContext.HttpContext.Request.Form["type"] == "folder")
            {
                return;
            }
            var account = DependencyResolver.Current.GetService<IAccountServices>().GetAccount(Int32.Parse(filterContext.HttpContext.Request.Cookies["memberID"].Value));
            var appId = Int32.Parse(filterContext.RequestContext.HttpContext.Request.Form["id"]);
            var isPermission = DependencyResolver.Current.GetService<ISecurityServices>().CheckPermissions(appId, account.Roles.Select(role => role.Id).ToArray());

            if (!isPermission)
            {
                ReturnMessage(filterContext, "对不起，您没有访问的权限！");
            }
        }

        private static void ReturnMessage(AuthorizationContext filterContext, String message)
        {
            var notPermissionMessage = $@"<script>window.parent.alertInfo('{message}')</script>";
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