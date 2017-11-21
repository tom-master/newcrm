using System;
using System.Linq;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Domain.Entitys.Agent
{
    public partial class Account
    {
        #region public method

        /// <summary>
        /// 修改密码
        /// </summary>
        public Account ModifyPassword(String newPassword)
        {
            LoginPassword = newPassword;
            return this;
        }

        /// <summary>
        /// 修改锁屏密码
        /// </summary>
        public Account ModifyLockScreenPassword(String newLockScreenPassword)
        {
            LockScreenPassword = newLockScreenPassword;
            return this;
        }

        /// <summary>
        /// 禁用当前用户
        /// </summary>
        public Account Disable()
        {
            IsDisable = true;
            return this;
        }

        /// <summary>
        /// 启用当前用户
        /// </summary>
        public Account Enable()
        {
            IsDisable = false;
            return this;
        }

        /// <summary>
        /// 在线
        /// </summary>
        public Account Online()
        {
            IsOnline = true;
            LastLoginTime = DateTime.Now;
            return this;
        }

        /// <summary>
        /// 离线
        /// </summary>
        public Account Offline()
        {
            IsOnline = false;
            return this;
        }

        /// <summary>
        /// 移除用户
        /// </summary>
        public void Remove()
        {
            IsDeleted = true;
        }

        public override String KeyGenerator()
        {
            return $"NewCRM:{nameof(Account)}:Id:{Id}";
        }

        #endregion
    }
}
