using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.QueryServices.Query;

namespace NewCRM.QueryServices.ConcreteQuery
{

    [Export(typeof(QueryFactory))]
    public sealed class DefaultQueryFactory : QueryFactory
    {

        [ImportMany]
        private IEnumerable<IQuery> Query { get; set; }

        public override IQuery Create<T>()
        {
            return Query.First();
        }
    }
}
