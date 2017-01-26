using System;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.DomainQuery.Query;
using NewCRM.Domain.DomainSpecification.Factory;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Factory;
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
        /// 查询工厂
        /// </summary>
        [Import]
        public IQuery Query { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        [Import("GetAccountId", typeof(Func<Int32>))]
        public Func<Int32> GetAccountId { get; set; }

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
