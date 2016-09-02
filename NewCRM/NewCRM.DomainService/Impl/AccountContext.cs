using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services.Impl
{
    [Export(typeof(IAccountContext))]
    internal class AccountContext : BaseService, IAccountContext
    {
        [Import]
        public IAccountConfigServices ConfigServices { get; set; }

        public Account Validate(String accountName, String password)
        {
            var a = TestRepositories;

            var accountResult = AccountRepository.Entities.FirstOrDefault(account => account.Name == accountName);
            if (accountResult == null)
            {
                throw new BusinessException($"该用户不存在或被禁用{accountName}");
            }
            if (!PasswordUtil.ComparePasswords(accountResult.LoginPassword, password))
            {
                throw new BusinessException("密码错误");
            }

            accountResult.Online();

            AccountRepository.Update(accountResult);

            OnlineRepository.Add(new Online(GetCurrentIpAddress(), accountResult.Id));

            return accountResult;

        }

        public void Logout(Int32 accountId)
        {
            var accountResult = GetAccountInfoService(accountId);

            if (!accountResult.IsOnline)
            {
                throw new BusinessException("该用户可能已在其他地方下线");
            }
            accountResult.Offline();

            AccountRepository.Update(accountResult);

            ModifyOnlineState(accountId);
        }

        #region private method

        /// <summary>
        /// 获取当前登陆的ip
        /// </summary>
        /// <returns></returns>
        private String GetCurrentIpAddress()
        {
            IPHostEntry localhost = Dns.GetHostEntry(Dns.GetHostName());
            return (localhost.AddressList[0]).ToString();
        }

        /// <summary>
        /// 修改在线状态
        /// </summary>
        /// <param name="accountId"></param>
        private void ModifyOnlineState(Int32 accountId)
        {
            var onlineResult = OnlineRepository.Entities.FirstOrDefault(online => online.AccountId == accountId);
            OnlineRepository.Remove(onlineResult);
        }

        #endregion
    }
}
