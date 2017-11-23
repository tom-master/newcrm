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
                    where.Append($@" a.AccountId={accountId}");
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

                        filter.And(app => app.AppReleaseState == appReleaseState);
                    }

                    //app应用审核状态
                    if(stats[0] == "AppAuditState")
                    {
                        var appAuditState = EnumExtensions.ParseToEnum<AppAuditState>(Int32.Parse(stats[1]));

                        filter.And(app => app.AppAuditState == appAuditState);
                    }
                }

                #endregion
            }
        }
    }
}
