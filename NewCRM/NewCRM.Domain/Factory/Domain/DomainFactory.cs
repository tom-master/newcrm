using NewCRM.Domain.Entities.DomainModel;

namespace NewCRM.Domain.Entities.Factory.Domain
{
    public abstract class DomainFactory
    {
        public abstract T Create<T>() where T : DomainModelBase, IAggregationRoot, new();
    }
}
