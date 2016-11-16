using NewCRM.Domain.Entities.DomainModel;

namespace NewCRM.Domain.Entities.DomainQuery.Query
{
    public abstract class QueryFactory
    {
        public abstract IQuery Create<T>() where T : DomainModelBase, IAggregationRoot;
    }
}
