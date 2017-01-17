using System;
using System.Collections.Generic;
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
        /// 获取桌面成员
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="desk"></param>
        /// <returns></returns>
        protected Member GetMember(Int32 memberId, Desk desk)
        {
            return desk.Members.FirstOrDefault(member => member.Id == memberId);
        }

        /// <summary>
        /// 获取当前账户下所有桌面
        /// </summary>
        /// <returns></returns>
        protected IList<Desk> GetDesks()
        {
            return Query.Find(FilterFactory.Create((Desk desk) => desk.AccountId == AccountId)).ToList();
        }


        protected Int32 AccountId { get; set; }

    }
}
