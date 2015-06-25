using System;
using System.Text;
using System.Web.Mvc;
using System.Threading;
namespace NewCRM.Web
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class GetException : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
             //GetMessageOfException(filterContext.Exception);
        }

        private void GetMessageOfException(Exception ex)
        {
            StringBuilder exceptionStr = new StringBuilder();
            Thread t = new Thread(() =>
            {
                while (true)
                {
                    if (ex.InnerException != null)
                    {
                        exceptionStr.AppendFormat("消息类型：{0}", ex.InnerException.GetType().Name);
                        exceptionStr.AppendFormat("消息内容：{0}", ex.InnerException.Message);
                        exceptionStr.AppendFormat("发出此消息的位置：{0}", ex.InnerException.TargetSite.Name);
                        exceptionStr.AppendFormat("此消息的调用堆栈：{0}", ex.InnerException.StackTrace);
                        GetMessageOfException(ex);
                    }
                    else
                    {
                        exceptionStr.AppendFormat("消息类型：{0}", ex.GetType().Name);
                        exceptionStr.AppendFormat("消息内容：{0}", ex.Message);
                        exceptionStr.AppendFormat("发出此消息的位置：{0}", ex.TargetSite.Name);
                        exceptionStr.AppendFormat("此消息的调用堆栈：{0}", ex.StackTrace);
                    }
                }
            });
            t.Start();
        }
    }
}