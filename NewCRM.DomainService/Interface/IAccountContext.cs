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
        Account Validate(String accountName, String password,String requestIp);

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

        /// <summary>
        /// 检查密码
        /// </summary>
        String GetOldPassword(Int32 accountId);

        /// <summary>
        /// 添加新账户
        /// </summary>
        void AddNewAccount(Account accountDto);

        /// <summary>
        /// 修改账户
        /// </summary>
        void ModifyAccount(Account accountDto);

        /// <summary>
        /// 账户启用
        /// </summary>
        void Enable(Int32 accountId);

        /// <summary>
        /// 账户禁用
        /// </summary>
        void Disable(Int32 accountId);

        /// <summary>
        /// 修改账户头像
        /// </summary>
        void ModifyAccountFace(Int32 accountId, String newFace);

        /// <summary>
        /// 修改账户密码
        /// </summary>
        void ModifyPassword(Int32 accountId, String newPassword);

        /// <summary>
        /// 修改锁屏密码
        /// </summary>
        void ModifyLockScreenPassword(Int32 accountId, String newScreenPassword);

        /// <summary>
        /// 移除账户
        /// </summary>
        void RemoveAccount(Int32 accountId);

        /// <summary>
        /// 解除屏幕锁定
        /// </summary>
        Boolean UnlockScreen(Int32 accountId, String unlockPassword);
    }
}
