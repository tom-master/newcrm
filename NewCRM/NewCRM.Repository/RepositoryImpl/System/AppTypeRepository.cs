using System.ComponentModel.Composition;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Repositories;
using NewCRM.Domain.Repositories.IRepository.System;
using NewCRM.Repository.DataBaseProvider; 

namespace NewCRM.Repository.RepositoryImpl.System
{

    [Export(typeof(IRepository<>)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class AppTypeRepository : EntityFrameworkProvider<AppType>, IAppTypeRepository
    {
    }
}
