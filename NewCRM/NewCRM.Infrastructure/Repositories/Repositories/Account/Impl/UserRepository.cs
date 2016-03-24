using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Domain.Repositories;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.Account.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly IRepository<User> _repository;

        private readonly IQueryable<User> _entities;

        public UserRepository()
        {
            _repository = new EfRepositoryBase<User>();
        }

        public IQueryable<User> Entities => _repository.Entities.Where(user => user.IsDisable == false && user.IsDeleted == false);

        public void Add(User entity, bool isSave = true) { _repository.Add(entity, isSave); }

        public void Add(IEnumerable<User> entities, bool isSave = true) { _repository.Add(entities, isSave); }

        public void Remove(int id, bool isSave = true) { _repository.Remove(id, isSave); }

        public void Remove(User entity, bool isSave = true) { _repository.Remove(entity, isSave); }

        public void Remove(IEnumerable<User> entities, bool isSave = true) { _repository.Remove(entities, isSave); }

        public void Remove(Expression<Func<User, bool>> predicate, bool isSave = true) { _repository.Remove(predicate, isSave); }

        public void Update(User entity, bool isSave = true) { _repository.Update(entity, isSave); }


        public void Update(Expression<Func<User, dynamic>> propertyExpression, User entity, bool isSave = true) { _repository.Update(propertyExpression, entity, isSave); }
    }
}
