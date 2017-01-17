using System;
using System.Collections.Generic;
using NewCRM.Domain.DomainQuery.Query;
using NewCRM.Domain.DomainSpecification.Factory;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Factory;
using NewCRM.Domain.UnitWork;
using NewCRM.Infrastructure.CommonTools.CustomHelper;

namespace NewCRM.Application.Services.Services
{
    internal class BaseService
    {
        protected Int32 AccountId { get; set; }

        /// <summary>
        /// 工作单元
        /// </summary>
        protected IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// 仓储工厂
        /// </summary>
        protected RepositoryFactory Repository { get; set; }

        /// <summary>
        /// 查询工厂
        /// </summary>
        protected IQuery Query { get; set; }

        /// <summary>
        /// 规约工厂
        /// </summary>
        protected SpecificationFactory FilterFactory { get; set; }

        /// <summary>
        /// 参数验证
        /// </summary>
        protected static Parameter ValidateParameter => new Parameter();

        /// <summary>
        /// 
        /// </summary>
        protected Func<IList<Desk>> GetDesks { get; set; }
    }
}
