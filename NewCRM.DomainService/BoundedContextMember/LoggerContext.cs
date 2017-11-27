using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.Repository.StorageProvider;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    public class LoggerContext : BaseServiceContext, ILoggerContext
    {
        public void AddLogger(Log log)
        {
            ValidateParameter.Validate(log);
            using(var dataStore = new DataStore())
            {
                var sql = $@"INSERT dbo.Logs
                            ( LogLevelEnum ,
                              Controller ,
                              Action ,
                              ExceptionMessage ,
                              Track ,
                              AccountId ,
                              IsDeleted ,
                              AddTime ,
                              LastModifyTime
                            )
                    VALUES  ( {(Int32)log.LogLevelEnum} , -- LogLevelEnum - int
                              N'{log.Controller}' , -- Controller - nvarchar(max)
                              N'{log.Action}' , -- Action - nvarchar(max)
                              N'{log.ExceptionMessage}' , -- ExceptionMessage - nvarchar(max)
                              N'{log.Track}' , -- Track - nvarchar(max)
                              {log.AccountId} , -- AccountId - int
                              0 , -- IsDeleted - bit
                              GETDATE() , -- AddTime - datetime
                              GETDATE()  -- LastModifyTime - datetime
                            )";
                dataStore.SqlExecute(sql);
            }
        }

        public IList<Log> GetLogs(int accountId, int logLevel, int pageIndex, int pageSize, out int totalCount)
        {
            using(var dataStore = new DataStore())
            {
                var where = new StringBuilder();
                if(accountId != 0)
                {
                    where.Append($@" AND a.AccountId={accountId}");
                }
                #region totalCount
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.Logs AS a WHERE 1=1 {where}";
                    totalCount = (Int32)dataStore.SqlScalar(sql);
                }
                #endregion

                #region sql
                {
                    var sql = $@"SELECT TOP {pageSize} * FROM 
                                (
	                                SELECT
	                                ROW_NUMBER() OVER(ORDER BY a.Id DESC) AS rownumber, 
	                                a.LogLevelEnum,
	                                a.Controller,
	                                a.Action,
	                                a.ExceptionMessage,
	                                a.Track
	                                FROM dbo.Logs AS a WHERE 1=1 {where}
                                ) AS aa WHERE aa.rownumber>{pageSize}*({pageIndex}-1)";
                    return dataStore.SqlGetDataTable(sql).AsList<Log>().ToList();
                }
                #endregion
            }
        }
    }
}
