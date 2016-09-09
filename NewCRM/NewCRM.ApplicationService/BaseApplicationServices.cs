using System;
using System.ComponentModel.Composition;
using NewCRM.Domain.Entities.DomainSpecification.Factory;
using NewCRM.Domain.Services;
using NewCRM.Infrastructure.CommonTools.CustomHelper;
using NewCRM.QueryServices.Query;

namespace NewCRM.Application.Services
{
    internal class BaseApplicationServices
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

        protected static Parameter ValidateParameter => new Parameter();

    }
}
