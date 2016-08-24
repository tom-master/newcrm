using System;
using System.Collections.Generic;
using NewCRM.Domain.Entities.DomainModel.Account;

namespace NewCRM.Domain.Services
{
    public interface IAccountServices
    {
        /// <summary>
        /// 验证用户是否合法
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        User Validate(String userName, String password);

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="userId"></param>
        void Logout(Int32 userId);

        /// <summary>
        /// 用户禁用
        /// </summary>
        /// <param name="userId"></param>
        void Disable(Int32 userId);

        /// <summary>
        /// 用户启用
        /// </summary>
        /// <param name="userId"></param>
        void Enable(Int32 userId);

        /// <summary>
        /// 获取用户的配置
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        User GetUserConfig(Int32 userId);

       

    }
}
