using System;
using System.Web;
using System.Configuration;
using System.Reflection;
using NewCRM.Common;

namespace NewCRM.Web.RequestProcess
{
    /// <summary>
    /// ProcessHandler 的摘要说明
    /// </summary>
    public class ProcessHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            String page = ConfigurationManager.AppSettings["PAGE"];
            String codeFile = context.Request.Params["codeFile"];
            Type type = Assembly.Load(page).GetType(page + "." + codeFile);
            dynamic dynamicDelegate = ProcessHtmlRequest.ExcutionOperate(type, context.Request.Params["method"]);

        }


        private String[] GetParameter(HttpRequest req, MethodInfo methodInfo)
        {
            String[] paraArr = null;
            ParameterInfo[] parameters = methodInfo.GetParameters();
            if (parameters.Length > 0)
            {
                paraArr = new string[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    paraArr[i] = req.Params[parameters[i].Name];
                }
            }
            return paraArr;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}