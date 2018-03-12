using NewCRM.Domain.ValueObject;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace NewCRM.Domain.Entitys.Agent
{
	[Description("用户"), Serializable]
	public partial class Account: DomainModelBase
	{
		#region private field
		/// <summary>
		/// 用户名
		/// </summary>
		private String _name;

		/// <summary>
		/// 登陆密码
		/// </summary> 
		private String _loginPassword;

		/// <summary>
		/// 锁屏密码
		/// </summary>  
		private String _lockScreenPassword;

		/// <summary>
		/// 是否禁用
		/// </summary>
		private Boolean _isDisable;

		/// <summary>
		/// 最后一次登录的时间
		/// </summary>
		private DateTime _lastLoginTime;

		/// <summary>
		/// 是否在线
		/// </summary>
		private Boolean _isOnline;

		/// <summary>
		/// 是否为管理员
		/// </summary>
		private Boolean _isAdmin;

		/// <summary>
		/// 用户角色
		/// </summary>
		private IEnumerable<AccountRole> _roles;
		#endregion

		#region public property

		/// <summary>
		/// 用户名
		/// </summary>
		[Required]
		public String Name
		{
			get
			{
				return _name;
			}
			private set
			{
				if (_name != value)
				{
					_name = value;
					OnPropertyChanged(Name);
				}
			}
		}


		/// <summary>
		/// 登陆密码
		/// </summary>
		[MinLength(6)]
		public String LoginPassword
		{
			get
			{
				return _loginPassword;
			}
			private set
			{
				if (_loginPassword != value)
				{
					_loginPassword = value;
					OnPropertyChanged(LoginPassword);
				}
			}
		}

		/// <summary>
		/// 锁屏密码
		/// </summary>
		[MinLength(6)]
		public String LockScreenPassword
		{
			get
			{
				return _lockScreenPassword;
			}
			private set
			{
				if (_lockScreenPassword != value)
				{
					_lockScreenPassword = value;
					OnPropertyChanged(LockScreenPassword);
				}
			}
		}

		/// <summary>
		/// 是否禁用
		/// </summary>
		public Boolean IsDisable
		{
			get
			{
				return _isDisable;
			}
			private set
			{
				if (_isDisable != value)
				{
					_isDisable = value;
					OnPropertyChanged("IsDisable");
				}
			}
		}

		/// <summary>
		/// 最后一次登录的时间
		/// </summary>
		public DateTime LastLoginTime
		{
			get
			{
				return _lastLoginTime;
			}
			private set
			{
				if (_lastLoginTime != value)
				{
					_lastLoginTime = value;
					OnPropertyChanged("LastLoginTime");
				}
			}
		}

		/// <summary>
		/// 是否在线
		/// </summary>
		public Boolean IsOnline
		{
			get
			{
				return _isOnline;
			}
			private set
			{
				if (_isOnline != value)
				{
					_isOnline = value;
					OnPropertyChanged("IsOnline");
				}
			}
		}

		/// <summary>
		/// 是否为管理员
		/// </summary>
		public Boolean IsAdmin
		{
			get
			{
				return _isAdmin;
			}
			private set
			{
				if (_isAdmin != value)
				{
					_isAdmin = value;
					OnPropertyChanged("IsAdmin");
				}
			}
		}

		public String AccountFace { get; private set; }

		/// <summary>
		/// 用户角色
		/// </summary>
		public IEnumerable<AccountRole> Roles
		{
			get
			{
				return _roles;
			}
			private set
			{
				if (_roles != value)
				{
					_roles = value;
					OnPropertyChanged("Roles");
				}
			}
		}
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
