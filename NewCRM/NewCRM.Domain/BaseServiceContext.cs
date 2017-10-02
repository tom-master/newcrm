using System;
using System.Linq;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Factory;
using NewCRM.Domain.Factory.DomainQuery.Query;
using NewCRM.Domain.Factory.DomainSpecification.Factory;
using NewCRM.Domain.UnitWork;
using NewCRM.Infrastructure.CommonTools.CustomHelper;

namespace NewCRM.Domain
{
    
    public class BaseServiceContext
    {
        /// <summary>
        /// 工作单元
        /// </summary>
        public IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// 数据库查询
        /// </summary>
        public QueryBase DatabaseQuery { get; set; }

        /// <summary>
        /// 缓存查询
        /// </summary>
        public QueryBase CacheQuery { get; set; }

        /// <summary>
        /// 规约工厂
        /// </summary>
        public SpecificationFactory FilterFactory { get; set; }

        /// <summary>
        /// 仓储工厂
        /// </summary>
        public RepositoryFactory Repository { get; set; }

        /// <summary>
        /// 参数验证
        /// </summary>
        public Parameter ValidateParameter => new Parameter();

        /// <summary>
        /// 获取桌面成员
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="desk"></param>
        /// <returns></returns>
        public Member GetMember(Int32 memberId, Desk desk)
        {
            return desk.Members.FirstOrDefault(member => member.Id == memberId);
        }
    }
}
