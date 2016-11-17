using System.ComponentModel.Composition;
using NewCRM.Domain.Entities.Repositories;
using NewCRM.Domain.Entities.Repositories.IRepository.Account;
using NewCRM.Repository.DataBaseProvider;

namespace NewCRM.Repository.RepositoryImpl.Account
{

    [Export(typeof(IRepository<>)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountRepository : EntityFrameworkProvider<Domain.Entities.DomainModel.Account.Account>
        , IAccountRepository
    {
    }
}
