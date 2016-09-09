using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entities.DomainModel;

namespace NewCRM.QueryServices.Query
{
    public abstract class QueryFactory
    {
        public abstract IQuery Create<T>() where T : DomainModelBase, IAggregationRoot;
    }
}
