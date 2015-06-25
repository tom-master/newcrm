using System;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.Account.Impl
{
    public class UserRepository : EfRepositoryBase<User, Int32>, IUserRepository
    {

    }
}
