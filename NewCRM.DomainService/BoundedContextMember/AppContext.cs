using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.Repository.StorageProvider;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    public class AppContext : BaseServiceContext, IAppContext
    {
        public List<App> GetApps(int accountId, int appTypeId, int orderId, string searchText, int pageIndex, int pageSize, out int totalCount)
        {
            ValidateParameter.Validate(accountId, true).Validate(orderId).Validate(searchText).Validate(pageIndex, true).Validate(pageSize);
            using(var dataStore = new DataStore())
            {


                var where = new StringBuilder();
                where.Append($@" WHERE 1=1 AND a.IsDeleted=0 AND a.AppAuditState={AppAuditState.Pass} AND a.AppReleaseState={AppReleaseState.Release} ");

                var orderBy = new StringBuilder();
                if(appTypeId != 0 && appTypeId != -1)//全部app
                {
                    where.Append($@" AND a.AppTypeId={appTypeId}");
                }
                else
                {
                    if(appTypeId == -1)//用户制作的app
                    {
                        where.Append($@" AND a.AccountId={accountId}");
                    }
                }
                if(!String.IsNullOrEmpty(searchText))//关键字搜索
                {
                    where.Append($@" AND a.Name LIKE '%{searchText}%'");
                }

                switch(orderId)
                {
                    case 1:
                        {
                            orderBy.Append($@" ORDER BY aa.AddTime DESC");
                            break;
                        }
                    case 2:
                        {
                            orderBy.Append($@" ORDER BY aa.UseCount DESC");
                            break;
                        }
                    case 3:
                        {

                            orderBy.Append($@" ORDER BY aa.AppStars DESC");
                            //filter.OrderByDescending(app => app.AppStars.Sum(s => s.StartNum) * 1.0);
                            break;
                        }
                }

                #region totalCount
                {
                    var sql = $@"SELECT 
	                        ROW_NUMBER() OVER(ORDER BY a.Id DESC) AS rownumber,
	                        a.AppTypeId,
	                        a.AccountId,
	                        a.AddTime,
	                        a.UseCount,
	                        (
		                        SELECT SUM(a1.StartNum) FROM dbo.AppStars AS a1 WHERE a1.AccountId=0 AND a1.AppId=0 AND a1.IsDeleted=0
	                        ) AS StartCount,
	                        a.Name,
	                        a.IconUrl,
	                        a.Remark,
	                        a.AppStyle,
	                        a.Id
	                        FROM dbo.Apps AS a {where}";
                    totalCount = (Int32)dataStore.SqlScalar(sql);
                }
                #endregion

                #region sql
                {
                    var sql = $@"SELECT TOP {pageSize} * FROM 
                        (
	                        SELECT 
	                        ROW_NUMBER() OVER(ORDER BY a.Id DESC) AS rownumber,
	                        a.AppTypeId,
	                        a.AccountId,
	                        a.AddTime,
	                        a.UseCount,
	                        (
		                        SELECT SUM(a1.StartNum) FROM dbo.AppStars AS a1 WHERE a1.AccountId={accountId} AND a1.AppId=0 AND a1.IsDeleted=0
	                        ) AS StartCount,
	                        a.Name,
	                        a.IconUrl,
	                        a.Remark,
	                        a.AppStyle,
	                        a.Id,
	(
		SELECT COUNT(*) FROM dbo.Members AS a1 WHERE a1.AccountId={accountId} AND a1.IsDeleted=0
	) AS IsInstall
	                        FROM dbo.Apps AS a {where}
                        ) AS aa WHERE aa.rownumber>{pageSize}*({pageIndex}-1)  {orderBy}";

                    return dataStore.SqlGetDataTable(sql).AsList<App>().ToList();
                }
                #endregion
            }
        }

        public List<App> GetAccountApps(Int32 accountId, String searchText, Int32 appTypeId, Int32 appStyleId, String appState, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(accountId, true).Validate(searchText).Validate(appTypeId, true).Validate(appStyleId, true).Validate(pageIndex).Validate(pageSize);

            using(var dataStore = new DataStore())
            {
                var where = new StringBuilder();
                where.Append($@" WHERE 1=1 ");

                #region 条件筛选

                if(accountId != default(Int32))
                {
                    where.Append($@" AND a.AccountId={accountId}");
                }

                //应用名称
                if(!String.IsNullOrEmpty(searchText))
                {
                    where.Append($@" AND a.Name LIKE '%{searchText}%'");
                }

                //应用所属类型
                if(appTypeId != 0)
                {
                    where.Append($@" AND a.AppTypeId={appTypeId}");
                }

                //应用样式
                if(appStyleId != 0)
                {
                    var appStyle = EnumExtensions.ParseToEnum<AppStyle>(appStyleId);
                    where.Append($@" AND a.AppStyle={appStyle}");
                }

                if((appState + "").Length > 0)
                {
                    //app发布状态
                    var stats = appState.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if(stats[0] == "AppReleaseState")
                    {
                        var appReleaseState = EnumExtensions.ParseToEnum<AppReleaseState>(Int32.Parse(stats[1]));
                        where.Append($@" AND a.AppReleaseState={appReleaseState} ");
                    }

                    //app应用审核状态
                    if(stats[0] == "AppAuditState")
                    {
                        var appAuditState = EnumExtensions.ParseToEnum<AppAuditState>(Int32.Parse(stats[1]));
                        where.Append($@" AND a.AppAuditState={appAuditState}");
                    }
                }

                #endregion

                #region totalCount
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.Apps AS a {where} ";
                    totalCount = (Int32)dataStore.SqlScalar(sql);
                }
                #endregion

                #region sql
                {
                    var sql = $@"SELECT TOP {pageSize} * FROM 
                    (
	                    SELECT
	                    ROW_NUMBER() OVER(ORDER BY a.Id DESC) AS rownumber,
	                    a.Name,
	                    a.AppStyle,
	                    a.UseCount,
	                    a.Id,
	                    a.IconUrl,
	                    a.AppAuditState,
	                    a.IsRecommand
	                    FROM dbo.Apps AS a {where} 
                    ) AS aa WHERE aa.rownumber>{pageSize}*({pageIndex}-1)";
                    return dataStore.SqlGetDataTable(sql).AsList<App>().ToList();
                }
                #endregion
            }
        }

        public App GetApp(Int32 appId)
        {
            ValidateParameter.Validate(appId);
            using(var dataStore = new DataStore())
            {
                var sql = $@"SELECT 
                            a.Name,
                            a.IconUrl,
                            a.Remark,
                            a.UseCount,
                            (
	                            SELECT SUM(a1.StartNum) FROM dbo.AppStars AS a1 WHERE a1.AppId={appId} AND a1.IsDeleted=0
                            ) AS StarCount,
                            a.AddTime,
                            a.AccountId,
                            a.Id,
                            a.IsResize,
                            a.IsOpenMax,
                            a.IsFlash,
                            a.AppStyle,
                            a.AppUrl,
                            a.Width,
                            a.Height,
                            a.AppAuditState,
                            a.AppReleaseState,
                            a.AppTypeId
                            FROM dbo.Apps AS a WHERE a.Id={appId} AND a.IsDeleted=0";
                return dataStore.SqlGetDataTable(sql).AsSignal<App>();
            }
        }

        public bool IsInstallApp(int accountId, int appId)
        {
            ValidateParameter.Validate(accountId).Validate(appId);
            using(var dataStore = new DataStore())
            {
                var sql = $@"
SELECT COUNT(*) FROM dbo.Members AS a WHERE a.AppId={appId} AND a.AccountId={accountId} AND a.IsDeleted=0";
                return (Int32)dataStore.SqlScalar(sql) > 0 ? true : false;
            }
        }

        public List<App> GetSystemApp(IEnumerable<int> appIds = null)
        {
            using(var dataStore = new DataStore())
            {
                var where = new StringBuilder();
                where.Append(" WHERE 1=1 a.IsSystem=1 AND a.IsDeleted=0");
                if(appIds.Any())
                {
                    where.Append($@" AND a.Id IN({String.Join(",", appIds)})");
                }

                var sql = $@"SELECT a.Id,a.Name,a.IconUrl FROM dbo.Apps AS a {where}";
                return dataStore.SqlGetDataTable(sql).AsList<App>().ToList();
            }
        }

        public void ModifyAppStar(int accountId, int appId, int starCount)
        {
            ValidateParameter.Validate(accountId).Validate(appId).Validate(starCount);

            using(var dataStore = new DataStore())
            {
                var sql = $@"INSERT dbo.AppStars
                            ( AccountId ,
                              StartNum ,
                              IsDeleted ,
                              AddTime ,
                              LastModifyTime ,
                              AppId
                            )
                    VALUES  ( {accountId} , -- AccountId - int
                              {starCount} , -- StartNum - float
                              0 , -- IsDeleted - bit
                              GETDATE() , -- AddTime - datetime
                              GETDATE() , -- LastModifyTime - datetime
                              {appId}  -- AppId - int
                            )";
                dataStore.SqlExecute(sql);
            }
        }
    }
}
