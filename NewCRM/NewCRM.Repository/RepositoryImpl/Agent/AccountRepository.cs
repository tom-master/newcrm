using System.ComponentModel.Composition;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Repositories;
using NewCRM.Domain.Repositories.IRepository.Agent;
using NewCRM.Repository.DataBaseProvider.EF;

namespace NewCRM.Repository.RepositoryImpl.Agent
{
    [Export(typeof(IRepository<>)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountRepository : EntityFrameworkProvider<Account>
        , IAccountRepository
    {
        
    }
}
