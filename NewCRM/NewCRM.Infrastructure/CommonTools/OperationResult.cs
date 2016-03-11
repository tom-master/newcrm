using System;

namespace NewCRM.Infrastructure.CommonTools
{
    public sealed class ResponseInformation
    {
        #region 构造函数

        public ResponseInformation(ResponseType responseType)
        {
            ResultType = responseType;
        }

        public ResponseInformation(ResponseType responseType, String message)
            : this(responseType)
        {
            Message = message;
        }

        public ResponseInformation(ResponseType responseType, String message, dynamic appendData)
            : this(responseType, message)
        {
            Data = appendData;
        }



        public ResponseInformation(ResponseType responseType, String message, String logMessage, dynamic appendData)
            : this(responseType, message, logMessage)
        {
            Data = appendData;
        }

        #endregion

        #region 属性

        public ResponseType ResultType { get; set; }

        public String Message { get; set; }

        public dynamic Data { get; set; }

        #endregion
    }
}