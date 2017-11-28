using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.Repository.StorageProvider;

namespace NewCRM.Domain.Services.BoundedContext
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

        public void CreateNewApp(App app)
        {
            using(var dataStore = new DataStore())
            {
                var sql = $@"INSERT dbo.Apps
                            (
                                Name,
                                IconUrl,
                                AppUrl,
                                Remark,
                                Width,
                                Height,
                                UseCount,
                                IsMax,
                                IsFull,
                                IsSetbar,
                                IsOpenMax,
                                IsLock,
                                IsSystem,
                                IsFlash,
                                IsDraw,
                                IsResize,
                                AccountId,
                                AppTypeId,
                                IsRecommand,
                                AppAuditState,
                                AppReleaseState,
                                AppStyle,
                                IsDeleted,
                                AddTime,
                                LastModifyTime
                            )
                            VALUES
                            (   N'{app.Name}',       -- Name - nvarchar(6)
                                N'{app.IconUrl}',       -- IconUrl - nvarchar(max)
                                N'{app.AppUrl}',       -- AppUrl - nvarchar(max)
                                N'{app.Remark}',       -- Remark - nvarchar(50)
                                {app.Width},         -- Width - int
                                {app.Height},         -- Height - int
                                0,         -- UseCount - int
                                {app.IsMax},      -- IsMax - bit
                                {app.IsFull},      -- IsFull - bit
                                {app.IsSetbar},      -- IsSetbar - bit
                                {app.IsOpenMax},      -- IsOpenMax - bit
                                0,      -- IsLock - bit
                                {app.IsSystem},      -- IsSystem - bit
                                {app.IsFlash},      -- IsFlash - bit
                                {app.IsDraw},      -- IsDraw - bit
                                {app.IsResize},      -- IsResize - bit
                                {app.AccountId},         -- AccountId - int
                                {app.AppTypeId},         -- AppTypeId - int
                                0,      -- IsRecommand - bit
                                {(Int32)app.AppAuditState},         -- AppAuditState - int
                                {(Int32)AppReleaseState.UnRelease},         -- AppReleaseState - int
                                {app.AppStyle},         -- AppStyle - int
                                0,      -- IsDeleted - bit
                                GETDATE(), -- AddTime - datetime
                                GETDATE()  -- LastModifyTime - datetime
                            )";
                dataStore.SqlExecute(sql);
            }
        }

        public void Pass(Int32 appId)
        {
            ValidateParameter.Validate(appId);
            using(var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Apps SET AppAuditState={AppAuditState.Pass} WHERE Id={appId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void Deny(Int32 appId)
        {
            ValidateParameter.Validate(appId);
            using(var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Apps SET AppAuditState={AppAuditState.Deny} WHERE Id={appId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void SetTodayRecommandApp(Int32 appId)
        {
            ValidateParameter.Validate(appId);
            using(var dataStore = new DataStore())
            {
                dataStore.OpenTransaction();
                try
                {
                    #region 取消之前的推荐app
                    {
                        var sql = $@"UPDATE dbo.Apps SET IsRecommand=0 WHERE IsRecommand=1 AND IsDeleted=0";
                        dataStore.SqlExecute(sql);
                    }
                    #endregion

                    #region 设置新的推荐app
                    {
                        var sql = $@"UPDATE dbo.Apps SET IsRecommand=1 WHERE Id={appId} AND IsDeleted=0";
                        dataStore.SqlExecute(sql);
                    }
                    #endregion

                    dataStore.Commit();
                }
                catch(Exception)
                {
                    dataStore.Rollback();
                    throw;
                }
            }
        }

        public void RemoveApp(Int32 appId)
        {
            ValidateParameter.Validate(appId);
            using(var dataStore = new DataStore())
            {
                dataStore.OpenTransaction();
                try
                {
                    #region 移除app的评分
                    {
                        var sql = $@"UPDATE dbo.AppStars SET IsDeleted=1 WHERE AppId={appId}";
                        dataStore.SqlExecute(sql);
                    }
                    #endregion

                    #region 移除app
                    {
                        var sql = $@"UPDATE dbo.Apps SET IsDeleted=1 WHERE Id={appId}";
                        dataStore.SqlExecute(sql);
                    }
                    #endregion

                    dataStore.Commit();
                }
                catch(Exception)
                {
                    dataStore.Rollback();
                    throw;
                }
            }
        }

        public void ReleaseApp(Int32 appId)
        {
            ValidateParameter.Validate(appId);
            using(var dataStore = new DataStore())
            {
                #region 发布app
                {
                    var sql = $@"UPDATE dbo.Apps SET AppReleaseState={AppReleaseState.Release} WHERE Id={appId} AND IsDeleted=0 AND AppAuditState={AppAuditState.Pass}";
                    dataStore.SqlExecute(sql);
                }
                #endregion
            }
        }

        public void ModifyAccountAppInfo(Int32 accountId, App app)
        {
            ValidateParameter.Validate(accountId).Validate(accountId).Validate(app);
            using(var dataStore = new DataStore())
            {
                var set = new StringBuilder();
                set.Append($@" IconUrl={app.IconUrl},Name={app.Name},AppTypeId={app.AppTypeId},AppUrl={app.AppUrl},Width={app.Width},Height={app.Height},AppStyle={app.AppStyle},IsResize={app.IsResize},IsOpenMax={app.IsOpenMax},IsFlash={app.IsFlash},Remark={app.Remark} ");
                if(app.AppAuditState == AppAuditState.Wait)
                {
                    set.Append($@" ,AppAuditState={(Int32)AppAuditState.Wait} ");
                }
                else
                {
                    set.Append($@" ,AppAuditState={(Int32)AppAuditState.UnAuditState} ");
                }
                dataStore.SqlExecute(set.ToString());
            }
        }

        public void DeleteAppType(Int32 appTypeId)
        {
            ValidateParameter.Validate(appTypeId);
            using(var dataStore = new DataStore())
            {
                #region 前置条件验证
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.Apps AS a WHERE a.AppTypeId={appTypeId} AND a.IsDeleted=0";
                    if((Int32)dataStore.SqlScalar(sql) > 0)
                    {
                        throw new BusinessException($@"当前分类下已有绑定app,不能删除当前分类");
                    }
                }
                #endregion

                #region 移除app分类
                {
                    var sql = $@"UPDATE dbo.AppTypes SET IsDeleted=1 WHERE Id={appTypeId}";
                    dataStore.SqlExecute(sql);
                }
                #endregion
            }
        }

        public void CreateNewAppType(AppType appType)
        {
            ValidateParameter.Validate(appType);
            using(var dataStore = new DataStore())
            {
                #region 前置条件验证
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.AppTypes AS a WHERE a.Name=@name AND a.IsDeleted=0";
                    var result = (Int32)dataStore.SqlScalar(sql, new List<SqlParameter> { new SqlParameter("@name", appType.Name) });
                    if(result > 0)
                    {
                        throw new BusinessException($@"分类:{appType.Name},已存在");
                    }
                }
                #endregion

                #region 添加app分类
                {
                    var sql = $@"INSERT dbo.AppTypes
                                (
                                    Name,
                                    Remark,
                                    IsDeleted,
                                    AddTime,
                                    LastModifyTime
                                )
                                VALUES
                                (   N'{appType.Name}',       -- Name - nvarchar(6)
                                    N'{appType.Remark}',       -- Remark - nvarchar(50)
                                    0,      -- IsDeleted - bit
                                    GETDATE(), -- AddTime - datetime
                                    GETDATE()  -- LastModifyTime - datetime
                                )";
                    dataStore.SqlExecute(sql);
                }
                #endregion
            }
        }

        public void ModifyAppType(String appTypeName, Int32 appTypeId)
        {
            ValidateParameter.Validate(appTypeName).Validate(appTypeId);
            using(var dataStore = new DataStore())
            {
                #region 前置条件验证
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.AppTypes AS a WHERE a.Name=@name AND a.IsDeleted=0";
                    var result = (Int32)dataStore.SqlScalar(sql, new List<SqlParameter> { new SqlParameter("@name", appTypeName) });
                    if(result > 0)
                    {
                        throw new BusinessException($@"分类:{appTypeName},已存在");
                    }
                }
                #endregion

                #region 更新app分类
                {
                    var sql = $@"UPDATE dbo.AppTypes SET Name=@name WHERE Id={appTypeId} AND IsDeleted=0";
                    dataStore.SqlExecute(sql, new List<SqlParameter> { new SqlParameter("@name", appTypeName) });
                }
                #endregion
            }
        }

        public void ModifyAppIcon(Int32 accountId, Int32 appId, String newIcon)
        {
            ValidateParameter.Validate(accountId).Validate(appId).Validate(newIcon);
            using(var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Apps SET IconUrl=@url WHERE Id={appId} AND AccountId={accountId} AND AppAuditState={(Int32)AppAuditState.Pass} AND AppReleaseState={(Int32)AppReleaseState.Release} AND IsDeleted=0";

                dataStore.SqlExecute(sql, new List<SqlParameter> { new SqlParameter("@url", newIcon) });
            }
        }

        public Tuple<Int32, Int32> GetAccountDevelopAppCountAndNotReleaseAppCount(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);
            using(var dataStore = new DataStore())
            {
                var sql = $@"SELECT a.Id FROM dbo.Apps AS a WHERE a.AccountId={accountId} AND a.IsDeleted=0";
                var result = dataStore.SqlGetDataTable(sql).AsList<App>();
                return new Tuple<int, int>(result.Count, result.Count(a => a.AppReleaseState == AppReleaseState.UnRelease));
            }
        }

        public void Install(Int32 accountId, Int32 appId, Int32 deskNum)
        {
            ValidateParameter.Validate(accountId).Validate(appId).Validate(deskNum);

            using(var dataStore = new DataStore())
            {
                dataStore.OpenTransaction();
                try
                {
                    App app = null;
                    #region 获取app
                    {
                        var sql = $@"SELECT
                                a.Name,
                                a.IconUrl,
                                a.AppUrl,
                                a.Id,
                                a.Width,
                                a.Height,
                                a.IsLock,
                                a.IsMax,
                                a.IsFull,
                                a.IsSetbar,
                                a.IsOpenMax,
                                a.IsFlash,
                                a.IsDraw
                                FROM  dbo.Apps AS a WHERE a.AppAuditState={AppAuditState.Pass} AND a.AppReleaseState={AppReleaseState.Release} AND a.IsDeleted=0 AND a.Id={appId}";
                        app = dataStore.SqlGetDataTable(sql).AsSignal<App>();
                    }
                    #endregion

                    if(app == null)
                    {
                        throw new BusinessException($"应用添加失败，请刷新重试");
                    }

                    #region 添加桌面成员
                    {
                        var newMember = new Member(app.Name, app.IconUrl, app.AppUrl, app.Id, app.Width, app.Height, app.IsLock, app.IsMax, app.IsFull, app.IsSetbar, app.IsOpenMax, app.IsFlash, app.IsDraw, app.IsResize);

                        var sql = $@"INSERT dbo.Members
                            ( AppId ,
                              Width ,
                              Height ,
                              FolderId ,
                              Name ,
                              IconUrl ,
                              AppUrl ,
                              IsOnDock ,
                              IsMax ,
                              IsFull ,
                              IsSetbar ,
                              IsOpenMax ,
                              IsLock ,
                              IsFlash ,
                              IsDraw ,
                              IsResize ,
                              MemberType ,
                              IsDeleted ,
                              AddTime ,
                              LastModifyTime ,
                              AccountId ,
                              DeskIndex
                            )
                    VALUES  ( {newMember.AppId} , -- AppId - int
                              {newMember.Width} , -- Width - int
                              {newMember.Height} , -- Height - int
                              0 , -- FolderId - int
                              N'{newMember.Name}' , -- Name - nvarchar(6)
                              N'{newMember.IconUrl}' , -- IconUrl - nvarchar(max)
                              N'{newMember.AppUrl}' , -- AppUrl - nvarchar(max)
                              0 , -- IsOnDock - bit
                              {newMember.IsMax} , -- IsMax - bit
                              {newMember.IsFull} , -- IsFull - bit
                              {newMember.IsSetbar} , -- IsSetbar - bit
                              {newMember.IsOpenMax} , -- IsOpenMax - bit
                              {newMember.IsLock} , -- IsLock - bit
                              {newMember.IsFlash} , -- IsFlash - bit
                              {newMember.IsDraw} , -- IsDraw - bit
                              {newMember.IsResize} , -- IsResize - bit
                              {newMember.MemberType} , -- MemberType - int
                              0 , -- IsDeleted - bit
                              GETDATE() , -- AddTime - datetime
                              GETDATE() , -- LastModifyTime - datetime
                              {accountId} , -- AccountId - int
                              {deskNum}  -- DeskIndex - int
                            )";
                    }
                    #endregion

                    #region 更改app使用数量
                    {
                        var sql = $@"UPDATE dbo.Apps SET UseCount=UseCount+1 WHERE Id={app.Id} AND IsDeleted=0";
                        dataStore.SqlExecute(sql);
                    }
                    #endregion

                    dataStore.Commit();
                }
                catch(Exception)
                {
                    dataStore.Rollback();
                    throw;
                }
            }
        }

        public List<AppType> GetAppTypes()
        {
            using(var dataStore = new DataStore())
            {
                var sql = $@"SELECT a.Id,a.Name FROM dbo.AppTypes AS a WHERE a.IsDeleted=0";
                return dataStore.SqlGetDataTable(sql).AsList<AppType>().ToList();
            }
        }
    }
}
