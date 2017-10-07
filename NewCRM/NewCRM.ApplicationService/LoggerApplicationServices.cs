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

namespace NewCRM.Application.Services
{
	public class LoggerApplicationServices : BaseServiceContext, ILoggerApplicationServices
    {
        public void AddLogger(Int32 accountId,LogDto log)
        {
            log.AccountId = accountId;
            Repository.Create<Log>().Add(log.ConvertToModel<LogDto, Log>());
        }

        public IList<LogDto> GetAllLog(Int32 logLevel, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            var internalLogLevel = EnumExtensions.ParseToEnum<LogLevel>(logLevel);
            return DatabaseQuery.PageBy(FilterFactory.Create((Log log) => log.LogLevelEnum == internalLogLevel), pageIndex, pageSize, out totalCount).ConvertToDtos<Log, LogDto>().ToList();
        }
    }
}
