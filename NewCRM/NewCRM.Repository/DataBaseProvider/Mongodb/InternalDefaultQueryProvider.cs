using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.Factory.DomainSpecification;
using NewCRM.Domain.Repositories;
using NewLib.Data.Mongodb;

namespace NewCRM.Repository.DataBaseProvider.Mongodb
{
    [Export("Mongodb", typeof(IDomainModelQueryProvider))]
    internal sealed class InternalDefaultQueryProvider : IDomainModelQueryProvider
    {
        private static readonly MongoServiceApi _mongoServiceApi = new MongoServiceApi();

        public IQueryable<T> Query<T>(Specification<T> selector) where T : DomainModelBase, IAggregationRoot
        {
            return _mongoServiceApi.FindAll<T>().Where(selector.Expression);
        }
    }
}
