using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Repositories.IRepository.System;
using NewCRM.Repository.DataBaseProvider.EF;

namespace NewCRM.Repository.RepositoryImpl.System
{
    
    public class DeskRepository : EntityFrameworkProvider<Desk>, IDeskRepository
    {
    }
}
