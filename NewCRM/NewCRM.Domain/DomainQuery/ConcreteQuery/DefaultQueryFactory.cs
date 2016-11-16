using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainQuery.Query;

namespace NewCRM.Domain.Entities.DomainQuery.ConcreteQuery
{

    [Export(typeof(QueryFactory))]
    public sealed class DefaultQueryFactory : QueryFactory
    {

        [Import]
        private IQuery Query { get; set; }

        public override IQuery Create<T>()
        {
            return Query;
        }
    }
}
