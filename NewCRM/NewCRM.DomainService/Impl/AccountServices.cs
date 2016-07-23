using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.Repositories.IRepository.Account;
using NewCRM.Domain.Entities.Repositories.IRepository.System;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services.Impl
{
    [Export(typeof(IAccountServices))]
    public class AccountServices : BaseService,IAccountServices
    {
        #region private field

      

        [Import]
        private IOnlineRepository _onlineRepository;
        #endregion

        #region public method

        public User Validate(String userName, String password)
        {
            var userResult = UserRepository.Entities.FirstOrDefault(user => user.Name == userName && user.IsDisable == false && user.IsDeleted == false);
            if (userResult == null)
            {
                throw new BusinessException($"该用户不存在或被禁用{userName}");
            }
            if (!PasswordUtil.ComparePasswords(userResult.LoginPassword, password))
            {
                throw new BusinessException("密码错误");
            }
            userResult.Online();

            UserRepository.Update(userResult);

            _onlineRepository.Add(new Online(GetCurrentIpAddress(), userResult.Id));

            return userResult;

        }

        public void Logout(Int32 userId)
        {
            var userResult = GetUser(userId);

            if (!userResult.IsOnline)
            {
                throw new BusinessException("该用户可能已在其他地方下线");
            }
            userResult.Offline();

            UserRepository.Update(userResult);

            ModifyOnlineState(userId);
        }

        public void Disable(Int32 userId)
        {
            var userResult = GetUser(userId);

            if (userResult.IsDisable)
            {
                throw new BusinessException("该用户可能已在其他地方被禁用");
            }

            userResult.Disable();
            userResult.Offline();

            UserRepository.Update(userResult);

            ModifyOnlineState(userId);
        }

        public void Enable(Int32 userId)
        {
            var userResult = GetUser(userId);
            userResult.Enable();

            UserRepository.Update(userResult);
        }

        public User GetUserConfig(Int32 userId)
        {
            var userResult = GetUser(userId);

            return userResult;
        }

        #endregion

        #region private method

        /// <summary>
        /// 获取当前登陆的ip
        /// </summary>
        /// <returns></returns>
        private String GetCurrentIpAddress()
        {
            IPHostEntry localhost = Dns.GetHostEntry(Dns.GetHostName());
            return (localhost.AddressList[0]).ToString();
        }

        /// <summary>
        /// 修改在线状态
        /// </summary>
        /// <param name="userId"></param>
        private void ModifyOnlineState(Int32 userId)
        {
            var onlineResult = _onlineRepository.Entities.FirstOrDefault(online => online.UserId == userId);
            _onlineRepository.Remove(onlineResult);
        }

        #endregion
    }
}
