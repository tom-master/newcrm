﻿using System;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entitys.Agent
{
    public partial class AccountRole : DomainModelBase
    {
        #region private field
        private Int32 _accountId;

		private Int32 _roleId;
		#endregion

        #region public property
        [Required]
        public Int32 AccountId
        {
            get
            {
                return _accountId;
            }
            private set
            {
                _accountId = value;
            }
        }

        [Required]
        public Int32 RoleId
        {
            get
            {
                return _roleId;
            }
            private set
            {
                _roleId = value;
            }
        }

		#endregion

		#region ctor

		public AccountRole(Int32 accountId, Int32 roleId)
		{
			AccountId = accountId;
			RoleId = roleId;
		}

		public AccountRole() { }

        #endregion
    }

	/// <summary>
	/// AccountRoleExtension
	/// </summary>
	public partial class AccountRole
	{

	}
}
