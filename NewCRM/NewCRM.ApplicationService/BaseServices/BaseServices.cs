using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainSpecification.Factory;
using NewCRM.Domain.Entities.Factory;
using NewCRM.Domain.Entities.UnitWork;
using NewCRM.Domain.Interface.BoundedContext.Account;
using NewCRM.Domain.Interface.BoundedContext.App;
using NewCRM.Domain.Interface.BoundedContext.Desk;
using NewCRM.Domain.Interface.BoundedContext.Wallpaper;
using NewCRM.Infrastructure.CommonTools.CustomHelper;
using NewCRM.QueryServices.Query;

//using NewCRM.Infrastructure.CommonTools.CustomHelper;
//using NewCRM.QueryServices.Query;

namespace NewCRM.Application.Services.BaseServices
{

    public abstract class BaseServices
    {
        [Import]
        protected IUnitOfWork UnitOfWork { get; set; }

        [Import]
        protected IAccountContext AccountContext { get; set; }

        [Import]
        protected IAppContext AppContext { get; set; }

        [Import]
        protected IDeskContext DeskContext { get; set; }

        [Import]
        protected IWallpaperContext WallpaperContext { get; set; }


        /// <summary>
        /// 仓储工厂
        /// </summary>
        [Import]
        protected RepositoryFactory Repository { get; set; }

        /// <summary>
        /// 查询工厂
        /// </summary>
        [ImportMany]
        protected IEnumerable<QueryFactory> QueryFactory { get; set; }

        /// <summary>
        /// 规约工厂
        /// </summary>
        [ImportMany]
        protected IEnumerable<SpecificationFactory> SpecificationFactory { get; set; }

        /// <summary>
        /// 参数验证
        /// </summary>
        protected static Parameter ValidateParameter => new Parameter();

        /// <summary>
        /// 获取登陆的账户
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        internal Account GetAccountInfoService(Int32 accountId)
        {
            return QueryFactory.First().Create<Account>().FindOne(SpecificationFactory.First().Create<Account>(account => account.Id == accountId));
        }
    }
}
