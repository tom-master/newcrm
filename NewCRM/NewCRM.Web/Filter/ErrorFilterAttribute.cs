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
            
        }
    }
}