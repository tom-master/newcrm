using System;
using System.Collections.Generic;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;

namespace NewCRM.Domain.Services.Interface
{
    public interface IAccountContext
    {


        /// <summary>
        /// 验证用户是否合法
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Account Validate(String accountName, String password);

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="accountId"></param>
        void Logout(Int32 accountId);

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        Config GetConfig(Int32 accountId);

        /// <summary>
        /// 获取壁纸
        /// </summary>
        /// <returns></returns>
        Wallpaper GetWallpaper(Int32 wallPaperId);

        /// <summary>
        /// 获取所有账户
        /// </summary>
        /// <returns></returns>
        List<Account> GetAccounts(String accountName, String accountType, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);
    }
}
