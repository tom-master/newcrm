using System;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Factory;
using NewCRM.Domain.Factory.DomainQuery.Query;
using NewCRM.Domain.Factory.DomainSpecification.Factory;
using NewCRM.Domain.UnitWork;
using NewCRM.Infrastructure.CommonTools.CustomHelper;

namespace NewCRM.Domain
{
    [Export(typeof(BaseServiceContext))]
    public class BaseServiceContext
    {
        /// <summary>
        /// 工作单元
        /// </summary>
        [Import]
        public IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// 数据库查询
        /// </summary>
        [Import("EF", typeof(IQuery))]
        public IQuery DatabaseQuery { get; set; }

        /// <summary>
        /// 缓存查询
        /// </summary>
        [Import("Redis", typeof(IQuery))]
        public IQuery CacheQuery { get; set; }

        /// <summary>
        /// 规约工厂
        /// </summary>
        [Import]
        public SpecificationFactory FilterFactory { get; set; }

        /// <summary>
        /// 仓储工厂
        /// </summary>
        [Import]
        public RepositoryFactory Repository { get; set; }

        [Import("GetAccountId", typeof(Func<Int32>))]
        private Func<Int32> GetAccountId { get; set; }

        public Int32 AccountId => GetAccountId();

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
