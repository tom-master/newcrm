using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.Repository.StorageProvider;
using NewLib;
using NewLib.Security;

namespace NewCRM.Domain.Services.BoundedContext
{
    public class AccountContext : BaseServiceContext, IAccountContext
    {
        public Account Validate(String accountName, String password)
        {
            ValidateParameter.Validate(accountName).Validate(password);

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
                        result = dataStore.SqlGetDataTable(sql, new List<SqlParameter> { new SqlParameter("@name", accountName) }).AsSignal<Account>();
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
                        var sql = $@"UPDATE dbo.Accounts SET IsOnline=1 WHERE Id={result.Id} AND IsDeleted=0 AND IsDisable=0";
                        dataStore.SqlExecute(sql);
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
                                    (   N'{(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0]).ToString()}',       -- IpAddress - nvarchar(max)
                                        {result.Id},         -- AccountId - int
                                        {0},      -- IsDeleted - bit
                                        GETDATE(), -- AddTime - datetime
                                        GETDATE()  -- LastModifyTime - datetime
                                    )";

                        dataStore.SqlExecute(sql);
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

        public void Logout(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            using (var dataStore = new DataStore())
            {
                try
                {
                    dataStore.OpenTransaction();

                    #region 设置用户下线
                    {
                        var sql = $@"UPDATE dbo.Accounts SET IsOnline=0 WHERE Id={accountId} AND IsDeleted=0 AND IsDisable=0";
                        dataStore.SqlExecute(sql);
                    }
                    #endregion

                    #region 将当前用户从在线列表中移除
                    {
                        var sql = $@"UPDATE dbo.Onlines SET IsDeleted=1 WHERE AccountId={accountId} AND IsDeleted=0";
                        dataStore.SqlExecute(sql);
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

        public Config GetConfig(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

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
                            a.IsBing
                            FROM dbo.Configs AS a WHERE a.AccountId={accountId} AND a.IsDeleted=0";
                return dataStore.SqlGetDataTable(sql).AsSignal<Config>();
            }
        }

        public Wallpaper GetWallpaper(Int32 wallPaperId)
        {
            ValidateParameter.Validate(wallPaperId);

            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT a.Url,a.Width,a.Height,a.Source FROM dbo.Wallpapers AS a WHERE a.Id={wallPaperId} AND a.IsDeleted=0";

                return dataStore.SqlGetDataTable(sql).AsSignal<Wallpaper>();
            }
        }

        public List<Account> GetAccounts(string accountName, string accountType, int pageIndex, int pageSize, out int totalCount)
        {
            ValidateParameter.Validate(accountName).Validate(pageIndex).Validate(pageSize);

            var where = new StringBuilder();
            where.Append("WHERE 1=1 AND a.IsDeleted=0 ");
            if (!String.IsNullOrEmpty(accountName))
            {
                where.Append(" AND a.Name=@name");
            }

            if (!String.IsNullOrEmpty(accountType))
            {
                var isAdmin = (EnumExtensions.ToEnum<AccountType>(Int32.Parse(accountType)) == AccountType.Admin) ? 1 : 0;
                where.Append($@" AND a.IsAdmin={isAdmin}");
            }

            using (var dataStore = new DataStore())
            {
                #region totalCount
                {
                    var sql = $@"SELECT COUNT(*)
	                            FROM dbo.Accounts AS a 
	                            INNER JOIN dbo.Configs AS a1
	                            ON a1.AccountId=a.Id AND a1.IsDeleted=0
	                            {where} ";
                    totalCount = (Int32)dataStore.SqlScalar(sql, new List<SqlParameter> { new SqlParameter("@name", accountName) });
                }
                #endregion

                #region sql
                {
                    var sql = $@"SELECT TOP {pageSize} * FROM 
                            (
	                            SELECT ROW_NUMBER() OVER(ORDER BY a.Id DESC) AS rownumber,
                                a.Id,a.IsAdmin,a.Name,a.IsDisable,a1.AccountFace 
	                            FROM dbo.Accounts AS a 
	                            INNER JOIN dbo.Configs AS a1
	                            ON a1.AccountId=a.Id AND a1.IsDeleted=0
	                            {where} 
                            ) AS a2 WHERE a2.rownumber>{pageSize}*({pageIndex}-1)";

                    return dataStore.SqlGetDataTable(sql, new List<SqlParameter> { new SqlParameter("@name", accountName) }).AsList<Account>().ToList();
                }
                #endregion
            }
        }

        public Account GetAccount(int accountId)
        {
            ValidateParameter.Validate(accountId);

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
                            WHERE a.Id={accountId} AND a.IsDeleted=0 AND a.IsDisable=0";
                return dataStore.SqlGetDataTable(sql).AsSignal<Account>();
            }
        }

        public List<Role> GetRoles(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);
            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT
                            a1.Id,
                            a1.Name,
                            a1.RoleIdentity
                            FROM dbo.AccountRoles AS a
                            INNER JOIN dbo.Roles AS a1
                            ON a1.Id=a.RoleId AND a1.IsDeleted=0 
                            WHERE a.AccountId={accountId} AND a.IsDeleted=0 ";
                return dataStore.SqlGetDataTable(sql).AsList<Role>().ToList();
            }
        }

