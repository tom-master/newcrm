using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Application.Services.Interface;
using NewCRM.Domain;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Dto;

namespace NewCRM.Application.Services
{
    public class LoggerServices : BaseServiceContext, ILoggerServices
    {
        private ILoggerContext _loggerContext;

        public LoggerServices(ILoggerContext loggerContext)
        {
            _loggerContext = loggerContext;
        }

        public void AddLogger(LogDto log)
        {
            ValidateParameter.Validate(log);
            _loggerContext.AddLogger(log.ConvertToModel<LogDto, Log>());
        }

        public IList<LogDto> GetAllLog(Int32 accountId, Int32 logLevel, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            var result = _loggerContext.GetLogs(accountId, logLevel, pageIndex, pageSize, out totalCount);
            return result.Select(s => new LogDto
            {
                AccountId = s.AccountId,
                Action = s.Action,
                AddTime = s.AddTime.ToString("yyyy-MM-dd"),
                Controller = s.Controller,
                ExceptionMessage = s.ExceptionMessage,
                Id = s.Id,
                LogLevelEnum = (Int32)s.LogLevelEnum,
                Track = s.Track
            }).ToList();
        }
    }
}
