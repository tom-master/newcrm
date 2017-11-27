using System;
using System.Collections.Generic;
using NewCRM.Dto;

namespace NewCRM.Application.Services.Interface
{
    public interface ILoggerServices
    {
        void AddLogger(LogDto log);

        IList<LogDto> GetLogs(Int32 accountId, Int32 logLevel, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);
    }
}
