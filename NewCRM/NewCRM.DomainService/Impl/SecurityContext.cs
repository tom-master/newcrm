using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services.Impl
{
    [Export(typeof(ISecurityContext))]
    internal sealed class SecurityContext : BaseService, ISecurityContext
    {
        [Import]
        public IRoleServices RoleServices { get; set; }

        [Import]
        public IPowerServices PowerServices { get; set; }

        #region Account

        public void AddNewAccount(Account account)
        {
            AccountType accountType;

            var enumConst = Enum.GetName(typeof(AccountType), account.IsAdmin ? "2"/*管理员*/ : "1"/*用户*/);

            if (!Enum.TryParse(enumConst, true, out accountType))
            {
                throw new BusinessException($"类型{enumConst}不是有效的枚举类型");
            }

            var internalNewAccount = new Account(account.Name, PasswordUtil.CreateDbPassword(account.LoginPassword), accountType);

            internalNewAccount.AddRole(account.Roles.Select(role => role.RoleId).ToArray());

            AccountRepository.Add(internalNewAccount);
        }

        public void ModifyAccount(Account account)
        {
            var accountResult = AccountRepository.Entities.FirstOrDefault(internalAccount => internalAccount.Id == account.Id);

            if (accountResult == null)
            {
                throw new BusinessException($"用户{account.Name}可能已被禁用或删除");
            }

            if ((account.LoginPassword + "").Length > 0)
            {
                var newPassword = PasswordUtil.CreateDbPassword(account.LoginPassword);
                accountResult.ModifyPassword(newPassword);
            }

            if (accountResult.Roles.Any())
            {
                accountResult.Roles.ToList().ForEach(role =>
                {
                    role.Remove();
                });
            }

            accountResult.AddRole(account.Roles.Select(role => role.RoleId).ToArray());

            AccountRepository.Update(accountResult);
        }

        #endregion
    }
}
