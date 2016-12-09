using System;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.DomainQuery.Query;
using NewCRM.Domain.DomainSpecification.Factory;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Factory;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Domain.Services.Service
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
        protected IQuery Query { get; set; }

        /// <summary>
        /// 规约工厂
        /// </summary>
        [Import]
        protected SpecificationFactory FilterFactory { get; set; }

        /// <summary>
        /// 获取一个用户
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        protected Account GetAccountInfoService(Int32 accountId)
        {

            var accountResult = Query.FindOne(FilterFactory.Create<Account>(account => account.Id == accountId));

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
