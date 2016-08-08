using System;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.Repositories.IRepository.Account;
using NewCRM.Domain.Entities.UnitWork;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services
{
    /// <summary>
    /// 基础服务实现
    /// </summary>
    public abstract class BaseService
    {
        protected IUnitOfWork UnitOfWork { get; set; }

        [Import]
        protected IUserRepository UserRepository { get; set; }

        /// <summary>
        /// 获取一个用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected User GetUser(Int32 userId)
        {
            var userResult = UserRepository.Entities.FirstOrDefault(user => user.Id == userId);

            if (userResult == null)
            {
                throw new BusinessException("该用户可能不存在");
            }
            return userResult;

        }

        protected Int32 GetRealDeskId(Int32 deskId, Config userConfig)
        {
            return userConfig.Desks.FirstOrDefault(desk => desk.DeskNumber == deskId).Id;
        }
    }
}
