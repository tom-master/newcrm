using System;
using NewCRM.Domain.Entities.DomainModel.Account;

namespace NewCRM.Domain.Interface
{
    public interface IAccountContext
    {

        IModifyDeskMemberPostionServices ModifyAccountConfigServices { get; set; }

        /// <summary>
        /// 验证用户是否合法
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Account Validate(String accountName, String password);

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="accountId"></param>
        void Logout(Int32 accountId);
    }
}
