using System.ComponentModel.Composition;
using NewCRM.Domain.DomainQuery.Query;

namespace NewCRM.Domain.DomainQuery.ConcreteQuery
{

    [Export(typeof(QueryFactory))]
    public sealed class DefaultQueryFactory : QueryFactory
    {

        [Import]
        private IQuery Query { get; set; }

        public override IQuery Create<T>() => Query;

    }
}
