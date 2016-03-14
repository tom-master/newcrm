using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NewCRM.Domain.DomainModel;
using NewCRM.Domain.Repositories;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories
{
    public class RepositoryFactory<TEntity, TRepository>
        where TEntity : EntityBase<Int32>
        where TRepository : EfRepositoryBase<TEntity, Int32>
    {
        private static IDictionary<String, IRepository<TEntity, Int32>> _repositoryCache = new ConcurrentDictionary<String, IRepository<TEntity, Int32>>();


        public static TRepository GetRepository()
        {
            var tRepositoryName = typeof(TRepository).Name;
            if (_repositoryCache.ContainsKey(tRepositoryName))
            {
                return (TRepository)_repositoryCache[typeof(TRepository).Name];
            }
            var tRepositoryInstance = Activator.CreateInstance<TRepository>();
            _repositoryCache.Add(new KeyValuePair<String, IRepository<TEntity, int>>(typeof(TRepository).Name,tRepositoryInstance));
            return tRepositoryInstance;
        }
    }
}
