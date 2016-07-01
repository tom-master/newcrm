using System;
using NewCRM.Domain.DomainModel.Security;

namespace NewCRM.Domain.DomainModel.Account
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
        /// 添加角色
        /// </summary>
        /// <param name="roles"></param>
        public User AddRole(params Role[] roles)
        {
            if (roles == null)
            {
                throw new ArgumentNullException($"{nameof(roles)}:不能为空");
            }
            if (roles.Length <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(roles)}:不能为0");
            }

            foreach (var role in roles)
            {
                Roles.Add(new UserRole(Id, role.Id));
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




        #endregion
    }
}
