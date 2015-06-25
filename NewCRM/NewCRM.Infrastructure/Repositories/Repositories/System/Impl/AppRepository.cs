using System;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.System.Impl
{
    public class AppRepository : EfRepositoryBase<App, Int32>, IAppRepository
    {
    }
}
