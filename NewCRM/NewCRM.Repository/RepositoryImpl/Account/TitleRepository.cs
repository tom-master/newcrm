using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.Repositories.IRepository.Account;
using NewCRM.Repository.DataBaseProvider;

namespace NewCRM.Repository.RepositoryImpl.Account
{

    public class TitleRepository : EntityFrameworkProvider<Title>, ITitleRepository
    {
    }
}
