using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entities.DomainModel;
using NewCRM.Domain.Entities.Repositories;

namespace NewCRM.Domain.Entities.Factory
{
    public abstract class RepositoryFactory
    {
        public abstract IRepository<T> Create<T>() where T : DomainModelBase, IAggregationRoot;
    }
}
