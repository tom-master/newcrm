using System.ComponentModel.Composition;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Domain.Repositories;
using NewCRM.Domain.Repositories.IRepository.Security;
using NewCRM.Repository.DataBaseProvider; 

namespace NewCRM.Repository.RepositoryImpl.Security
{

    [Export(typeof(IRepository<>)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class PowerRepository : EntityFrameworkProvider<Power>,IPowerRepository
    {
    }
}
