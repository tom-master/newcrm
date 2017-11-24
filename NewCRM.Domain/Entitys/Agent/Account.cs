using NewCRM.Domain.ValueObject;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace NewCRM.Domain.Entitys.Agent
{
    [Description("用户"), Serializable]
    public partial class Account : DomainModelBase
    {
        #region public property

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public String Name { get; private set; }

        /// <summary>
        /// 登陆密码
        /// </summary>
        [Required, MinLength(6)]
        public String LoginPassword { get; private set; }

        /// <summary>
        /// 锁屏密码
        /// </summary>
        [MinLength(6)]
        public String LockScreenPassword { get; private set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public Boolean IsDisable { get; private set; }

        /// <summary>
        /// 最后一次登录的时间
        /// </summary>
        public DateTime LastLoginTime { get; private set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public Boolean IsOnline { get; private set; }

        /// <summary>
        /// 是否为管理员
        /// </summary>
        public Boolean IsAdmin { get; private set; }

        /// <summary>
        /// 头像
        /// </summary>
        public String Face { get; set; }


        /// <summary>
        /// 用户角色
        /// </summary>
        public IEnumerable<AccountRole> Roles { get; private set; }
        #endregion

        #region ctor

        /// <summary>
        /// 实例化一个用户对象
        /// </summary>
        public Account(String name, String password, IEnumerable<AccountRole> roles, AccountType accountType = default(AccountType))
        {
            Name = name;
            LoginPassword = password;
            IsDisable = false;
            LastLoginTime = DateTime.Now;
            LockScreenPassword = password;
            IsOnline = false;
            IsAdmin = accountType == AccountType.Admin;
            Roles = roles;
        }

        public Account() { }

        #endregion
    }
}
