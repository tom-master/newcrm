using System;
using System.Collections.Generic;
using System.ComponentModel;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.ValueObject;

namespace NewCRM.Domain.Entitys.Agent
{
    [Description("用户"), Serializable]
    public partial class Account : DomainModelBase, IAggregationRoot
    {
        #region public property

        /// <summary>
        /// 用户名
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// 登陆密码
        /// </summary>
        public String LoginPassword { get; private set; }

        /// <summary>
        /// 锁屏密码
        /// </summary>
        public String LockScreenPassword { get; private set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public Boolean IsDisable { get; private set; }

        /// <summary>
        /// 最后一次登录的时间
        /// </summary>
        public DateTime LastLoginTime { get; private set; } = DateTime.Now;

        /// <summary>
        /// 是否在线
        /// </summary>
        public Boolean IsOnline { get; private set; }

        /// <summary>
        /// 是否为管理员
        /// </summary>
        public Boolean IsAdmin { get; private set; }

        /// <summary>
        /// 职称
        /// </summary>
        public virtual Title Title { get; private set; }

        /// <summary>
        /// 用户配置
        /// </summary>
        public virtual Config Config { get; private set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public virtual ICollection<AccountRole> AccountRoles { get; private set; }

        #endregion

        #region ctor

        /// <summary>
        /// 实例化一个用户对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="accountType"></param>
        public Account(String name, String password, AccountType accountType = default(AccountType)):this()
        {
            Name = name;
            LoginPassword = password;
            IsDisable = false;
            LastLoginTime = DateTime.Now;
            LockScreenPassword = password;
            AccountRoles = new List<AccountRole>();
            Config = new Config();
            IsOnline = false;
            IsAdmin = accountType == AccountType.Admin;
        }
        public Account()
        {
            AddTime = DateTime.Now;
        }
        #endregion
    }
}
