using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Repositories.IRepository.Agent;
using NewCRM.Repository.DataBaseProvider.EF;

namespace NewCRM.Repository.RepositoryImpl.Agent
{
    public class AccountRepository : EntityFrameworkProvider<Account>
        , IAccountRepository
    {
        
    }
}
