using NewCRM.Domain.Entities.DomainModel;

namespace NewCRM.QueryServices
{
    internal interface IQueryExtension<T> where T : DomainModelBase, IAggregationRoot
    {
        
    }
}
