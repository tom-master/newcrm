using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewLib;
using NewLib.Data.SqlMapper.InternalDataStore;
using NewLib.Security;

namespace NewCRM.Domain.Services.BoundedContext
{
    public class AccountContext : BaseServiceContext, IAccountContext
    {
        public Account Validate(String accountName, String password, String requestIp)
        {
            Parameter.Validate(accountName).Validate(password);

            using (var dataStore = new DataStore())
            {
                Account result = null;
                try
                {
                    dataStore.OpenTransaction();

                    #region 查询用户
                    {
                        var sql = @"SELECT a.Id,a.Name,a.LoginPassword,a1.AccountFace 
                                    FROM dbo.Accounts AS a
                                    INNER JOIN dbo.Configs AS a1
                                    ON a1.AccountId=a.Id 
                                    WHERE a.Name=@name AND a.IsDeleted=0 AND a.IsDisable=0";
                        result = dataStore.Find<Account>(sql, new List<SqlParameter> { new SqlParameter("@name", accountName) }).FirstOrDefault();
                        if (result == null)
                        {
                            throw new BusinessException($"该用户不存在或被禁用{accountName}");
                        }

                        if (!PasswordUtil.ComparePasswords(result.LoginPassword, password))
                        {
                            throw new BusinessException("密码错误");
                        }
                    }
                    #endregion

                    #region 设置用户在线
                    {
                        var sql = $@"UPDATE dbo.Accounts SET IsOnline=1,LastLoginTime=GETDATE() WHERE Id=@accountId AND IsDeleted=0 AND IsDisable=0 SELECT CAST(@@ROWCOUNT AS INT)";
                        var parameters = new List<SqlParameter>
                        {
                            new SqlParameter("@accountId",result.Id)
                        };
                        var rowCount = dataStore.FindSingleValue<Int32>(sql, parameters);
                        if (rowCount == 0)
                        {
                            throw new BusinessException("设置用户在线状态失败");
                        }
                    }
                    #endregion

                    #region 添加在线用户列表
                    {
                        var sql = $@"INSERT dbo.Onlines
                                    (
                                        IpAddress,
                                        AccountId,
                                        IsDeleted,
                                        AddTime,
                                        LastModifyTime
                                    )
                                    VALUES
                                    (   @requestIp,       -- IpAddress - nvarchar(max)
                                        @accountId,         -- AccountId - int
                                        0,      -- IsDeleted - bit
                                        GETDATE(), -- AddTime - datetime
                                        GETDATE()  -- LastModifyTime - datetime
                                    ) SELECT CAST(@@ROWCOUNT AS INT)";
                        var parameters = new List<SqlParameter>
                        {
                            new SqlParameter("@requestIp",requestIp),
                            new SqlParameter("@accountId",result.Id)
                        };
                        var rowCount = dataStore.FindSingleValue<Int32>(sql, parameters);
                        if (rowCount == 0)
                        {
                            throw new BusinessException("添加在线列表失败");
                        }
                    }
                    #endregion

                    dataStore.Commit();
                    return result;
                }
                catch (Exception)
                {
                    dataStore.Rollback();
                    throw;
                }
            }
        }

        public Config GetConfig(Int32 accountId)
        {
            Parameter.Validate(accountId);
            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT 
                            a.Id,
                            a.Skin,
                            a.AccountFace,
                            a.AppSize,
                            a.AppVerticalSpacing,
                            a.AppHorizontalSpacing,
                            a.DefaultDeskNumber,
                            a.DefaultDeskCount,
                            a.AppXy,
                            a.DockPosition,
                            a.WallpaperMode,
                            a.WallpaperId,
                            a.IsBing,
                            a.AccountId
                            FROM dbo.Configs AS a WHERE a.AccountId=@accountId AND a.IsDeleted=0";
                var parameters = new List<SqlParameter> { new SqlParameter("@accountId", accountId) };
                var result = dataStore.Find<Config>(sql, parameters).FirstOrDefault();
                return result;
            }
        }

