using System;
using System.ComponentModel.Composition;
using System.Net;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContext.Account;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services.BoundedContext.Account
{
    [Export(typeof(IAccountContext))]
    internal class AccountContext : BaseService.BaseService, IAccountContext
    {
        [Import]
        public IModifyDeskMemberPostionServices ModifyAccountConfigServices { get; set; }

        public Entitys.Account.Account Validate(String accountName, String password)
        {

            var accountResult = QueryFactory.Create<Entitys.Account.Account>().FindOne(SpecificationFactory.Create<Entitys.Account.Account>(account => account.Name == accountName));

            if (accountResult == null)
            {
                throw new BusinessException($"该用户不存在或被禁用{accountName}");
            }

            if (!PasswordUtil.ComparePasswords(accountResult.LoginPassword, password))
            {
                throw new BusinessException("密码错误");
            }

            accountResult.Online();

            Repository.Create<Entitys.Account.Account>().Update(accountResult);

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

            Repository.Create<Entitys.Account.Account>().Update(accountResult);

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
