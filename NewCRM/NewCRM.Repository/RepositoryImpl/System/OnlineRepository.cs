using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.Repositories.IRepository.System;
using NewCRM.Repository.RepositoryProvide;

namespace NewCRM.Repository.RepositoryImpl.System
{
    public class OnlineRepository : EfRepositoryBase<Online>,IOnlineRepository
    {
    }
}
