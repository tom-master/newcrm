using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.DomainQuery.Query;
using NewCRM.Domain.DomainSpecification.Factory;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Factory;

namespace NewCRM.Domain.Services.Service
{
    /// <summary>
    /// 基础服务实现
    /// </summary>
    internal class BaseService
    {

        /// <summary>
        /// 获取仓储工厂
        /// </summary>
        [Import]
        protected RepositoryFactory Repository { get; set; }

        /// <summary>
        /// 查询工厂
        /// </summary>
        [Import]
        protected IQuery Query { get; set; }

        /// <summary>
        /// 规约工厂
        /// </summary>
        [Import]
        protected SpecificationFactory FilterFactory { get; set; }

 


        protected Member InternalDeskMember(Int32 memberId, Desk desk)
        {
            return desk.Members.FirstOrDefault(member => member.Id == memberId);
        }

        [Export(typeof(Int32))]
        protected Int32 AccountId { get; set; }
    }
}