        public List<RolePower> GetPowers()
        {
            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT a.RoleId,a.AppId FROM dbo.RolePowers AS a WHERE a.IsDeleted=0";
                return dataStore.SqlGetDataTable(sql).AsList<RolePower>().ToList();
            }
        }

        public Boolean CheckAccountNameExist(string accountName)
        {
            ValidateParameter.Validate(accountName);

            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT COUNT(*) FROM dbo.Accounts AS a WHERE a.Name=@name AND a.IsDeleted=0";
                return (Int32)dataStore.SqlScalar(sql, new List<SqlParameter> { new SqlParameter("@name", accountName) }) != 0 ? false : true;
            }
        }

        public String GetOldPassword(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);
            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT a.LoginPassword FROM dbo.Accounts AS a WHERE a.Id={accountId} AND a.IsDeleted=0 AND a.IsDisable=0";

                return dataStore.SqlScalar(sql).ToString();
            }
        }

        public void AddNewAccount(Account account)
        {
            ValidateParameter.Validate(account);

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
                        VALUES  ( N'{config.Skin}' , -- Skin - nvarchar(max)
                                  {config.AppSize} , -- AppSize - int
                                  {config.AppVerticalSpacing} , -- AppVerticalSpacing - int
                                  {config.AppHorizontalSpacing} , -- AppHorizontalSpacing - int
                                  {config.DefaultDeskNumber} , -- DefaultDeskNumber - int
                                  {config.DefaultDeskCount} , -- DefaultDeskCount - int
                                  {(Int32)config.WallpaperMode} , -- WallpaperMode - int
                                  {(Int32)config.AppXy} , -- AppXy - int
                                  {(Int32)config.DockPosition} , -- DockPosition - int
                                  0 , -- IsDeleted - bit
                                  GETDATE() , -- AddTime - datetime
                                  GETDATE() , -- LastModifyTime - datetime
                                  3 , -- WallpaperId - int
                                  N'{config.Face}'  -- Face - nvarchar(150)
                                ) SELECT CAST(@@IDENTITY AS INT) AS Id";
                        configId = (Int32)dataStore.SqlScalar(sql);
                    }
                    #endregion

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
                              {configId} , -- ConfigId - int
                              0,  -- TitleId - int
                              0
                            ) SELECT CAST(@@IDENTITY AS INT) AS Id";
                        var parameters = new List<SqlParameter>
                        {
                            new SqlParameter("@name",account.Name),
                            new SqlParameter("@loginPassword",account.LoginPassword),
                            new SqlParameter("@lockScreenPassword",account.LockScreenPassword),
                            new SqlParameter("@isAdmin",account.IsAdmin),
                        };
                        accountId = (Int32)dataStore.SqlScalar(sql, parameters);
                    }
                    #endregion

                    #region 更新用户的配置
                    {
                        var sql = $@"UPDATE dbo.Configs SET AccountId={accountId} WHERE IsDeleted=0 AND AccountId=0";
                        dataStore.SqlExecute(sql);
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
            ValidateParameter.Validate(accountDto);

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
                            var sql = $@"UPDATE dbo.Accounts SET LoginPassword='{newPassword}' WHERE Id={accountDto.Id} AND IsDeleted=0 AND IsDisable=0";

                            dataStore.SqlExecute(sql);
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
            ValidateParameter.Validate(accountId);

            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Accounts SET IsDisable=0 WHERE Id={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void Disable(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);
            using (var dataStore = new DataStore())
            {
                #region 前置条件验证
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.Roles AS a
                                INNER JOIN dbo.AccountRoles AS a1
                                ON a1.AccountId={accountId} AND a1.RoleId=a.Id AND a1.IsDeleted=0
                                WHERE a.IsDeleted=0 AND a.IsAllowDisable=0";
                    var result = (Int32)dataStore.SqlScalar(sql);
                    if (result > 0)
                    {
                        throw new BusinessException("当前用户拥有管理员角色，因此不能禁用或删除");
                    }
                }
                #endregion
                {
                    var sql = $@"UPDATE dbo.Accounts SET IsDisable=1 WHERE Id={accountId} AND IsDeleted=0";
                    dataStore.SqlExecute(sql);
                }
            }
        }

        public void ModifyAccountFace(Int32 accountId, String newFace)
        {
            ValidateParameter.Validate(accountId).Validate(newFace);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Configs SET Face=@face WHERE AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql, new List<SqlParameter> { new SqlParameter("@face", newFace) });
            }
        }

        public void ModifyPassword(Int32 accountId, String newPassword)
        {
            ValidateParameter.Validate(accountId).Validate(newPassword);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Accounts SET LoginPassword=@password WHERE Id={accountId} AND IsDeleted=0 AND IsDisable=0";
                dataStore.SqlExecute(sql, new List<SqlParameter> { new SqlParameter("@password", newPassword) });
            }
        }

        public void ModifyLockScreenPassword(Int32 accountId, String newScreenPassword)
        {
            ValidateParameter.Validate(accountId).Validate(newScreenPassword);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Accounts SET LockScreenPassword=@password WHERE Id={accountId} AND IsDeleted=0 AND IsDisable=0";
                dataStore.SqlExecute(sql, new List<SqlParameter> { new SqlParameter("@password", newScreenPassword) });
            }
        }

        public void RemoveAccount(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            using (var dataStore = new DataStore())
            {
                dataStore.OpenTransaction();

                try
                {
                    #region 前置条件验证
                    {
                        var sql = $@"SELECT a.IsAdmin FROM dbo.Accounts AS a WHERE a.Id={accountId} AND a.IsDeleted=0 AND a.IsDisable=0";
                        var isAdmin = Boolean.Parse(dataStore.SqlScalar(sql).ToString());
                        if (isAdmin)
                        {
                            throw new BusinessException("不能删除管理员");
                        }
                    }
                    #endregion

                    #region 移除账户
                    {
                        var sql = $@"UPDATE dbo.Accounts SET IsDeleted=1 WHERE Id={accountId} AND IsDeleted=0 AND IsDisable=0";
                        dataStore.SqlExecute(sql);
                    }
                    #endregion

                    #region 移除账户配置
                    {
                        var sql = $@"UPDATE dbo.Configs SET IsDeleted=1 WHERE AccountId={accountId} AND IsDeleted=0";
                        dataStore.SqlExecute(sql);
                    }
                    #endregion

                    #region 移除用户角色
                    {
                        var sql = $@"UPDATE dbo.AccountRoles SET IsDeleted=1 WHERE AccountId={accountId} AND IsDeleted=0";
                        dataStore.SqlExecute(sql);
                    }
                    #endregion

                    #region 移除用户安装的app
                    {
                        var sql = $@"UPDATE dbo.Members SET IsDeleted=0 WHERE AccountId={accountId}";
                        dataStore.SqlExecute(sql);
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

        public Boolean UnlockScreen(Int32 accountId, String unlockPassword)
        {
            ValidateParameter.Validate(accountId).Validate(unlockPassword);
            using (var dataStore = new DataStore())
            {
                #region 获取锁屏密码
                {
                    var sql = $@"SELECT a.LockScreenPassword FROM dbo.Accounts AS a WHERE a.Id={accountId} AND a.IsDeleted=0 AND a.IsDisable=0";
                    var password = dataStore.SqlScalar(sql).ToString();
                    return PasswordUtil.ComparePasswords(password, unlockPassword);
                }
                #endregion
            }
        }
    }
}
