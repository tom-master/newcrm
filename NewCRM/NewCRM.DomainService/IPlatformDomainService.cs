using System;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Infrastructure.CommonTools;

namespace NewCRM.DomainService
{
    public interface IPlatformDomainService
    {
        User VaildateUser(String userName, String passWord);
    }
}
