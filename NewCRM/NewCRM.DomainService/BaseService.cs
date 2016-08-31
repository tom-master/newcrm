using System;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.Repositories.IRepository.Account;
using NewCRM.Domain.Entities.Repositories.IRepository.Security;
using NewCRM.Domain.Entities.Repositories.IRepository.System;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services
{
    /// <summary>
    /// 基础服务实现
    /// </summary>
    internal abstract class BaseService
    {
        [Import]
        protected IAccountRepository AccountRepository { get; set; }

        [Import]
        protected IAppTypeRepository AppTypeRepository { get; set; }

        [Import]
        protected IAppRepository AppRepository { get; set; }

        [Import]
        protected IDeskRepository DeskRepository { get; set; }

        [Import]
        protected IOnlineRepository OnlineRepository { get; set; }

        [Import]
        protected IWallpaperRepository WallpaperRepository { get; set; }

        [Import]
        protected IRoleRepository RoleRepository { get; set; }

        [Import]
        protected IPowerRepository PowerRepository { get; set; }

        /// <summary>
        /// 获取一个用户
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        protected Account GetAccountInfoService(Int32 accountId)
        {
            var accountResult = AccountRepository.Entities.FirstOrDefault(account => account.Id == accountId);

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
            return accountConfig.Desks.FirstOrDefault(desk => desk.DeskNumber == deskId).Id;
        }
    }
}
