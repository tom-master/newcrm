using System;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using NewCRM.Application.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Web.Filter
{
	public sealed class ErrorFilter: IExceptionFilter
	{
		public void OnException(ExceptionContext filterContext)
		{
			filterContext.ExceptionHandled = true;

			var isAjaxRequest = filterContext.RequestContext.HttpContext.Request.IsAjaxRequest();
			var exception = filterContext.Exception is BusinessException;
			if (isAjaxRequest)
			{
				filterContext.Result = new JsonResult
				{
					Data = new ResponseModel<String>
					{
						IsSuccess = false,
						Message = exception ? filterContext.Exception.Message : "出现未知错误，请重试",
					},
					ContentEncoding = Encoding.UTF8,
					JsonRequestBehavior = JsonRequestBehavior.AllowGet
				};
			}
			else
			{
				filterContext.Result = new ContentResult
				{
					ContentEncoding = Encoding.UTF8,
					Content = $@"<script>top.alertInfo('出现未知错误，请重试')</script>"
				};
			}

			DependencyResolver.Current.GetService<ILoggerServices>().AddLoggerAsync(new LogDto
			{
				Action = filterContext.RouteData.Values["action"].ToString(),
				Controller = filterContext.RouteData.Values["controller"].ToString(),
				ExceptionMessage = filterContext.Exception.Message,
				Track = filterContext.Exception.StackTrace,
				LogLevelEnum = exception ? LogLevel.Warning : LogLevel.Error,
				Id = new Random().Next(1, Int32.MaxValue),
				AddTime = DateTime.Now.ToString(CultureInfo.CurrentCulture)
			});


		}
	}
}