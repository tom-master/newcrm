using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewLib.Data.Mapper.InternalDataStore;

namespace NewCRM.Domain.Services.BoundedContext
{
	public class LoggerContext: BaseServiceContext, ILoggerContext
	{
		public async Task AddLoggerAsync(Log log)
		{
			Parameter.Validate(log);
			await Task.Run(() =>
			{
				using (var dataStore = new DataStore())
				{
					var sql = $@"INSERT dbo.Log
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
						VALUES  ( @LogLevelEnum , -- LogLevelEnum - int
								  @Controller , -- Controller - nvarchar(max)
								  @Action , -- Action - nvarchar(max)
								  @ExceptionMessage , -- ExceptionMessage - nvarchar(max)
								  @Track , -- Track - nvarchar(max)
								  @AccountId , -- AccountId - int
								  0 , -- IsDeleted - bit
								  GETDATE() , -- AddTime - datetime
								  GETDATE()  -- LastModifyTime - datetime
								)";
					var parameters = new List<SqlParameter>
					  {
							new SqlParameter("@LogLevelEnum",(Int32)log.LogLevelEnum),
							new SqlParameter("@Controller",log.Controller),
							new SqlParameter("@Action",log.Action),
							new SqlParameter("@ExceptionMessage",log.ExceptionMessage),
							new SqlParameter("@Track",log.Track),
							new SqlParameter("@AccountId",log.AccountId),
					  };
					dataStore.SqlExecute(sql, parameters);
				}
			});
		}

		public IList<Log> GetLogs(Int32 accountId, Int32 logLevel, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
		{
			using (var dataStore = new DataStore())
			{
				var where = new StringBuilder();
				var parameters = new List<SqlParameter>();
				if (accountId != 0)
				{
					parameters.Add(new SqlParameter("AccountId", accountId));
					where.Append($@" AND a.AccountId=@AccountId");
				}
				#region totalCount
				{
					var sql = $@"SELECT COUNT(*) FROM dbo.Log AS a WHERE 1=1 {where}";
					totalCount = dataStore.FindSingleValue<Int32>(sql, parameters);
				}
				#endregion

				#region sql
				{
					var sql = $@"SELECT TOP (@pageSize) * FROM 
                                (
	                                SELECT
	                                ROW_NUMBER() OVER(ORDER BY a.Id DESC) AS rownumber, 
	                                a.LogLevelEnum,
	                                a.Controller,
	                                a.Action,
	                                a.ExceptionMessage,
	                                a.Track
	                                FROM dbo.Log AS a WHERE 1=1 {where}
                                ) AS aa WHERE aa.rownumber>@pageSize*(@pageIndex-1)";
					parameters.Add(new SqlParameter("@pageIndex", pageIndex));
					parameters.Add(new SqlParameter("@pageSize", pageSize));
					return dataStore.Find<Log>(sql, parameters);
				}
				#endregion
			}
		}
	}
}
