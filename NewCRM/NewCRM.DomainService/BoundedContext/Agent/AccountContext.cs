using System;
using System.ComponentModel.Composition;
using System.Net;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContext.Agent;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Domain.Services.Service;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services.BoundedContext.Agent
{
    [Export(typeof(IAccountContext))]
    internal class AccountContext : BaseService, IAccountContext
    {
        [Import]
        public IModifyDeskMemberPostionServices ModifyAccountConfigServices { get; set; }

        public Account Validate(String accountName, String password)
        {

            var accountResult = QueryFactory.Create<Account>()
                .FindOne(SpecificationFactory
                .Create<Account>(account => account.Name == accountName)
                , account => new { account.Id, account.LoginPassword });

            if (accountResult == null)
            {
                throw new BusinessException($"该用户不存在或被禁用{accountName}");
            }

            if (!PasswordUtil.ComparePasswords(accountResult.LoginPassword, password))
            {
                throw new BusinessException("密码错误");
            }

            accountResult.Online();

            Repository.Create<Account>().Update(accountResult);

            Repository.Create<Online>().Add(new Online(GetCurrentIpAddress(), accountResult.Id));

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

            Repository.Create<Account>().Update(accountResult);

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
            var onlineResult = QueryFactory.Create<Online>().FindOne(SpecificationFactory.Create<Online>(online => online.AccountId == accountId));

            Repository.Create<Online>().Remove(onlineResult);

        }

        #endregion
    }
}
