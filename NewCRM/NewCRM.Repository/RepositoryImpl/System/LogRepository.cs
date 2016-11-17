
using System.ComponentModel.Composition;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.Repositories;
using NewCRM.Domain.Entities.Repositories.IRepository.System;
using NewCRM.Repository.DataBaseProvider;

namespace NewCRM.Repository.RepositoryImpl.System
{

    [Export(typeof(IRepository<>)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class LogRepository : EntityFrameworkProvider<Log>, ILogRepository
    {
    }
}
