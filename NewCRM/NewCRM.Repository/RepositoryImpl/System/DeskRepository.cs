
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.Repositories.IRepository.System;
using NewCRM.Repository.DataBaseProvider;
namespace NewCRM.Repository.RepositoryImpl.System
{
    public class DeskRepository : EntityFrameworkProvider<Desk>, IDeskRepository
    {
    }
}
