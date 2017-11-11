using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.ValueObject;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.Application.Services.Interface;
using NewCRM.Domain.Repositories.IRepository.System;

namespace NewCRM.Application.Services
{
    public class LoggerServices : BaseServiceContext, ILoggerServices
    {
        private readonly ILogRepository _loggerRepository;

        public LoggerServices(ILogRepository loggerRepository)
        {
            _loggerRepository = loggerRepository;
        }

        public void AddLogger(LogDto log)
        {
            _loggerRepository.Add(log.ConvertToModel<LogDto, Log>());
        }

        public IList<LogDto> GetAllLog(Int32 logLevel, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            var internalLogLevel = EnumExtensions.ParseToEnum<LogLevel>(logLevel);
            return DatabaseQuery.PageBy(FilterFactory.Create((Log log) => log.LogLevelEnum == internalLogLevel), pageIndex, pageSize, out totalCount).ConvertToDtos<Log, LogDto>().ToList();
        }
    }
}
