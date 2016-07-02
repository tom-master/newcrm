using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.Repositories.IRepository.Account;
using NewCRM.Domain.Entities.Repositories.IRepository.System;
using NewCRM.Infrastructure.CommonTools;

namespace NewCRM.Domain.Services.Impl
{
    [Export(typeof(IAccountServices))]
    public class AccountServices : IAccountServices
    {

        [Import]
        private IUserRepository _useRepository;


        private readonly IOnlineRepository _onlineRepository;

        public Boolean Validate(String userName, String password)
        {
            try
            {
                var userResult = _useRepository.Entities.FirstOrDefault(user => user.Name == userName);
                if (userResult == null)
                {
                    return false;
                }
                if (!PasswordUtil.ComparePasswords(userResult.LoginPassword, password))
                {
                    return false;
                }
                userResult.Online();

                _onlineRepository.Add(new Online(GetCurrentIpAddress(), userResult.Id));

                return true;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public void Logout(Int32 userId)
        {
            var userResult = _useRepository.Entities.FirstOrDefault(user => user.Id == userId);

            if (!userResult.IsOnline)
            {
                return;
            }
            userResult.Offline();
            _useRepository.Update(userResult);

            ModifyOnlineState(userId);
        }



        public void Disable(Int32 userId)
        {
            var userResult = _useRepository.Entities.FirstOrDefault(user => user.Id == userId);


            userResult.Disable();
            userResult.Offline();

            _useRepository.Update(userResult);

            ModifyOnlineState(userId);
        }

        public void Enable(Int32 userId)
        {
            var userResult = _useRepository.Entities.FirstOrDefault(user => user.Id == userId);
            userResult.Enable();

            _useRepository.Update(userResult);
        }



        #region private method

        private String GetCurrentIpAddress()
        {
            IPHostEntry localhost = Dns.GetHostEntry(Dns.GetHostName());
            return (localhost.AddressList[0]).ToString();
        }

        private void ModifyOnlineState(Int32 userId)
        {
            var onlineResult = _onlineRepository.Entities.FirstOrDefault(online => online.UserId == userId);
            _onlineRepository.Remove(onlineResult);
        }
        #endregion
    }
}
