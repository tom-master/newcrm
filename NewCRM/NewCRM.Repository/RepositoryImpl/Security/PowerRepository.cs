using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.DomainModel.Security;
using NewCRM.Domain.Repositories.IRepository.Security;

using NewCRM.Repository.RepositoryProvide;

namespace NewCRM.Repository.RepositoryImpl.Security
{
    public class PowerRepository : EfRepositoryBase<Power>,IPowerRepository
    {
    }
}
