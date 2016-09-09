using NewCRM.Domain.Entities.Repositories.IRepository.Account;
using NewCRM.Repository.DataBaseProvider;

namespace NewCRM.Repository.RepositoryImpl.Account
{


    public class AccountRepository : EntityFrameworkProvider<Domain.Entities.DomainModel.Account.Account>, IAccountRepository
    {
    }
}
