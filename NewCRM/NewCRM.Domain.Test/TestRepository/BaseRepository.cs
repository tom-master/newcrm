using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainModel;
using NewCRM.Domain.Repositories;
using NewCRM.Repository.RepositoryProvide;

namespace NewCRM.Domain.Test.TestRepository
{
    internal abstract class BaseRepository<T> : EfRepositoryBase<T> where T : DomainModelBase, IAggregationRoot
    {
       
    }
}
