using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainModel.Security;
using NewCRM.Domain.Repositories;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.Security.Impl
{
    public class PowerRepository : IPowerRepository
    {
        private readonly IRepository<Power> _repository;

        public PowerRepository()
        {
            _repository = new EfRepositoryBase<Power>();

        }

        public IQueryable<Power> Entities { get; }

        public void Add(Power entity, bool isSave = true) { _repository.Add(entity, isSave); }

        public void Add(IEnumerable<Power> entities, bool isSave = true) { _repository.Add(entities, isSave); }

        public void Remove(int id, bool isSave = true) { _repository.Remove(id, isSave); }

        public void Remove(Power entity, bool isSave = true) { _repository.Remove(entity, isSave); }

        public void Remove(IEnumerable<Power> entities, bool isSave = true) { _repository.Remove(entities, isSave); }

        public void Remove(Expression<Func<Power, bool>> predicate, bool isSave = true) { _repository.Remove(predicate, isSave); }

        public void Update(Power entity, bool isSave = true) { _repository.Update(entity, isSave); }

        public void Update(Expression<Func<Power, dynamic>> predicate, Power entity, bool isSave = true) { _repository.Update(predicate, entity, isSave); }
    }
}
