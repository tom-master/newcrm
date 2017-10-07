using System;
using System.Linq;
using Microsoft.Practices.Unity;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Factory;
using NewCRM.Domain.Factory.DomainQuery.Query;
using NewCRM.Domain.Factory.DomainSpecification.Factory;
using NewCRM.Domain.UnitWork;
using NewCRM.Infrastructure.CommonTools.CustomHelper;
using System.Collections.Generic;

namespace NewCRM.Domain
{

    public class BaseServiceContext
    {
        /// <summary>
        /// 工作单元
        /// </summary>
        [Dependency]
        public IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// 数据库查询
        /// </summary>
        [Dependency("DefaultQuery")]
        public QueryBase DatabaseQuery { get; set; }

        /// <summary>
        /// 缓存查询
        /// </summary>
        [Dependency("DefaultQueryFormCache")]
        public QueryBase CacheQuery { get; set; }

        /// <summary>
        /// 规约工厂
        /// </summary>
        [Dependency]
        public SpecificationFactory FilterFactory { get; set; }

        /// <summary>
        /// 仓储工厂
        /// </summary>
        [Dependency]
        public RepositoryFactory Repository { get; set; }

        /// <summary>
        /// 参数验证
        /// </summary>
        public Parameter ValidateParameter => new Parameter();

        /// <summary>
        /// 获取桌面成员
        /// </summary>
        /// <returns></returns>
        public Member GetMember(Int32 memberId, Desk desk)
        {
            ValidateParameter.Validate(memberId).Validate(desk);
            return desk.Members.FirstOrDefault(member => member.Id == memberId);
        }

        /// <summary>
        /// 获取桌面
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Desk> GetDesks(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);
            return DatabaseQuery.Find(FilterFactory.Create((Desk desk) => desk.AccountId == accountId));
        }

        /// <summary>
        /// 获取真实的桌面编号
        /// </summary>
        /// <returns></returns>
        public static Int32 GetRealDeskId(Int32 deskId, IEnumerable<Desk> desks) => desks.FirstOrDefault(desk => desk.DeskNumber == deskId).Id;
    }
}
