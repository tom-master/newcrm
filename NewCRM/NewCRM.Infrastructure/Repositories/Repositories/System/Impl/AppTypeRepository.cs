using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Domain.Repositories;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.System.Impl
{
    public class AppTypeRepository : IAppTypeRepository
    {
        private readonly IRepository<AppType> _repository;

        public AppTypeRepository()
        {
            _repository = new EfRepositoryBase<AppType>();

        }

        public IQueryable<AppType> Entities { get; }

        public void Add(AppType entity, bool isSave = true) { _repository.Add(entity, isSave); }

        public void Add(IEnumerable<AppType> entities, bool isSave = true) { _repository.Add(entities, isSave); }

        public void Remove(int id, bool isSave = true) { _repository.Remove(id, isSave); }

        public void Remove(AppType entity, bool isSave = true) { _repository.Remove(entity, isSave); }

        public void Remove(IEnumerable<AppType> entities, bool isSave = true) { _repository.Remove(entities, isSave); }

        public void Remove(Expression<Func<AppType, bool>> predicate, bool isSave = true) { _repository.Remove(predicate, isSave); }

        public void Update(AppType entity, bool isSave = true) { _repository.Update(entity, isSave); }

        public void Update(Expression<Func<AppType, dynamic>> predicate, AppType entity, bool isSave = true) { _repository.Update(predicate, entity, isSave); }
    }
}
