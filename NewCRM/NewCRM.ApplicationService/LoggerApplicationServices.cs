using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Application.Interface;
using NewCRM.Domain;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Factory.DomainQuery.Query;
using NewCRM.Domain.ValueObject;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustomExtension;

namespace NewCRM.Application.Services
{
    internal class LoggerApplicationServices : BaseServiceContext, ILoggerApplicationServices
    {
        private readonly QueryBase _query;

        
        public LoggerApplicationServices(QueryBase query)
        {
            _query = query;
        }

        public void AddLogger(LogDto log)
        {
            log.AccountId = AccountId;

            Repository.Create<Log>().Add(log.ConvertToModel<LogDto, Log>());
        }

        public IList<LogDto> GetAllLog(Int32 logLevel, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            var internalLogLevel = EnumExtensions.ParseToEnum<LogLevel>(logLevel);

            return _query.PageBy(FilterFactory.Create((Log log) => log.LogLevelEnum == internalLogLevel), pageIndex, pageSize, out totalCount).ConvertToDtos<Log, LogDto>().ToList();
        }
    }
}
