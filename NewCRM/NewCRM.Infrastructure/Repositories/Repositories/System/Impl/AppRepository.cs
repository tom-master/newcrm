using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Domain.Repositories;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.System.Impl
{
    public class AppRepository : IAppRepository
    {
        private readonly IRepository<App> _repository;

        public AppRepository()
        {
            _repository = new EfRepositoryBase<App>();

        }

        public IQueryable<App> Entities { get; }

        public void Add(App entity, bool isSave = true) { _repository.Add(entity, isSave); }

        public void Add(IEnumerable<App> entities, bool isSave = true) { _repository.Add(entities, isSave); }

        public void Remove(int id, bool isSave = true) { _repository.Remove(id, isSave); }

        public void Remove(App entity, bool isSave = true) { _repository.Remove(entity, isSave); }

        public void Remove(IEnumerable<App> entities, bool isSave = true) { _repository.Remove(entities, isSave); }

        public void Remove(Expression<Func<App, bool>> predicate, bool isSave = true) { _repository.Remove(predicate, isSave); }

        public void Update(App entity, bool isSave = true) { _repository.Update(entity, isSave); }

        public void Update(Expression<Func<App, dynamic>> predicate, App entity, bool isSave = true) { _repository.Update(predicate, entity, isSave); }
    }
}
