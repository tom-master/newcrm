using System;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.DomainSpecification.Factory;
using NewCRM.Domain.Entities.Factory;
using NewCRM.Domain.Entities.UnitWork;
using NewCRM.Infrastructure.CommonTools.CustemException;
using NewCRM.QueryServices.Query;

namespace NewCRM.Domain.Services.BaseService
{
    /// <summary>
    /// 基础服务实现
    /// </summary>
    internal abstract class BaseService
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
        protected QueryFactory QueryFactory { get; set; }

        /// <summary>
        /// 规约工厂
        /// </summary>
        [Import]
        protected SpecificationFactory SpecificationFactory { get; set; }

        /// <summary>
        /// 获取一个用户
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        protected Account GetAccountInfoService(Int32 accountId)
        {

            var accountResult = QueryFactory.Create<Account>().FindOne(SpecificationFactory.Create<Account>(account => account.Id == accountId));

            if (accountResult == null)
            {
                throw new BusinessException("该用户可能不存在");
            }
            return accountResult;

        }

        /// <summary>
        /// 获取真实的桌面Id
        /// </summary>
        /// <param name="deskId"></param>
        /// <param name="accountConfig"></param>
        /// <returns></returns>
        protected Int32 GetRealDeskIdService(Int32 deskId, Config accountConfig)
        {
            var internalDesk = accountConfig.Desks.FirstOrDefault(desk => desk.DeskNumber == deskId);

            return internalDesk?.Id ?? 0;
        }

        protected Member InternalDeskMember(Int32 memberId, Desk desk) => desk.Members.FirstOrDefault(member => member.Id == memberId);
    }
}
