using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using NewCRM.Domain.Entities.Factory;
using NewCRM.Domain.Entities.Repositories;

namespace NewCRM.Repository
{
    [Export(typeof(RepositoryFactory))]
    internal sealed class DefaultRepositoryFactory : RepositoryFactory
    {
        private static readonly IDictionary<String, dynamic> _repositoryCache = new Dictionary<String, dynamic>();

        public override IRepository<T> Create<T>()
        {
            lock (_repositoryCache)
            {
                if (_repositoryCache.ContainsKey(typeof(T).Name))
                {
                    return _repositoryCache[typeof(T).Name];
                }

                var currentExcuteAssembly = System.Reflection.Assembly.GetExecutingAssembly();

                var repositoryType =
                    currentExcuteAssembly.GetTypes().FirstOrDefault(assembly => assembly.Name.EndsWith("Repository", true, CultureInfo.InvariantCulture) && assembly.Name.Contains(typeof(T).Name));

                if (repositoryType == null)
                {
                    return null;
                }

                var newRepositoryInstance = currentExcuteAssembly.CreateInstance(repositoryType.FullName);
                if (!(newRepositoryInstance is IRepository<T>))
                {
                    return null;
                }

                _repositoryCache.Add(typeof(T).Name, (IRepository<T>)newRepositoryInstance);
                return (IRepository<T>)newRepositoryInstance;
            }
        }
    }
}
