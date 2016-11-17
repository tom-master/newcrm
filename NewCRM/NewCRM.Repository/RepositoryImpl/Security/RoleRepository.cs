using System.ComponentModel.Composition;
using NewCRM.Domain.Entities.DomainModel.Security;
using NewCRM.Domain.Entities.Repositories;
using NewCRM.Domain.Entities.Repositories.IRepository.Security;
using NewCRM.Repository.DataBaseProvider;

namespace NewCRM.Repository.RepositoryImpl.Security
{
    [Export(typeof(IRepository<>)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class RoleRepository : EntityFrameworkProvider<Role>,IRoleRepository
    {
    }
}
