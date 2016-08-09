using System;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.Repositories.IRepository.Account;
using NewCRM.Domain.Entities.Repositories.IRepository.System;
using NewCRM.Domain.Entities.UnitWork;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services
{
    /// <summary>
    /// 基础服务实现
    /// </summary>
    internal abstract class BaseService
    {
        protected IUnitOfWork UnitOfWork { get; set; }

        [Import]
        protected IUserRepository UserRepository { get; set; }

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


        /// <summary>
        /// 获取一个用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected User GetUserInfoService(Int32 userId)
        {
            var userResult = UserRepository.Entities.FirstOrDefault(user => user.Id == userId);

            if (userResult == null)
            {
                throw new BusinessException("该用户可能不存在");
            }
            return userResult;

        }

        /// <summary>
        /// 获取真实的桌面Id
        /// </summary>
        /// <param name="deskId"></param>
        /// <param name="userConfig"></param>
        /// <returns></returns>
        protected Int32 GetRealDeskIdService(Int32 deskId, Config userConfig)
        {
            return userConfig.Desks.FirstOrDefault(desk => desk.DeskNumber == deskId).Id;
        }


    }
}
