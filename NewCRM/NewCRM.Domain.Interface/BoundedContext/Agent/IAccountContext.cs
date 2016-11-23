using System;
using System.ComponentModel.Composition;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Interface.BoundedContextMember;

namespace NewCRM.Domain.Interface.BoundedContext.Agent
{
    public interface IAccountContext
    {
        [Import]
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
