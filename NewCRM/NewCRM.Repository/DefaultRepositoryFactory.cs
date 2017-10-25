using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NewCRM.Domain.Factory;
using NewCRM.Domain.Repositories;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Repository
{

    public sealed class DefaultRepositoryFactory : RepositoryFactory
    {

        private static readonly IDictionary<String, Type> _repositoryCache = new Dictionary<String, Type>();

        public override IRepository<T> Create<T>()
        {
            lock (_repositoryCache)
            {
                if (_repositoryCache.ContainsKey(typeof(T).Name))
                {
                    return _repositoryCache[typeof(T).Name] as IRepository<T>;
                }

                var currentExcuteAssembly = System.Reflection.Assembly.GetExecutingAssembly();
                var repositoryType =
                    currentExcuteAssembly
                    .GetTypes()
                    .FirstOrDefault(assembly => assembly.Name.EndsWith("Repository", true, CultureInfo.InvariantCulture) && assembly.Name.Contains(typeof(T).Name));

                if (repositoryType == null)
                {
                    throw new RepositoryException($"{nameof(repositoryType)}:为空");
                }

                var newRepositoryInstance = Activator.CreateInstance(repositoryType) as IRepository<T>;
                if (newRepositoryInstance == null)
                {
                    throw new RepositoryException($"{nameof(newRepositoryInstance)}:为空");
                }

                _repositoryCache.Add(typeof(T).Name, newRepositoryInstance.GetType());

                return newRepositoryInstance;
            }
        }
    }
}
