using System;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Repositories.IRepository.System;
using NewCRM.Repository.DataBaseProvider.EF;
using NewLib.Data.Mongodb;
using NewCRM.Repository.DataBaseProvider.Redis.InternalHelper;
using Microsoft.Practices.Unity;

namespace NewCRM.Repository.RepositoryImpl.System
{

    public class LogRepository : EntityFrameworkProvider<Log>, ILogRepository
    {
        //private static readonly MongoServiceApi _mongodbContext = new MongoServiceApi();
        [Dependency("ICacheQueryProvider")]
        public ICacheQueryProvider CacheQueryProvider { get; set; }

        public override void Add(Log entity, Boolean isSave = true)
        {
            // _mongodbContext.Add(entity);

            CacheQueryProvider.ListLeftPush(entity.KeyGenerator(), entity);
        }
    }
}
