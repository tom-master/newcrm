using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewCRM.Web.Filter
{
    public sealed class ErrorFilterAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            //filterContext.HttpContext.Response.Output.Write("<script>ZENG.msgbox.show(" + filterContext.Exception.Message + ", 1, 2000);</script>");
            base.OnException(filterContext);
        }
    }
}