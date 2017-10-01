using System;
using System.ComponentModel.Composition;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Repositories;
using NewCRM.Domain.Repositories.IRepository.System;
using NewCRM.Repository.DataBaseProvider.EF;
using NewLib.Data.Mongodb;

namespace NewCRM.Repository.RepositoryImpl.System
{
    
    public class LogRepository : EntityFrameworkProvider<Log>, ILogRepository
    {
        private static readonly MongoServiceApi _mongodbContext = new MongoServiceApi();

        public override void Add(Log entity, Boolean isSave = true)
        {
            _mongodbContext.Add(entity);
        }
    }
}
