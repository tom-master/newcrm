﻿using System;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entitys.Agent
{
    public class AccountRole : DomainModelBase
    {
        #region public property
        [Required]
        public Int32 AccountId { get; private set; }

        [Required]
        public Int32 RoleId { get; private set; }


        #endregion

        #region ctor

        public AccountRole(Int32 accountId, Int32 roleId)
        {
            AccountId = accountId;
            RoleId = roleId; 
        }

        public AccountRole() { }

        public void Remove() => IsDeleted = true;

        #endregion
    }
}
