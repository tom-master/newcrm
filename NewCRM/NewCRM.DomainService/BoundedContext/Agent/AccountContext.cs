using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Repositories.IRepository.Agent;
using NewCRM.Domain.Repositories.IRepository.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustomException;
using System;
using System.Net;

namespace NewCRM.Domain.Services.BoundedContext.Agent
{
    public class AccountContext : BaseServiceContext, IAccountContext
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IOnlineRepository _onlineRepository;

        public AccountContext(IAccountRepository accountRepository, IOnlineRepository onlineRepository)
        {
            _accountRepository = accountRepository;
            _onlineRepository = onlineRepository;
        }

        public Account Validate(String accountName, String password)
        {
            ValidateParameter.Validate(accountName).Validate(password);

            var filter = FilterFactory.Create<Account>(acccount => acccount.Name == accountName);
            var accountResult = DatabaseQuery.FindOne(filter, account => new
            {
                account.Id,
                account.LoginPassword,
                account.Name
            });

            if (accountResult == null)
            {
                throw new BusinessException($"该用户不存在或被禁用{accountName}");
            }

            if (!PasswordUtil.ComparePasswords(accountResult.LoginPassword, password))
            {
                throw new BusinessException("密码错误");
            }

            accountResult.Online();
            _accountRepository.Update(accountResult);
            _onlineRepository.Add(new Online(GetCurrentIpAddress(), accountResult.Id));

            return accountResult;
        }

        public void Logout(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);
            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == accountId));

            if (!accountResult.IsOnline)
            {
                throw new BusinessException("该用户可能已在其他地方下线");
            }
            accountResult.Offline();
            _accountRepository.Update(accountResult);
            ModifyOnlineState(accountId);
        }

        #region private method

        /// <summary>
        /// 获取当前登陆的ip
        /// </summary>
        /// <returns></returns>
        private String GetCurrentIpAddress() => (Dns.GetHostEntry(Dns.GetHostName()).AddressList[0]).ToString();

        /// <summary>
        /// 修改在线状态
        /// </summary>
        /// <param name="accountId"></param>
        private void ModifyOnlineState(Int32 accountId)
        {
            var onlineResult = DatabaseQuery.FindOne(FilterFactory.Create<Online>(online => online.AccountId == accountId));
            _onlineRepository.Remove(onlineResult);
        }

        #endregion
    }
}
