using NewCRM.Domain.Entitys;

namespace NewCRM.Domain.DomainQuery.Query
{
    public abstract class QueryFactory
    {
        public abstract IQuery Create<T>() where T : DomainModelBase, IAggregationRoot;
    }
}
