using NewCRM.Domain.Entitys;
using NewCRM.Domain.Repositories;

namespace NewCRM.Domain.Factory
{
    public abstract class RepositoryFactory
    {
        public abstract IRepository<T> Create<T>() where T : DomainModelBase, IAggregationRoot;

    }
}
