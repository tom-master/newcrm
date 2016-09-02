using NewCRM.Domain.Entities.DomainModel.Security;
using NewCRM.Domain.Entities.Repositories.IRepository.Security;
using NewCRM.Repository.RepositoryProvide;

namespace NewCRM.Repository.RepositoryImpl.Security
{
    public class RoleRepository : EntityFrameworkRepository<Role>,IRoleRepository
    {
    }
}
