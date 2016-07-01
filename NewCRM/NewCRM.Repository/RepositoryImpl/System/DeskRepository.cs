using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Domain.Repositories.IRepository.System;

using NewCRM.Repository.RepositoryProvide;

namespace NewCRM.Repository.RepositoryImpl.System
{
    public class DeskRepository : EfRepositoryBase<Desk>,IDeskRepository
    {
    }
}
