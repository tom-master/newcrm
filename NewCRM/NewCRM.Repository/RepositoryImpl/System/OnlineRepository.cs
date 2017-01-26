using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Repositories.IRepository.System;
using NewCRM.Repository.DataBaseProvider;

namespace NewCRM.Repository.RepositoryImpl.System
{

    public class OnlineRepository : EntityFrameworkProvider<Online>,IOnlineRepository
    {
    }
}
