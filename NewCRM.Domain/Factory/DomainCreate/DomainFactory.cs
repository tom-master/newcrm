using NewCRM.Domain.Entitys;

namespace NewCRM.Domain.Factory.DomainCreate
{
    public abstract class DomainFactory
    {
        public abstract T Create<T>() where T : DomainModelBase;
    }
}
