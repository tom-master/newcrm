using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainModel.Security;
using NewCRM.Domain.Repositories;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.Security.Impl
{
    public class RoleRepository : IRoleRepository
    {

        private readonly IRepository<Role> _repository;

        public RoleRepository()
        {
            _repository = new EfRepositoryBase<Role>();

        }

        public IQueryable<Role> Entities { get; }

        public void Add(Role entity, bool isSave = true) { _repository.Add(entity, isSave); }

        public void Add(IEnumerable<Role> entities, bool isSave = true) { _repository.Add(entities, isSave); }

        public void Remove(int id, bool isSave = true) { _repository.Remove(id, isSave); }

        public void Remove(Role entity, bool isSave = true) { _repository.Remove(entity, isSave); }

        public void Remove(IEnumerable<Role> entities, bool isSave = true) { _repository.Remove(entities, isSave); }

        public void Remove(Expression<Func<Role, bool>> predicate, bool isSave = true) { _repository.Remove(predicate, isSave); }

        public void Update(Role entity, bool isSave = true) { _repository.Update(entity, isSave); }

        public void Update(Expression<Func<Role, dynamic>> predicate, Role entity, bool isSave = true) { _repository.Update(predicate, entity, isSave); }
    }
}
