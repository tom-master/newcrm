using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.Repositories.IRepository.Account;
using NewCRM.Repository.RepositoryProvide;

namespace NewCRM.Repository.RepositoryImpl.Account
{


    public class AccountRepository : EntityFrameworkRepository<Domain.Entities.DomainModel.Account.Account>, IAccountRepository
    {
    }
}
