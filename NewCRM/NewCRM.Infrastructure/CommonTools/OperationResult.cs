using System;
using NewCRM.Infrastructure.CommonTools.CustomExtension;

namespace NewCRM.Infrastructure.CommonTools
{
    public sealed class ResponseInformation<T>
    {
        #region 构造函数


        public ResponseInformation(ResponseType responseType)
        {
            ResultType = responseType;
            Message = responseType.GetDescription();
        }

        public ResponseInformation(ResponseType responseType, T appendData) : this(responseType)
        {
            Data = appendData;
        }

        #endregion

        #region 属性

        public ResponseType ResultType { get; set; }

        public String Message { get; set; }

        public T Data { get; set; }

        #endregion
    }
}