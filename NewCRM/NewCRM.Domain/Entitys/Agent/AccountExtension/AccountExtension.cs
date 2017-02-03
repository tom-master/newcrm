using System;
using System.Linq;

namespace NewCRM.Domain.Entitys.Agent
{
    public partial class Account
    {
        #region public method

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public Account ModifyPassword(String newPassword)
        {
            LoginPassword = newPassword;
            return this;
        }

        /// <summary>
        /// 修改锁屏密码
        /// </summary>
        /// <param name="newLockScreenPassword"></param>
        /// <returns></returns>
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
        /// 添加职称
        /// </summary>
        /// <param name="newTitle"></param>
        public Account AddTitle(Title newTitle)
        {
            if (newTitle == null)
            {
                throw new ArgumentNullException($"{nameof(newTitle)}:不能为空");
            }
            Title = newTitle;
            return this;
        }

        /// <summary>
        /// 添加用户角色
        /// </summary>
        /// <param name="roleIds"></param>
        public Account AddRole(params Int32[] roleIds)
        {
            if (roleIds == null)
            {
                throw new ArgumentNullException($"{nameof(roleIds)}:不能为空");
            }

            if (!roleIds.Any())
            {
                throw new BusinessException($"{nameof(roleIds)}:不能为0");
            }

            foreach (var roleId in roleIds)
            {
                AccountRoles.Add(new AccountRole(Id, roleId));
            }

            return this;
        }

        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public Account RemoveRole(params Int32[] roleIds)
        {
            if (roleIds == null)
            {
                throw new ArgumentNullException($"{nameof(roleIds)}:不能为空");
            }
            if (!roleIds.Any())
            {
                throw new BusinessException($"{nameof(roleIds)}:不能为0");
            }
            foreach (var roleId in roleIds)
            {
                AccountRoles.FirstOrDefault(r => r.Id == roleId).Remove();
            }
            return this;
        }

        /// <summary>
        /// 在线
        /// </summary>
        /// <returns></returns>
        public Account Online()
        {
            IsOnline = true;
            return this;
        }

        /// <summary>
        /// 离线
        /// </summary>
        /// <returns></returns>
        public Account Offline()
        {
            IsOnline = false;
            return this;
        }

        /// <summary>
        /// 移除职称
        /// </summary>
        public Account RemoveTitle()
        {
            Title.Remove();
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
