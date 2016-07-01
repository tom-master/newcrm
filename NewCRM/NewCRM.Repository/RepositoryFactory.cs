using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using NewCRM.Domain.DomainModel;
using NewCRM.Domain.Repositories;

namespace NewCRM.Repository
{
    /// <summary>
    /// 仓储工厂
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public static class RepositoryFactory<TEntity> where TEntity : DomainModelBase, IAggregationRoot
    {
        private static readonly IDictionary<String, IRepository<TEntity>> _repositoryCache = new Dictionary<String, IRepository<TEntity>>();

        /// <summary>
        /// 创建仓储
        /// </summary>
        /// <returns></returns>
        public static IRepository<TEntity> CreateRepository()
        {
            if (_repositoryCache.ContainsKey(typeof(TEntity).Name))
            {
                return _repositoryCache[typeof(TEntity).Name];
            }


            var currentExcuteAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            var repositoryType =
                currentExcuteAssembly.GetTypes().FirstOrDefault(assembly => !assembly.Name.StartsWith("I", true, CultureInfo.InvariantCulture) && assembly.Name.EndsWith("Repository", true, CultureInfo.InvariantCulture));
            if (repositoryType == null)
            {
                return null;
            }



            var newRepositoryInstance = currentExcuteAssembly.CreateInstance(repositoryType.FullName);
            if (!(newRepositoryInstance is IRepository<TEntity>))
            {
                return null;
            }

            _repositoryCache.Add(typeof(TEntity).Name, (IRepository<TEntity>)newRepositoryInstance);
            return (IRepository<TEntity>)newRepositoryInstance;
        }
    }
}
