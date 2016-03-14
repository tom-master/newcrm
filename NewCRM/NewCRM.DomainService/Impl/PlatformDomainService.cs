using System;
using System.Linq;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.Repositories.Repositories.Account;
using NewCRM.Infrastructure.Repositories.Repositories.Account.Impl;

namespace NewCRM.DomainService.Impl
{
    public sealed class PlatformDomainService : IPlatformDomainService
    {

        private readonly IUserRepository _userRepository;

        public PlatformDomainService()
        {
            _userRepository = new UserRepository();
        }

        public User VaildateUser(String userName, String passWord)
        {
            var result =
             _userRepository.Entities.FirstOrDefault(user => user.Name == userName && !user.IsDisable);

            if (result == null)
            {
                return null;
            }
            var isEquality = PasswordUtil.ComparePasswords(result.Password, passWord);
            return !isEquality ? null : result;
        }
    }
}