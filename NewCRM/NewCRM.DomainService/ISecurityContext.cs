using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using NewCRM.Domain.Entities.DomainModel.Account;

namespace NewCRM.Domain.Services
{
    public interface ISecurityContext
    {
        IRoleServices RoleServices { get; set; }

        IPowerServices PowerServices { get; set; }

        /// <summary>
        /// 添加新的用户
        /// </summary>
        /// <param name="account"></param>
        void AddNewAccount(Account account);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="account"></param>
        void ModifyAccount(Account account);
    }
}
