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
        /// 添加职称
        /// </summary>
        public Account AddTitle(Title newTitle)
        {
            if (newTitle == null)
            {
                throw new BusinessException($"{nameof(newTitle)}:不能为空");
            }
            Title = newTitle;
            return this;
        }

        /// <summary>
        /// 添加用户角色
        /// </summary>
        public Account AddRole(params Int32[] roleIds)
        {
            if (roleIds == null)
            {
                throw new BusinessException($"{nameof(roleIds)}:不能为空");
            }

            if (!roleIds.Any())
            {
                throw new BusinessException($"{nameof(roleIds)}:不能为0");
            }

            foreach (var roleId in roleIds)
            {
                Roles.Add(new AccountRole(Id, roleId));
            }

            return this;
        }

        /// <summary>
        /// 删除用户角色
        /// </summary>
        public Account RemoveRole(params Int32[] roleIds)
        {
            if (roleIds == null)
            {
                throw new BusinessException($"{nameof(roleIds)}:不能为空");
            }
            if (!roleIds.Any())
            {
                throw new BusinessException($"{nameof(roleIds)}:不能为0");
            }
            foreach (var roleId in roleIds)
            {
                Roles.FirstOrDefault(r => r.Id == roleId).Remove();
            }
            return this;
        }

        /// <summary>
        /// 在线
        /// </summary>
        public Account Online()
        {
            IsOnline = true;
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
