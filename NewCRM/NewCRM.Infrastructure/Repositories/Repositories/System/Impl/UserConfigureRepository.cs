using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Domain.Repositories;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.System.Impl
{
    public class UserConfigureRepository : IUserConfigureRepository
    {
        private readonly IRepository<UserConfigure> _repository;

        public UserConfigureRepository()
        {
            _repository = new EfRepositoryBase<UserConfigure>();

        }

        public IQueryable<UserConfigure> Entities { get; }

        public void Add(UserConfigure entity, bool isSave = true) { _repository.Add(entity, isSave); }

        public void Add(IEnumerable<UserConfigure> entities, bool isSave = true) { _repository.Add(entities, isSave); }

        public void Remove(int id, bool isSave = true) { _repository.Remove(id, isSave); }

        public void Remove(UserConfigure entity, bool isSave = true) { _repository.Remove(entity, isSave); }

        public void Remove(IEnumerable<UserConfigure> entities, bool isSave = true) { _repository.Remove(entities, isSave); }

        public void Remove(Expression<Func<UserConfigure, bool>> predicate, bool isSave = true) { _repository.Remove(predicate, isSave); }

        public void Update(UserConfigure entity, bool isSave = true) { _repository.Update(entity, isSave); }

        public void Update(Expression<Func<UserConfigure, dynamic>> predicate, UserConfigure entity, bool isSave = true) { _repository.Update(predicate, entity, isSave); }
    }
}
