using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NewCRM.Domain.DomainModel;
using NewCRM.Domain.Repositories;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories
{
    public class RepositoryFactory<TEntity, TRepository>
        where TEntity : EntityBase<Int32>, IAggregationRoot
        where TRepository : EfRepositoryBase<TEntity, Int32>
    {
        private static IDictionary<String, IRepository<TEntity, Int32>> _repositoryCache = new ConcurrentDictionary<String, IRepository<TEntity, Int32>>();

        /// <summary>
        /// 根据传入的聚合根和仓储来获取相对应的仓储实例
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TRepository"></typeparam>
        /// <returns></returns>
        public static TRepository GetRepository()
        {
            var tRepositoryName = typeof(TRepository).Name;
            if (_repositoryCache.ContainsKey(tRepositoryName))//检查当前仓储是否在缓存中
            {
                return (TRepository)_repositoryCache[typeof(TRepository).Name];//返回缓存中的仓储
            }
            var tRepositoryInstance = Activator.CreateInstance<TRepository>();//创建一个仓储实例
            _repositoryCache.Add(new KeyValuePair<string, IRepository<TEntity, int>>(typeof(TRepository).Name,
                tRepositoryInstance));//将新建的仓储实例添加到缓存中
            return tRepositoryInstance;//返回新创建的仓储实例
        }
    }
}
