using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Domain.Repositories;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.System.Impl
{
    public class OnlineRepository : IOnlineRepository
    {
        private readonly IRepository<Online> _repository;

        public OnlineRepository()
        {
            _repository = new EfRepositoryBase<Online>();

        }

        public IQueryable<Online> Entities { get; }

        public void Add(Online entity, bool isSave = true) { _repository.Add(entity, isSave); }

        public void Add(IEnumerable<Online> entities, bool isSave = true) { _repository.Add(entities, isSave); }

        public void Remove(int id, bool isSave = true) { _repository.Remove(id, isSave); }

        public void Remove(Online entity, bool isSave = true) { _repository.Remove(entity, isSave); }

        public void Remove(IEnumerable<Online> entities, bool isSave = true) { _repository.Remove(entities, isSave); }

        public void Remove(Expression<Func<Online, bool>> predicate, bool isSave = true) { _repository.Remove(predicate, isSave); }

        public void Update(Online entity, bool isSave = true) { _repository.Update(entity, isSave); }

        public void Update(Expression<Func<Online, dynamic>> predicate, Online entity, bool isSave = true) { _repository.Update(predicate, entity, isSave); }
    }
}
