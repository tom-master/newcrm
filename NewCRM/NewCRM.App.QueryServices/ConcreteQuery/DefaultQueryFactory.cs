using System.ComponentModel.Composition;
using NewCRM.QueryServices.Query;

namespace NewCRM.QueryServices.ConcreteQuery
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
