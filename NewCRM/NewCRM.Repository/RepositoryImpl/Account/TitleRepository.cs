using System.ComponentModel.Composition;
using NewCRM.Domain.Entitys.Account;
using NewCRM.Domain.Repositories;
using NewCRM.Domain.Repositories.IRepository.Account;
using NewCRM.Repository.DataBaseProvider;

namespace NewCRM.Repository.RepositoryImpl.Account
{
    [Export(typeof(IRepository<>)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class TitleRepository : EntityFrameworkProvider<Title>, ITitleRepository
    {
    }
}
