using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using NewCRM.Domain.DomainModel;
using NewCRM.Domain.Repositories;

namespace NewCRM.Infrastructure.Repositories
{
    public class RepositoryFactory<TEntity> where TEntity : EntityBase<Int32>
    {
        private static readonly IDictionary<String, IRepository<TEntity>> _repositoryCache = new Dictionary<String, IRepository<TEntity>>();


        public static IRepository<TEntity> CreateRepository()
        {
            var currentExcuteAssembly = Assembly.GetExecutingAssembly();
            var repositoryType =
                currentExcuteAssembly.GetTypes().FirstOrDefault(assembly => !assembly.Name.StartsWith("I", true, CultureInfo.InvariantCulture) && assembly.Name.EndsWith("Repository", true, CultureInfo.InvariantCulture) && assembly.Name == typeof(TEntity).Name + "Repository");
            if (repositoryType == null)
            {
                return null;
            }

            if (_repositoryCache.ContainsKey(repositoryType.Name))
            {
                return _repositoryCache[repositoryType.Name];
            }

            var newRepositoryInstance = currentExcuteAssembly.CreateInstance(repositoryType.FullName);
            if (!(newRepositoryInstance is IRepository<TEntity>))
            {
                return null;
            }

            _repositoryCache.Add(repositoryType.Name, (IRepository<TEntity>)newRepositoryInstance);
            return (IRepository<TEntity>)newRepositoryInstance;
        }
    }
}
