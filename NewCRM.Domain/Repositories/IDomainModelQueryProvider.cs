using System.Linq;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.Factory.DomainSpecification;

namespace NewCRM.Domain.Repositories
{
    public interface IDomainModelQueryProvider
    {
        IQueryable<T> Query<T>(Specification<T> selector) where T : DomainModelBase;
    }
}