        public Wallpaper GetWallpaper(Int32 wallPaperId)
        {
            Parameter.Validate(wallPaperId);

            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT a.Url,a.Width,a.Height,a.Source FROM dbo.Wallpapers AS a WHERE a.Id=@wallpaperId AND a.IsDeleted=0";
                var parameters = new List<SqlParameter> { new SqlParameter("@wallpaperId", wallPaperId) };
                return dataStore.Find<Wallpaper>(sql, parameters).FirstOrDefault();
            }
        }

        public List<Account> GetAccounts(string accountName, string accountType, int pageIndex, int pageSize, out int totalCount)
        {
            Parameter.Validate(pageIndex).Validate(pageSize);

            var where = new StringBuilder();
            where.Append("WHERE 1=1 AND a.IsDeleted=0 ");
            var parameters = new List<SqlParameter>();
            if (!String.IsNullOrEmpty(accountName))
            {
                parameters.Add(new SqlParameter("@name", accountName));
                where.Append(" AND a.Name=@name");
            }

            if (!String.IsNullOrEmpty(accountType))
            {
                var isAdmin = (EnumExtensions.ToEnum<AccountType>(Int32.Parse(accountType)) == AccountType.Admin) ? 1 : 0;
                parameters.Add(new SqlParameter("@isAdmin", isAdmin));
                where.Append($@" AND a.IsAdmin=@isAdmin");
            }

            using (var dataStore = new DataStore())
            {
                #region totalCount
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.Accounts AS a 
                                 INNER JOIN dbo.Configs AS a1 ON a1.AccountId=a.Id AND a1.IsDeleted=0 {where} ";
                    totalCount = dataStore.FindSingleValue<Int32>(sql, parameters);
                }
                #endregion

                #region sql
                {
                    var sql = $@"SELECT TOP (@pageSize) * FROM 
                            (
	                            SELECT ROW_NUMBER() OVER(ORDER BY a.Id DESC) AS rownumber,
                                a.Id,a.IsAdmin,a.Name,a.IsDisable,a1.AccountFace 
	                            FROM dbo.Accounts AS a 
	                            INNER JOIN dbo.Configs AS a1
	                            ON a1.AccountId=a.Id AND a1.IsDeleted=0
	                            {where} 
                            ) AS a2 WHERE a2.rownumber>@pageSize*(@pageIndex-1)";
                    parameters.Add(new SqlParameter("@pageSize", pageSize));
                    parameters.Add(new SqlParameter("@pageIndex", pageIndex));
                    return dataStore.Find<Account>(sql, parameters);
                }
                #endregion
            }
        }

        public Account GetAccount(int accountId)
        {
            Parameter.Validate(accountId);

            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT 
                            a1.AccountFace,
                            a.AddTime,
                            a.Id,
                            a.IsAdmin,
                            a.IsDisable,
                            a.IsOnline,
                            a.LastLoginTime,
                            a.LastModifyTime,
                            a.Name,
                            a.LockScreenPassword,
                            a.LoginPassword
                            FROM dbo.Accounts AS a 
                            INNER JOIN dbo.Configs AS a1
                            ON a1.AccountId=a.Id
                            WHERE a.Id=@accountId AND a.IsDeleted=0 AND a.IsDisable=0";
                var parameters = new List<SqlParameter> { new SqlParameter("@accountId", accountId) };
                return dataStore.FindOne<Account>(sql, parameters);
            }
        }

        public List<Role> GetRoles(Int32 accountId)
        {
            Parameter.Validate(accountId);
            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT
                            a1.Id,
                            a1.Name,
                            a1.RoleIdentity
                            FROM dbo.AccountRoles AS a
                            INNER JOIN dbo.Roles AS a1
                            ON a1.Id=a.RoleId AND a1.IsDeleted=0 
                            WHERE a.AccountId=@accountId AND a.IsDeleted=0 ";
                var parameters = new List<SqlParameter> { new SqlParameter("@accountId", accountId) };
                return dataStore.Find<Role>(sql, parameters);
            }
        }

        public List<RolePower> GetPowers()
        {
            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT a.RoleId,a.AppId FROM dbo.RolePowers AS a WHERE a.IsDeleted=0";
                return dataStore.Find<RolePower>(sql);
            }
        }

        public Boolean CheckAccountNameExist(string accountName)
        {
            Parameter.Validate(accountName);

            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT COUNT(*) FROM dbo.Accounts AS a WHERE a.Name=@name AND a.IsDeleted=0";
                return dataStore.FindSingleValue<Int32>(sql, new List<SqlParameter> { new SqlParameter("@name", accountName) }) != 0 ? false : true;
            }
        }

        public String GetOldPassword(Int32 accountId)
        {
            Parameter.Validate(accountId);
            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT a.LoginPassword FROM dbo.Accounts AS a WHERE a.Id=@accountId AND a.IsDeleted=0 AND a.IsDisable=0";
                var parameters = new List<SqlParameter> { new SqlParameter("@accountId", accountId) };
                return dataStore.FindSingleValue<String>(sql, parameters);
            }
        }

        public Boolean UnlockScreen(Int32 accountId, String unlockPassword)
        {
            Parameter.Validate(accountId).Validate(unlockPassword);
            using (var dataStore = new DataStore())
            {
                #region 获取锁屏密码
                {
                    var sql = $@"SELECT a.LockScreenPassword FROM dbo.Accounts AS a WHERE a.Id=@accountId AND a.IsDeleted=0 AND a.IsDisable=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@accountId",accountId)
                    };
                    var password = dataStore.FindSingleValue<String>(sql, parameters);
                    return PasswordUtil.ComparePasswords(password, unlockPassword);
                }
                #endregion
            }
        }

        public void Logout(Int32 accountId)
        {
            Parameter.Validate(accountId);

            using (var dataStore = new DataStore())
            {
                try
                {
                    dataStore.OpenTransaction();
                    var parameters = new List<SqlParameter> { new SqlParameter("@accountId", accountId) };
                    #region 设置用户下线
                    {
                        var sql = $@"UPDATE dbo.Accounts SET IsOnline=0 WHERE Id=@accountId AND IsDeleted=0 AND IsDisable=0 SELECT CAST(@@ROWCOUNT AS INT)";
                        var rowCount = dataStore.FindSingleValue<Int32>(sql, parameters);
                        if (rowCount == 0)
                        {
                            throw new BusinessException("设置用户下线状态失败");
                        }
                    }
                    #endregion

                    #region 将当前用户从在线列表中移除
                    {
                        var sql = $@"UPDATE dbo.Onlines SET IsDeleted=1 WHERE AccountId=@accountId AND IsDeleted=0 SELECT CAST(@@ROWCOUNT AS INT)";
                        var rowCount = dataStore.FindSingleValue<Int32>(sql, parameters);
                        if (rowCount == 0)
                        {
                            throw new BusinessException("将用户移出在线列表时失败");
                        }
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

        public void AddNewAccount(Account account)
        {
            Parameter.Validate(account);

            using (var dataStore = new DataStore())
            {
                try
                {
                    dataStore.OpenTransaction();

                    var accountId = 0;
                    var configId = 0;
                    #region 初始化配置
                    {
                        var config = new Config();
                        var sql = $@"INSERT dbo.Configs
                                ( Skin ,
                                  AppSize ,
                                  AppVerticalSpacing ,
                                  AppHorizontalSpacing ,
                                  DefaultDeskNumber ,
                                  DefaultDeskCount ,
                                  WallpaperMode ,
                                  AppXy ,
                                  DockPosition ,
                                  IsDeleted ,
                                  AddTime ,
                                  LastModifyTime ,
                                  WallpaperId ,
                                  AccountFace
                                )
                        VALUES  ( @Skin , -- Skin - nvarchar(max)
                                  @AppSize , -- AppSize - int
                                  @AppVerticalSpacing , -- AppVerticalSpacing - int
                                  @AppHorizontalSpacing , -- AppHorizontalSpacing - int
                                  @DefaultDeskNumber , -- DefaultDeskNumber - int
                                  @DefaultDeskCount , -- DefaultDeskCount - int
                                  @WallpaperMode , -- WallpaperMode - int
                                  @AppXy , -- AppXy - int
                                  @DockPosition , -- DockPosition - int
                                  0 , -- IsDeleted - bit
                                  GETDATE() , -- AddTime - datetime
                                  GETDATE() , -- LastModifyTime - datetime
                                  3 , -- WallpaperId - int
                                  @AccountFace  -- Face - nvarchar(150)
                                ) SELECT CAST(@@IDENTITY AS INT) AS Id";
                        var parameters = new List<SqlParameter>
                        {
                            new SqlParameter("@Skin",config.Skin),
                            new SqlParameter("@AppSize",config.AppSize),
                            new SqlParameter("@AppVerticalSpacing",config.AppVerticalSpacing),
                            new SqlParameter("@AppHorizontalSpacing",config.AppHorizontalSpacing),
                            new SqlParameter("@DefaultDeskNumber",config.DefaultDeskNumber),
                            new SqlParameter("@DefaultDeskCount",config.DefaultDeskCount),
                            new SqlParameter("@WallpaperMode",(Int32)config.WallpaperMode),
                            new SqlParameter("@AppXy",(Int32)config.AppXy),
                            new SqlParameter("@DockPosition",(Int32)config.DockPosition),
                            new SqlParameter("@AccountFace",config.AccountFace)
                        };
                        configId = dataStore.FindSingleValue<Int32>(sql, parameters);
                    }
                    #endregion
                    if (configId == 0)
                    {
                        throw new BusinessException("初始化配置时失败");
                    }
                    #region 新增用户
                    {
                        var sql = $@"INSERT dbo.Accounts
                            ( 
                              Name ,
                              LoginPassword ,
                              LockScreenPassword ,
                              IsDisable ,
                              LastLoginTime ,
                              IsOnline ,
                              IsAdmin ,
                              IsDeleted ,
                              AddTime ,
                              LastModifyTime ,
                              ConfigId ,
                              TitleId,
                              IsBing
                            )
                    VALUES  ( 
                              @name , -- Name - nvarchar(max)
                              @loginPassword , -- LoginPassword - nvarchar(max)
                              @lockScreenPassword , -- LockScreenPassword - nvarchar(max)
                              0 , -- IsDisable - bit
                              GETDATE() , -- LastLoginTime - datetime
                              0 , -- IsOnline - bit
                              @isAdmin , -- IsAdmin - bit
                              0 , -- IsDeleted - bit
                              GETDATE() , -- AddTime - datetime
                              GETDATE() , -- LastModifyTime - datetime
                              @configId , -- ConfigId - int
                              0,  -- TitleId - int
                              0
                            ) SELECT CAST(@@IDENTITY AS INT) AS Id";
                        var parameters = new List<SqlParameter>
                        {
                            new SqlParameter("@name",account.Name),
                            new SqlParameter("@loginPassword",account.LoginPassword),
                            new SqlParameter("@lockScreenPassword",account.LockScreenPassword),
                            new SqlParameter("@isAdmin",account.IsAdmin),
                            new SqlParameter("@configId",configId)
                        };
                        accountId = dataStore.FindSingleValue<Int32>(sql, parameters);
                    }
                    #endregion

                    if (accountId == 0)
                    {
                        throw new BusinessException("初始化用户时失败");
                    }

                    #region 更新用户的配置
                    {
                        var sql = $@"UPDATE dbo.Configs SET AccountId=@accountId WHERE IsDeleted=0 AND Id=@id SELECT CAST(@@ROWCOUNT AS INT)";
                        var parameters = new List<SqlParameter> { new SqlParameter("@accountId", accountId), new SqlParameter("@id", configId) };
                        var rowCount = dataStore.FindSingleValue<Int32>(sql, parameters);
                        if (rowCount == 0)
                        {
                            throw new BusinessException("更新用户配置失败");
                        }
                    }
                    #endregion

                    #region 用户角色
                    {
                        var sqlBuilder = new StringBuilder();
                        foreach (var item in account.Roles)
                        {
                            sqlBuilder.Append($@"INSERT dbo.AccountRoles
                                ( AccountId ,
                                  RoleId ,
                                  IsDeleted ,
                                  AddTime ,
                                  LastModifyTime
                                )
                        VALUES  ( {accountId} , -- AccountId - int
                                  {item.RoleId} , -- RoleId - int
                                  0 , -- IsDeleted - bit
                                  GETDATE() , -- AddTime - datetime
                                  GETDATE()  -- LastModifyTime - datetime
                                )");
                        }
                        dataStore.SqlExecute(sqlBuilder.ToString());
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

        public void ModifyAccount(Account accountDto)
        {
            Parameter.Validate(accountDto);

            using (var dataStore = new DataStore())
            {
                dataStore.OpenTransaction();
                try
                {
                    if (!String.IsNullOrEmpty(accountDto.LoginPassword))
                    {
                        #region 修改密码
                        {
                            var newPassword = PasswordUtil.CreateDbPassword(accountDto.LoginPassword);
                            var sql = $@"UPDATE dbo.Accounts SET LoginPassword=@newPassword WHERE Id=@accountId AND IsDeleted=0 AND IsDisable=0 SELECT CAST(@@ROWCOUNT AS INT)";
                            var parameters = new List<SqlParameter> { new SqlParameter("@newPassword", newPassword), new SqlParameter("@accountId", accountDto.Id) };
                            var rowCount = dataStore.FindSingleValue<Int32>(sql, parameters);
                            if (rowCount == 0)
                            {
                                throw new BusinessException("修改登陆密码失败");
                            }
                        }
                        #endregion
                    }

                    #region 修改账户角色
                    {
                        if (accountDto.Roles.Any())
                        {
                            var sqlBuilder = new StringBuilder();
                            sqlBuilder.Append($@"UPDATE dbo.AccountRoles SET IsDeleted=1 WHERE AccountId={accountDto.Id} AND IsDeleted=0");

                            foreach (var item in accountDto.Roles)
                            {
                                sqlBuilder.Append($@" INSERT dbo.AccountRoles
                                            ( AccountId ,
                                              RoleId ,
                                              IsDeleted ,
                                              AddTime ,
                                              LastModifyTime
                                            )
                                    VALUES  ( {accountDto.Id} , -- AccountId - int
                                              {item.RoleId} , -- RoleId - int
                                              0 , -- IsDeleted - bit
                                              GETDATE() , -- AddTime - datetime
                                              GETDATE()  -- LastModifyTime - datetime
                                            )");
                            }
                            dataStore.SqlExecute(sqlBuilder.ToString());
                        }
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

        public void Enable(Int32 accountId)
        {
            Parameter.Validate(accountId);

            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Accounts SET IsDisable=0 WHERE Id=@accountId AND IsDeleted=0";
                var parameters = new List<SqlParameter> { new SqlParameter("@accountId", accountId) };
                dataStore.SqlExecute(sql, parameters);
            }
        }

        public void Disable(Int32 accountId)
        {
            Parameter.Validate(accountId);
            using (var dataStore = new DataStore())
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@accountId", accountId) };
                #region 前置条件验证
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.Roles AS a
                                INNER JOIN dbo.AccountRoles AS a1
                                ON a1.AccountId=@accountId AND a1.RoleId=a.Id AND a1.IsDeleted=0
                                WHERE a.IsDeleted=0 AND a.IsAllowDisable=0";
                    var result = dataStore.FindSingleValue<Int32>(sql, parameters);
                    if (result > 0)
                    {
                        throw new BusinessException("当前用户拥有管理员角色，因此不能禁用或删除");
                    }
                }
                #endregion
                {
                    var sql = $@"UPDATE dbo.Accounts SET IsDisable=1 WHERE Id=@accountId AND IsDeleted=0";
                    dataStore.SqlExecute(sql, parameters);
                }
            }
        }

        public void ModifyAccountFace(Int32 accountId, String newFace)
        {
            Parameter.Validate(accountId).Validate(newFace);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Configs SET AccountFace=@face WHERE AccountId=@accountId AND IsDeleted=0";
                dataStore.SqlExecute(sql, new List<SqlParameter> { new SqlParameter("@face", newFace), new SqlParameter("@accountId", accountId) });
            }
        }

        public void ModifyPassword(Int32 accountId, String newPassword, Boolean isTogetherSetLockPassword)
        {
            Parameter.Validate(accountId).Validate(newPassword);
            using (var dataStore = new DataStore())
            {
                var lockPassword = "";
                var parameters = new List<SqlParameter>();
                if (isTogetherSetLockPassword)
                {
                    lockPassword = ",LockScreenPassword=@lockPassword";
                    parameters.Add(new SqlParameter("@lockPassword", newPassword));
                }
                var sql = $@"UPDATE dbo.Accounts SET LoginPassword=@password {lockPassword} WHERE Id=@accountId AND IsDeleted=0 AND IsDisable=0";
                parameters.Add(new SqlParameter("@password", newPassword));
                parameters.Add(new SqlParameter("@accountId", accountId));

                dataStore.SqlExecute(sql, parameters);
            }
        }

        public void ModifyLockScreenPassword(Int32 accountId, String newScreenPassword)
        {
            Parameter.Validate(accountId).Validate(newScreenPassword);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Accounts SET LockScreenPassword=@password WHERE Id=@accountId AND IsDeleted=0 AND IsDisable=0";
                dataStore.SqlExecute(sql, new List<SqlParameter> { new SqlParameter("@password", newScreenPassword), new SqlParameter("@accountId", accountId) });
            }
        }

        public void RemoveAccount(Int32 accountId)
        {
            Parameter.Validate(accountId);

            using (var dataStore = new DataStore())
            {
                dataStore.OpenTransaction();
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@accountId",accountId)
                };
                try
                {
                    #region 前置条件验证
                    {
                        var sql = $@"SELECT a.IsAdmin FROM dbo.Accounts AS a WHERE a.Id=@accountId AND a.IsDeleted=0 AND a.IsDisable=0";
                        var isAdmin = Boolean.Parse(dataStore.FindSingleValue<String>(sql, parameters));
                        if (isAdmin)
                        {
                            throw new BusinessException("不能删除管理员");
                        }
                    }
                    #endregion

                    #region 移除账户
                    {
                        var sql = $@"UPDATE dbo.Accounts SET IsDeleted=1 WHERE Id=@accountId AND IsDeleted=0 AND IsDisable=0";
                        dataStore.SqlExecute(sql, parameters);
                    }
                    #endregion

                    #region 移除账户配置
                    {
                        var sql = $@"UPDATE dbo.Configs SET IsDeleted=1 WHERE AccountId=@accountId AND IsDeleted=0";
                        dataStore.SqlExecute(sql, parameters);
                    }
                    #endregion

                    #region 移除用户角色
                    {
                        var sql = $@"UPDATE dbo.AccountRoles SET IsDeleted=1 WHERE AccountId=@accountId AND IsDeleted=0";
                        dataStore.SqlExecute(sql, parameters);
                    }
                    #endregion

                    #region 移除用户安装的app
                    {
                        var sql = $@"UPDATE dbo.Members SET IsDeleted=0 WHERE AccountId=@accountId";
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

        public bool CheckAppName(string name)
        {
            Parameter.Validate(name);

            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT COUNT(*) FROM dbo.Apps AS a WHERE a.Name=@name AND a.IsDeleted=0 ";
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@name",name)
                };
                return dataStore.FindSingleValue<Int32>(sql, parameters) > 0;
            }
        }

        public bool CheckAppUrl(string url)
        {
            Parameter.Validate(url);

            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT COUNT(*) FROM dbo.Apps AS a WHERE a.AppUrl = @url AND a.IsDeleted=0";
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@url",url)
                };

                return dataStore.FindSingleValue<Int32>(sql, parameters) > 0;
            }
        }
    }
}
