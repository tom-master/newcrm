using System;
using NewCRM.Infrastructure.CommonTools.CustomExtension;

namespace NewCRM.Infrastructure.CommonTools
{
	public abstract class ResponseModel<T>
	{
		protected ResponseModel()
		{
			ResponseType = ResponseType.Fail;
			Message = ResponseType.Fail.GetDescription();
			Model = default(T);
		}

		public ResponseType ResponseType { get; set; }

		public String Message { get; set; }

		public T Model { get; set; }
	}

	public class RepsonseModels<T> : ResponseModel<T>
	{
		public Int32 TotalCount { get; set; }
	}
}