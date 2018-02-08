using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Dto;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewLib;
using NewLib.Data.SqlMapper.InternalDataStore;

namespace NewCRM.Domain.Services.BoundedContext
{
    public class AppContext : BaseServiceContext, IAppContext
    {

        public void ModifyAppStar(int accountId, int appId, int starCount)
        {
            Parameter.Validate(accountId).Validate(appId).Validate(starCount);

            using (var dataStore = new DataStore())
            {
                #region 前置条件判断
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.AppStars AS a WHERE a.AccountId=@accountId AND a.AppId=@appId AND a.IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@accountId",accountId),
                        new SqlParameter("@appId",appId)
                    };
                    var result = dataStore.FindSingleValue<Int32>(sql, parameters);
                    if (result > 0)
                    {
                        throw new BusinessException("您已为这个应用打分");
                    }
                }
                #endregion

                #region sql
                {
                    var sql = $@"INSERT dbo.AppStars
                            ( AccountId ,
                              StartNum ,
                              IsDeleted ,
                              AddTime ,
                              LastModifyTime ,
                              AppId
                            ) 
                    VALUES  ( @accountId , -- AccountId - int
                              @starCount , -- StartNum - float
                              0 , -- IsDeleted - bit
                              GETDATE() , -- AddTime - datetime
                              GETDATE() , -- LastModifyTime - datetime
                              @appId  -- AppId - int
                            )";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@accountId",accountId),
                        new SqlParameter("@starCount",starCount),
                        new SqlParameter("@appId",appId)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
                #endregion
            }
        }

        public void CreateNewApp(App app)
        {
            using (var dataStore = new DataStore())
            {
                #region app
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
                                LastModifyTime,
                                IsIconByUpload
                            )
                            VALUES
                            (   @Name,       -- Name - nvarchar(6)
                                @IconUrl,       -- IconUrl - nvarchar(max)
                                @AppUrl,       -- AppUrl - nvarchar(max)
                                @Remark,       -- Remark - nvarchar(50)
                                @Width,         -- Width - int
                                @Height,         -- Height - int
                                0,         -- UseCount - int
                                @IsMax,      -- IsMax - bit
                                @IsFull,      -- IsFull - bit
                                @IsSetbar,      -- IsSetbar - bit
                                @IsOpenMax,      -- IsOpenMax - bit
                                0,      -- IsLock - bit
                                @IsSystem,      -- IsSystem - bit
                                @IsFlash,      -- IsFlash - bit
                                @IsDraw,      -- IsDraw - bit
                                @IsResize,      -- IsResize - bit
                                @AccountId,         -- AccountId - int
                                @AppTypeId,         -- AppTypeId - int
                                0,      -- IsRecommand - bit
                                @AppAuditState,         -- AppAuditState - int
                                @UnRelease,         -- AppReleaseState - int
                                @AppStyle,         -- AppStyle - int
                                0,      -- IsDeleted - bit
                                GETDATE(), -- AddTime - datetime
                                GETDATE()  -- LastModifyTime - datetime,
                                @IsIconByUpload
                            )";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@Name",app.Name),
                        new SqlParameter("@IconUrl",app.IconUrl),
                        new SqlParameter("@AppUrl",app.AppUrl),
                        new SqlParameter("@Remark",app.Remark),
                        new SqlParameter("@Width",app.Width),
                        new SqlParameter("@Height",app.Height),
                        new SqlParameter("@IsMax",app.IsMax.ParseToInt32()),
                        new SqlParameter("@IsFull",app.IsFull.ParseToInt32()),
                        new SqlParameter("@IsSetbar",app.IsSetbar.ParseToInt32()),
                        new SqlParameter("@IsOpenMax",app.IsOpenMax.ParseToInt32()),
                        new SqlParameter("@IsSystem",app.IsSystem.ParseToInt32()),
                        new SqlParameter("@IsFlash",app.IsFlash.ParseToInt32()),
                        new SqlParameter("@IsDraw",app.IsDraw.ParseToInt32()),
                        new SqlParameter("@IsResize",app.IsResize.ParseToInt32()),
                        new SqlParameter("@AccountId",app.AccountId),
                        new SqlParameter("@AppTypeId",app.AppTypeId),
                        new SqlParameter("@AppAuditState",(Int32)app.AppAuditState),
                        new SqlParameter("@UnRelease",(Int32)AppReleaseState.UnRelease),
                        new SqlParameter("@AppStyle",(Int32)app.AppStyle),
                        new SqlParameter("@IsIconByUpload",app.IsIconByUpload)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
                #endregion
            }
        }

        public void Pass(Int32 appId)
        {
            Parameter.Validate(appId);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Apps SET AppAuditState=@AppAuditState WHERE Id=@appId AND IsDeleted=0";
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@AppAuditState",(Int32)AppAuditState.Pass),
                    new SqlParameter("@appId",appId)
                };
                dataStore.SqlExecute(sql, parameters);
            }
        }

        public void Deny(Int32 appId)
        {
            Parameter.Validate(appId);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Apps SET AppAuditState=@AppAuditState WHERE Id=@appId AND IsDeleted=0";
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@AppAuditState",(Int32)AppAuditState.Deny),
                    new SqlParameter("@appId",appId)
                };
                dataStore.SqlExecute(sql, parameters);
            }
        }

        public void SetTodayRecommandApp(Int32 appId)
        {
            Parameter.Validate(appId);
            using (var dataStore = new DataStore())
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
                        var sql = $@"UPDATE dbo.Apps SET IsRecommand=1 WHERE Id=@appId AND IsDeleted=0";
                        var parameters = new List<SqlParameter>
                        {
                            new SqlParameter("@appId",appId)
                        };
                        dataStore.SqlExecute(sql, parameters);
                    }
                    #endregion

                    dataStore.Commit();
                }
                catch (Exception)
                {
                    dataStore.Rollback();
                    throw;
                }
            }
        }

        public void RemoveApp(Int32 appId)
        {
            Parameter.Validate(appId);
            using (var dataStore = new DataStore())
            {
                dataStore.OpenTransaction();
                try
                {
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@appId",appId)
                    };
                    #region 移除app的评分
                    {
                        var sql = $@"UPDATE dbo.AppStars SET IsDeleted=1 WHERE AppId=@appId";
                        dataStore.SqlExecute(sql, parameters);
                    }
                    #endregion

                    #region 移除app
                    {
                        var sql = $@"UPDATE dbo.Apps SET IsDeleted=1 WHERE Id=@appId";
                        dataStore.SqlExecute(sql, parameters);
                    }
                    #endregion

                    dataStore.Commit();
                }
                catch (Exception)
                {
                    dataStore.Rollback();
                    throw;
                }
            }
        }

        public void ReleaseApp(Int32 appId)
        {
            Parameter.Validate(appId);
            using (var dataStore = new DataStore())
            {
                #region 发布app
                {
                    var sql = $@"UPDATE dbo.Apps SET AppReleaseState=@AppReleaseState WHERE Id=@appId AND IsDeleted=0 AND AppAuditState=@AppAuditState";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@AppReleaseState",(Int32)AppReleaseState.Release),
                        new SqlParameter("@AppAuditState",(Int32)AppAuditState.Pass),
                        new SqlParameter("@appId",appId)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
                #endregion
            }
        }

        public void ModifyAccountAppInfo(Int32 accountId, App app)
        {
            Parameter.Validate(accountId).Validate(accountId).Validate(app);
            using (var dataStore = new DataStore())
            {
                var set = new StringBuilder();
                set.Append($@" IsIconByUpload=@IsIconByUpload ,IconUrl=@IconUrl,Name=@Name,AppTypeId=@AppTypeId,AppUrl=@AppUrl,Width=@Width,Height=@Height,AppStyle=@AppStyle,IsResize=@IsResize,IsOpenMax=@IsOpenMax,IsFlash=@IsFlash,Remark=@Remark ");

                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@IsIconByUpload",(app.IsIconByUpload ? 1 : 0)),
                    new SqlParameter("@IconUrl",app.IconUrl),
                    new SqlParameter("@Name",app.Name),
                    new SqlParameter("@AppTypeId",app.AppTypeId),
                    new SqlParameter("@AppUrl",app.AppUrl),
                    new SqlParameter("@Width",app.Width),
                    new SqlParameter("@Height",app.Height),
                    new SqlParameter("@AppStyle",(Int32)app.AppStyle),
                    new SqlParameter("@IsResize",app.IsResize.ParseToInt32()),
                    new SqlParameter("@IsOpenMax",app.IsOpenMax.ParseToInt32()),
                    new SqlParameter("@IsFlash",app.IsFlash.ParseToInt32()),
                    new SqlParameter("@Remark",app.Remark),
                };
                if (app.AppAuditState == AppAuditState.Wait)
                {
                    parameters.Add(new SqlParameter("@AppAuditState", (Int32)AppAuditState.Wait));
                    set.Append($@" ,AppAuditState=@AppAuditState ");
                }
                else
                {
                    parameters.Add(new SqlParameter("@AppAuditState", (Int32)AppAuditState.UnAuditState));
                    set.Append($@" ,AppAuditState=@AppAuditState ");
                }

                var sql = $@"UPDATE Apps SET {set} WHERE AccountId=@accountId AND Id=@appId AND IsDeleted=0";
                parameters.Add(new SqlParameter("@accountId", accountId));
                parameters.Add(new SqlParameter("@appId", app.Id));
                dataStore.SqlExecute(sql.ToString(), parameters);
            }
        }

        public void DeleteAppType(Int32 appTypeId)
        {
            Parameter.Validate(appTypeId);
            using (var dataStore = new DataStore())
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@AppTypeId",appTypeId)
                };
                #region 前置条件验证
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.Apps AS a WHERE a.AppTypeId=@AppTypeId AND a.IsDeleted=0";
                    if (dataStore.FindSingleValue<Int32>(sql, parameters) > 0)
                    {
                        throw new BusinessException($@"当前分类下已有绑定app,不能删除当前分类");
                    }
                }
                #endregion

                #region 移除app分类
                {
                    var sql = $@"UPDATE dbo.AppTypes SET IsDeleted=1 WHERE Id=@AppTypeId";
                    dataStore.SqlExecute(sql, parameters);
                }
                #endregion
            }
        }

        public void CreateNewAppType(AppType appType)
        {
            Parameter.Validate(appType);
            using (var dataStore = new DataStore())
            {
                #region 前置条件验证
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.AppTypes AS a WHERE a.Name=@name AND a.IsDeleted=0";
                    var result = dataStore.FindSingleValue<Int32>(sql, new List<SqlParameter> { new SqlParameter("@name", appType.Name) });
                    if (result > 0)
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
                                (   @Name,       -- Name - nvarchar(6)
                                    @Remark,       -- Remark - nvarchar(50)
                                    0,      -- IsDeleted - bit
                                    GETDATE(), -- AddTime - datetime
                                    GETDATE()  -- LastModifyTime - datetime
                                )";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@Name",appType.Name),
                        new SqlParameter("@Remark",appType.Remark)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
                #endregion
            }
        }

        public void ModifyAppType(String appTypeName, Int32 appTypeId)
        {
            Parameter.Validate(appTypeName).Validate(appTypeId);
            using (var dataStore = new DataStore())
            {
                #region 前置条件验证
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.AppTypes AS a WHERE a.Name=@name AND a.IsDeleted=0";
                    var result = dataStore.FindSingleValue<Int32>(sql, new List<SqlParameter> { new SqlParameter("@name", appTypeName) });
                    if (result > 0)
                    {
                        throw new BusinessException($@"分类:{appTypeName},已存在");
                    }
                }
                #endregion

                #region 更新app分类
                {
                    var sql = $@"UPDATE dbo.AppTypes SET Name=@name WHERE Id=@Id AND IsDeleted=0";
                    dataStore.SqlExecute(sql, new List<SqlParameter> { new SqlParameter("@name", appTypeName), new SqlParameter("Id", appTypeId) });
                }
                #endregion
            }
        }

        public void ModifyAppIcon(Int32 accountId, Int32 appId, String newIcon)
        {
            Parameter.Validate(accountId).Validate(appId).Validate(newIcon);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Apps SET IconUrl=@url WHERE Id=@appId AND AccountId=@accountId AND IsDeleted=0 ";
                dataStore.SqlExecute(sql, new List<SqlParameter> { new SqlParameter("@url", newIcon), new SqlParameter("@appId", appId), new SqlParameter("@accountId", accountId) });
            }
        }

        public void Install(Int32 accountId, Int32 appId, Int32 deskNum)
        {
            Parameter.Validate(accountId).Validate(appId).Validate(deskNum);

            using (var dataStore = new DataStore())
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
                                a.IsDraw,
                                a.IsIconByUpload
                                FROM  dbo.Apps AS a WHERE a.AppAuditState=@AppAuditState AND a.AppReleaseState=@AppReleaseState AND a.IsDeleted=0 AND a.Id=@Id";
                        var parameters = new List<SqlParameter>
                        {
                            new SqlParameter("@AppAuditState",(Int32)AppAuditState.Pass),
                            new SqlParameter("@AppReleaseState",(Int32)AppReleaseState.Release),
                            new SqlParameter("@Id",appId)
                        };
                        app = dataStore.FindOne<App>(sql, parameters);

                        if (app == null)
                        {
                            throw new BusinessException($"获取应用失败，请刷新重试");
                        }
                    }
                    #endregion

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
                              DeskIndex,
                              IsIconByUpload
                            )
                    VALUES  ( @AppId , -- AppId - int
                              @Width , -- Width - int
                              @Height , -- Height - int
                              0 , -- FolderId - int
                              @Name , -- Name - nvarchar(6)
                              @IconUrl , -- IconUrl - nvarchar(max)
                              @AppUrl , -- AppUrl - nvarchar(max)
                              0 , -- IsOnDock - bit
                              @IsMax , -- IsMax - bit
                              @IsFull , -- IsFull - bit
                              @IsSetbar , -- IsSetbar - bit
                              @IsOpenMax , -- IsOpenMax - bit
                              @IsLock , -- IsLock - bit
                              @IsFlash , -- IsFlash - bit
                              @IsDraw , -- IsDraw - bit
                              @IsResize , -- IsResize - bit
                              @MemberType , -- MemberType - int
                              0 , -- IsDeleted - bit
                              GETDATE() , -- AddTime - datetime
                              GETDATE() , -- LastModifyTime - datetime
                              @accountId , -- AccountId - int
                              @deskNum,  -- DeskIndex - int
                              @IsIconByUpload
                            )";
                        var parameters = new List<SqlParameter>
                        {
                            new SqlParameter("@AppId",newMember.AppId),
                            new SqlParameter("@Width",newMember.Width),
                            new SqlParameter("@Height",newMember.Height),
                            new SqlParameter("@Name",newMember.Name),
                            new SqlParameter("@IconUrl",newMember.IconUrl),
                            new SqlParameter("@AppUrl",newMember.AppUrl),
                            new SqlParameter("@IsMax",newMember.IsMax.ParseToInt32()),
                            new SqlParameter("@IsFull",newMember.IsFull.ParseToInt32()),
                            new SqlParameter("@IsSetbar",newMember.IsSetbar.ParseToInt32()),
                            new SqlParameter("@IsOpenMax",newMember.IsOpenMax.ParseToInt32()),
                            new SqlParameter("@IsLock",newMember.IsLock.ParseToInt32()),
                            new SqlParameter("@IsFlash",newMember.IsFlash.ParseToInt32()),
                            new SqlParameter("@IsDraw",newMember.IsDraw.ParseToInt32()),
                            new SqlParameter("@IsResize",newMember.IsResize.ParseToInt32()),
                            new SqlParameter("@MemberType",(Int32)newMember.MemberType),
                            new SqlParameter("@accountId",accountId),
                            new SqlParameter("@deskNum",deskNum),
                            new SqlParameter("@IsIconByUpload",(app.IsIconByUpload ? 1 : 0)),
                        };
                        dataStore.SqlExecute(sql, parameters);
                    }
                    #endregion

                    #region 更改app使用数量
                    {
                        var sql = $@"UPDATE dbo.Apps SET UseCount=UseCount+1 WHERE Id=@appId AND IsDeleted=0";
                        var parameters = new List<SqlParameter>
                        {
                            new SqlParameter("@appId",app.Id)
                        };
                        dataStore.SqlExecute(sql, parameters);
                    }
                    #endregion

                    dataStore.Commit();
                }
                catch (Exception)
                {
                    dataStore.Rollback();
                    throw;
                }
            }
        }

        public Tuple<Int32, Int32> GetAccountDevelopAppCountAndNotReleaseAppCount(Int32 accountId)
        {
            Parameter.Validate(accountId);
            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT a.Id FROM dbo.Apps AS a WHERE a.AccountId=@accountId AND a.IsDeleted=0";
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@accountId",accountId)
                };
                var result = dataStore.Find<App>(sql, parameters);
                return new Tuple<int, int>(result.Count, result.Count(a => a.AppReleaseState == AppReleaseState.UnRelease));
            }
        }

        public List<AppType> GetAppTypes()
        {
            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT a.Id,a.Name FROM dbo.AppTypes AS a WHERE a.IsDeleted=0";
                return dataStore.Find<AppType>(sql);
            }
        }

        public TodayRecommendAppDto GetTodayRecommend(int accountId)
        {
            Parameter.Validate(accountId);

            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT 
                            a.UseCount,
                            a.Id,
                            a.Name,
                            a.IconUrl AS AppIcon,
                            a.Remark,
                            a.AppStyle AS Style,
                            (
		                        SELECT AVG(stars.StartNum) FROM dbo.AppStars AS stars WHERE stars.AppId=a.Id AND stars.IsDeleted=0 GROUP BY stars.AppId
                            ) AS AppStars,
                            (
	                            CASE 
								WHEN a2.Id IS NULL THEN CAST(0 AS BIT)
								ELSE CAST(1 AS BIT)
								END
                            ) AS IsInstall,
                            ISNULL(a.IsIconByUpload,0) AS IsIconByUpload
                            FROM dbo.Apps AS a 
							LEFT JOIN dbo.Members AS a2 ON a2.AccountId=@accountId AND a2.IsDeleted=0 AND a2.AppId=a.Id
                            WHERE a.AppAuditState=@AppAuditState AND a.AppReleaseState=@AppReleaseState AND a.IsRecommand=1";
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@AppAuditState",(Int32)AppAuditState.Pass),
                    new SqlParameter("@AppReleaseState",(Int32)AppReleaseState.Release),
                    new SqlParameter("@accountId",accountId)
                };
                return dataStore.FindOne<TodayRecommendAppDto>(sql, parameters);
            }
        }

        public List<App> GetApps(int accountId, int appTypeId, int orderId, string searchText, int pageIndex, int pageSize, out int totalCount)
        {
            Parameter.Validate(accountId, true).Validate(orderId).Validate(searchText).Validate(pageIndex, true).Validate(pageSize);
            using (var dataStore = new DataStore())
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@AppAuditState", (Int32)AppAuditState.Pass));
                parameters.Add(new SqlParameter("@AppReleaseState", (Int32)AppReleaseState.Release));

                var where = new StringBuilder();
                where.Append($@" WHERE 1=1 AND a.IsDeleted=0 AND a.AppAuditState=@AppAuditState AND a.AppReleaseState=@AppReleaseState");
                if (appTypeId != 0 && appTypeId != -1)//全部app
                {
                    parameters.Add(new SqlParameter("@AppTypeId", appTypeId));
                    where.Append($@" AND a.AppTypeId=@AppTypeId");
                }
                else
                {
                    if (appTypeId == -1)//用户制作的app
                    {
                        parameters.Add(new SqlParameter("@accountId", accountId));
                        where.Append($@" AND a.AccountId=@accountId");
                    }
                }
                if (!String.IsNullOrEmpty(searchText))//关键字搜索
                {
                    parameters.Add(new SqlParameter("@Name", $@"%{searchText}%"));
                    where.Append($@" AND a.Name LIKE @Name");
                }

                var orderBy = new StringBuilder();
                switch (orderId)
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
                            orderBy.Append($@" ORDER BY aa.StarCount DESC");
                            break;
                        }
                }

                #region totalCount
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.Apps AS a 
                                LEFT JOIN dbo.AppStars AS a1
                                ON a1.AppId=a.Id AND a1.IsDeleted=0 {where}";
                    totalCount = dataStore.FindSingleValue<Int32>(sql, parameters);
                }
                #endregion

                #region sql
                {
                    var sql = $@"SELECT TOP (@pageSize) * FROM 
                                (
	                                SELECT 
	                                    ROW_NUMBER() OVER(ORDER BY a.AddTime DESC) AS rownumber,
	                                    a.AppTypeId,
	                                    a.AccountId,
	                                    a.AddTime,
	                                    a.UseCount,
	                                    (
		                                    SELECT AVG(stars.StartNum) FROM dbo.AppStars AS stars WHERE stars.AppId=a.Id AND stars.IsDeleted=0 GROUP BY stars.AppId
	                                    ) AS StarCount,
	                                    a.Name,
	                                    a.IconUrl,
	                                    a.Remark,
	                                    a.AppStyle,
	                                    a.Id,
	                                    (
		                                    CASE 
			                                    WHEN a1.Id IS NOT NULL THEN CAST(1 AS BIT)
			                                    ELSE CAST(0 AS BIT)
		                                    END
	                                    ) AS IsInstall,
                                        a.IsIconByUpload
	                                    FROM dbo.Apps AS a
	                                    LEFT JOIN dbo.Members AS a1 ON a1.AccountId=a.AccountId AND a1.AppId=a.Id AND a.IsDeleted=0
                                        {where}
                                ) AS aa WHERE aa.rownumber>@pageSize*(@pageIndex-1) {orderBy}";
                    parameters.Add(new SqlParameter("@pageSize", pageSize));
                    parameters.Add(new SqlParameter("@pageIndex", pageIndex));
                    return dataStore.Find<App>(sql, parameters);
                }
                #endregion
            }
        }

        public List<App> GetAccountApps(Int32 accountId, String searchText, Int32 appTypeId, Int32 appStyleId, String appState, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            Parameter.Validate(accountId, true).Validate(searchText).Validate(appTypeId, true).Validate(appStyleId, true).Validate(pageIndex).Validate(pageSize);

            using (var dataStore = new DataStore())
            {
                var where = new StringBuilder();
                where.Append($@" WHERE 1=1 ");
                var parameters = new List<SqlParameter>();

                #region 条件筛选

                if (accountId != default(Int32))
                {
                    parameters.Add(new SqlParameter("@accountId", accountId));
                    where.Append($@" AND a.AccountId=@accountId");
                }

                //应用名称
                if (!String.IsNullOrEmpty(searchText))
                {
                    parameters.Add(new SqlParameter("@Name", $@"%{searchText}%"));
                    where.Append($@" AND a.Name LIKE @Name");
                }

                //应用所属类型
                if (appTypeId != 0)
                {
                    parameters.Add(new SqlParameter("AppTypeId", appTypeId));
                    where.Append($@" AND a.AppTypeId=@AppTypeId");
                }

                //应用样式
                if (appStyleId != 0)
                {
                    var appStyle = EnumExtensions.ToEnum<AppStyle>(appStyleId);
                    parameters.Add(new SqlParameter("@AppStyle", (Int32)appStyle));
                    where.Append($@" AND a.AppStyle=@AppStyle");
                }

                if ((appState + "").Length > 0)
                {
                    //app发布状态
                    var stats = appState.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (stats[0] == "AppReleaseState")
                    {
                        var appReleaseState = EnumExtensions.ToEnum<AppReleaseState>(Int32.Parse(stats[1]));
                        parameters.Add(new SqlParameter("AppReleaseState", (Int32)appReleaseState));
                        where.Append($@" AND a.AppReleaseState=@AppReleaseState ");
                    }

                    //app应用审核状态
                    if (stats[0] == "AppAuditState")
                    {
                        var appAuditState = EnumExtensions.ToEnum<AppAuditState>(Int32.Parse(stats[1]));
                        parameters.Add(new SqlParameter("@AppAuditState", (Int32)appAuditState));
                        where.Append($@" AND a.AppAuditState=@AppAuditState");
                    }
                }

                #endregion

                #region totalCount
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.Apps AS a {where} ";
                    totalCount = dataStore.FindSingleValue<Int32>(sql, parameters);
                }
                #endregion

                #region sql
                {
                    var sql = $@"SELECT TOP (@pageSize) * FROM 
                    (
	                    SELECT
	                    ROW_NUMBER() OVER(ORDER BY a.Id DESC) AS rownumber,
	                    a.Name,
	                    a.AppStyle,
	                    a.UseCount,
	                    a.Id,
	                    a.IconUrl,
	                    a.AppAuditState,
	                    a.IsRecommand,
                        a.AppTypeId,
                        a.AccountId,
                        a.IsIconByUpload
	                    FROM dbo.Apps AS a {where} 
                    ) AS aa WHERE aa.rownumber>@pageSize*(@pageIndex-1)";
                    parameters.Add(new SqlParameter("@pageIndex", pageIndex));
                    parameters.Add(new SqlParameter("@pageSize", pageSize));
                    return dataStore.Find<App>(sql, parameters);
                }
                #endregion
            }
        }

        public App GetApp(Int32 appId)
        {
            Parameter.Validate(appId);
            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT 
                            a.Name,
                            a.IconUrl,
                            a.Remark,
                            a.UseCount,
                            (
		                      SELECT AVG(stars.StartNum) FROM dbo.AppStars AS stars WHERE stars.AppId=a.Id AND stars.IsDeleted=0 GROUP BY stars.AppId
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
                            a.AppTypeId,
                            a2.Name AS AccountName,
                            a.IsIconByUpload
                            FROM dbo.Apps AS a 
                            LEFT JOIN dbo.Accounts AS a2
                            ON a2.Id=a.AccountId AND a2.IsDeleted=0 AND a2.IsDisable=0
                            WHERE a.Id=@Id AND a.IsDeleted=0";
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Id",appId)
                };
                return dataStore.FindOne<App>(sql, parameters);
            }
        }

        public Boolean IsInstallApp(int accountId, int appId)
        {
            Parameter.Validate(accountId).Validate(appId);
            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT COUNT(*) FROM dbo.Members AS a WHERE a.AppId=@Id AND a.AccountId=@AccountId AND a.IsDeleted=0";
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Id",appId),
                    new SqlParameter("@AccountId",accountId)
                };
                return dataStore.FindSingleValue<Int32>(sql, parameters) > 0 ? true : false;
            }
        }

        public List<App> GetSystemApp(IEnumerable<Int32> appIds = default(IEnumerable<Int32>))
        {
            using (var dataStore = new DataStore())
            {
                var where = new StringBuilder();
                where.Append(" WHERE 1=1 AND a.IsSystem=1 AND a.IsDeleted=0");
                if (appIds != default(IEnumerable<Int32>) && appIds.Any())
                {
                    where.Append($@" AND a.Id IN({String.Join(",", appIds)})");
                }

                var sql = $@"SELECT a.Id,a.Name,a.IconUrl FROM dbo.Apps AS a {where}";
                return dataStore.Find<App>(sql);
            }
        }

        public Boolean CheckAppTypeName(String appTypeName)
        {
            Parameter.Validate(appTypeName);

            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT COUNT(*) FROM dbo.AppTypes AS a WHERE a.Name=@name AND a.IsDeleted=0";
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@name",appTypeName)
                };
                return dataStore.FindSingleValue<Int32>(sql, parameters) > 0;
            }
        }
    }
}
