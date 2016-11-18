using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using NewCRM.Domain.Factory;
using NewCRM.Domain.Repositories;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Repository
{
    [Export(typeof(RepositoryFactory))]
    internal sealed class DefaultRepositoryFactory : RepositoryFactory
    {
        [ImportMany(typeof(IRepository<>))]
        private IEnumerable<dynamic> RepositoryFactory { get; set; }

        private static readonly IDictionary<String, Type> _repositoryCache = new Dictionary<String, Type>();


        public override IRepository<T> Create<T>()
        {
            lock (_repositoryCache)
            {
                if (_repositoryCache.ContainsKey(typeof(T).Name))
                {
                    return RepositoryFactory.FirstOrDefault(w => w.GetType().FullName == _repositoryCache[typeof(T).Name].FullName);
                }

                var currentExcuteAssembly = System.Reflection.Assembly.GetExecutingAssembly();

                var repositoryType =
                    currentExcuteAssembly.GetTypes().FirstOrDefault(assembly => assembly.Name.EndsWith("Repository", true, CultureInfo.InvariantCulture) && assembly.Name.Contains(typeof(T).Name));

                if (repositoryType == null)
                {
                    throw new RepositoryException($"{nameof(repositoryType)}:为空");
                }

                var newRepositoryInstance = RepositoryFactory.FirstOrDefault(w => w.GetType().FullName == repositoryType.FullName) as IRepository<T>;

                if (newRepositoryInstance == null)
                {
                    throw new RepositoryException($"{nameof(newRepositoryInstance)}:为空");
                }

                _repositoryCache.Add(typeof(T).Name, repositoryType);

                return newRepositoryInstance;
            }
        }
    }
}
