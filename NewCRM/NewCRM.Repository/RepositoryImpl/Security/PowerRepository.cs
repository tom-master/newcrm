
using NewCRM.Domain.Entities.DomainModel.Security;
using NewCRM.Domain.Entities.Repositories.IRepository.Security;
using NewCRM.Repository.DataBaseProvider; 

namespace NewCRM.Repository.RepositoryImpl.Security
{
    public class PowerRepository : EntityFrameworkProvider<Power>,IPowerRepository
    {
    }
}
