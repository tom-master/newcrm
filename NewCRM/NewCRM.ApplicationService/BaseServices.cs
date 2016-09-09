using System;
using System.ComponentModel.Composition;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainSpecification.Factory;
using NewCRM.Domain.Services;
using NewCRM.Infrastructure.CommonTools.CustomHelper;
using NewCRM.QueryServices.Query;

namespace NewCRM.Application.Services
{
    internal class BaseServices
    {

        [Import]
        protected IAccountContext AccountContext { get; set; }

        [Import]
        protected IAppServices AppServices { get; set; }

        [Import]
        protected IWallpaperServices WallpaperServices { get; set; }

        [Import]
        protected ISecurityContext SecurityContext { get; set; }

        /// <summary>
        /// 查询工厂
        /// </summary>
        [Import]
        protected QueryFactory QueryFactory { get; set; }

        /// <summary>
        /// 规约工厂
        /// </summary>
        [Import]
        protected SpecificationFactory SpecificationFactory { get; set; }

        /// <summary>
        /// 参数验证
        /// </summary>
        protected static Parameter ValidateParameter => new Parameter();

        /// <summary>
        /// 获取登陆的账户
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        internal Account GetLoginAccount(Int32 accountId)
        {
            return QueryFactory.Create<Account>().FindOne(SpecificationFactory.Create<Account>(account => account.Id == accountId));
        }
    }
}
