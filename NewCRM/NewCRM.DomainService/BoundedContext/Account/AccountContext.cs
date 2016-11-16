using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using NewCRM.Domain.Entities.DomainModel.System;
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

        public Entities.DomainModel.Account.Account Validate(String accountName, String password)
        {

            var accountResult = QueryFactory.Create<Entities.DomainModel.Account.Account>().FindOne(SpecificationFactory.Create<Entities.DomainModel.Account.Account>(account => account.Name == accountName));

            if (accountResult == null)
            {
                throw new BusinessException($"该用户不存在或被禁用{accountName}");
            }

            if (!PasswordUtil.ComparePasswords(accountResult.LoginPassword, password))
            {
                throw new BusinessException("密码错误");
            }

            accountResult.Online();

            Repository.Create<Entities.DomainModel.Account.Account>().Update(accountResult);

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

            Repository.Create<Entities.DomainModel.Account.Account>().Update(accountResult);

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
            var onlineResult = QueryFactory.Create<Online>().FindOne(SpecificationFactory.Create<Online>(online => online.AccountId == accountId));

            Repository.Create<Online>().Remove(onlineResult);

        }

        #endregion
    }
}
