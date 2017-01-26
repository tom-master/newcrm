using System;
using System.ComponentModel.Composition;
using System.Net;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContext.Agent;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Domain.Services.BoundedContext.Agent
{
    [Export(typeof(IAccountContext))]
    internal class AccountContext : IAccountContext
    {
        [Import]
        public BaseServiceContext BaseContext { get; set; }

        public Account Validate(String accountName, String password)
        {
            var specification = BaseContext.FilterFactory.Create<Account>(account => account.Name == accountName);

            var accountResult = BaseContext.Query.FindOne(specification);

            if (accountResult == null)
            {
                throw new BusinessException($"该用户不存在或被禁用{accountName}");
            }

            if (!PasswordUtil.ComparePasswords(accountResult.LoginPassword, password))
            {
                throw new BusinessException("密码错误");
            }

            accountResult.Online();

            BaseContext.Repository.Create<Account>().Update(accountResult);

            BaseContext.Repository.Create<Online>().Add(new Online(GetCurrentIpAddress(), accountResult.Id));

            return accountResult;

        }

        public void Logout(Int32 accountId)
        {
            var accountResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create((Account account) => account.Id == accountId));

            if (!accountResult.IsOnline)
            {
                throw new BusinessException("该用户可能已在其他地方下线");
            }
            accountResult.Offline();

            BaseContext.Repository.Create<Account>().Update(accountResult);

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
            var onlineResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create<Online>(online => online.AccountId == accountId));

            BaseContext.Repository.Create<Online>().Remove(onlineResult);

        }

        #endregion
    }
}
