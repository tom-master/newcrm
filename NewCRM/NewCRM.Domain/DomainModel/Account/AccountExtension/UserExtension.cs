using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.Security;


namespace NewCRM.Domain.Entities.DomainModel.Account
{
    public partial class User
    {
        #region public method

        /// <summary>
        /// 禁用当前用户
        /// </summary>
        public User Disable()
        {
            IsDisable = true;
            return this;
        }

        /// <summary>
        /// 启用当前用户
        /// </summary>
        public User Enable()
        {
            IsDisable = false;
            return this;
        }

        /// <summary>
        /// 添加职称
        /// </summary>
        /// <param name="newTitle"></param>
        public User AddTitle(Title newTitle)
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
        /// <param name="roles"></param>
        public User AddUserRole(params Role[] roles)
        {
            return AddUserRole(roles.Select(r => r.Id).ToArray());
        }

        /// <summary>
        /// 添加用户角色
        /// </summary>
        /// <param name="roleIds"></param>
        public User AddUserRole(params Int32[] roleIds)
        {
            if (roleIds == null)
            {
                throw new ArgumentNullException($"{nameof(roleIds)}:不能为空");
            }
            if (roleIds.Length <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(roleIds)}:不能为0");
            }

            foreach (var roleId in roleIds)
            {
                Roles.Add(new UserRole(Id, roleId));
            }
            return this;
        }

        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public User RemoveUserRole(params Role[] roles)
        {
            return RemoveUserRole(roles.Select(role => role.Id).ToArray());
        }

        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public User RemoveUserRole(params Int32[] roleIds)
        {
            if (roleIds == null)
            {
                throw new ArgumentNullException($"{nameof(roleIds)}:不能为空");
            }
            if (roleIds.Length <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(roleIds)}:不能为0");
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
        /// <returns></returns>
        public User Online()
        {
            IsOnline = true;
            return this;
        }

        /// <summary>
        /// 离线
        /// </summary>
        /// <returns></returns>
        public User Offline()
        {
            IsOnline = false;
            return this;
        }

        public void RemoveTitle()
        {
            Title.Remove();
        }

        public void Remove()
        {
            IsDeleted = true;
        }

        #endregion
    }
}
