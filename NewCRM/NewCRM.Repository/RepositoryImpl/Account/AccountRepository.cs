using System.ComponentModel.Composition;
using NewCRM.Domain.Repositories;
using NewCRM.Domain.Repositories.IRepository.Account;
using NewCRM.Repository.DataBaseProvider;

namespace NewCRM.Repository.RepositoryImpl.Account
{

    [Export(typeof(IRepository<>)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountRepository : EntityFrameworkProvider<Domain.Entitys.Account.Account>
        , IAccountRepository
    {
    }
}
