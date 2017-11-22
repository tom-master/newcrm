using System;
using System.Collections.Generic;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Domain.Entitys.System;

namespace NewCRM.Domain.Services.Interface
{
    public interface IAccountContext
    {
        /// <summary>
        /// 验证用户是否合法
        /// </summary>
        Account Validate(String accountName, String password);

        /// <summary>
        /// 用户登出
        /// </summary>
        void Logout(Int32 accountId);

        /// <summary>
        /// 获取配置
        /// </summary>
        Config GetConfig(Int32 accountId);

        /// <summary>
        /// 获取壁纸
        /// </summary>
        Wallpaper GetWallpaper(Int32 wallPaperId);

        /// <summary>
        /// 获取所有账户
        /// </summary>
        List<Account> GetAccounts(String accountName, String accountType, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);

        /// <summary>
        /// 获取单个账户
        /// </summary>
        Account GetAccount(Int32 accountId);

        /// <summary>
        /// 获取账户权限
        /// </summary>
        List<Role> GetRoles(Int32 accountId);

        /// <summary>
        /// 获取角色所属权限
        /// </summary>
        List<RolePower> GetPowers();

        /// <summary>
        /// 检查用户名是否重复
        /// </summary>
        Boolean CheckAccountNameExist(String accountName);
    }
}
