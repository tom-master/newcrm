using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.Repository.StorageProvider;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;

namespace NewCRM.Domain.Services.BoundedContext.Agent
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
                    dataStore.UseTransaction = true;

                    #region 查询用户
                    {
                        var sql = @"SELECT a.Id,a.Name,a.LoginPassword FROM dbo.Accounts AS a WHERE a.Name=@name";
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
                    (   N'{GetCurrentIpAddress()}',       -- IpAddress - nvarchar(max)
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
                catch (Exception ex)
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
                    dataStore.UseTransaction = true;

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
                catch (Exception ex)
                {
                    dataStore.Rollback();
                    throw;
                }
            }
        }

        #region private method

        /// <summary>
        /// 获取当前登陆的ip
        /// </summary>
        /// <returns></returns>
        private String GetCurrentIpAddress() => (Dns.GetHostEntry(Dns.GetHostName()).AddressList[0]).ToString();
        #endregion
    }
}
