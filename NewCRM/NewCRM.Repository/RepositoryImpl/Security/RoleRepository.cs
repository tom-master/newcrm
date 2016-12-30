using System.ComponentModel.Composition;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Domain.Repositories;
using NewCRM.Domain.Repositories.IRepository.Security;
using NewCRM.Repository.DataBaseProvider;
using NewCRM.Repository.DataBaseProvider.EF;

namespace NewCRM.Repository.RepositoryImpl.Security
{
    [Export(typeof(IRepository<>)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class RoleRepository : EntityFrameworkProvider<Role>,IRoleRepository
    {
    }
}
