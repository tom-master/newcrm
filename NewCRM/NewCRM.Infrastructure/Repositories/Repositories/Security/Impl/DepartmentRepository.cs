using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainModel.Security;
using NewCRM.Domain.Repositories;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.Security.Impl
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IRepository<Department> _repository;

        public DepartmentRepository()
        {
            _repository = new EfRepositoryBase<Department>();

        }

        public IQueryable<Department> Entities { get; }

        public void Add(Department entity, bool isSave = true) { _repository.Add(entity, isSave); }

        public void Add(IEnumerable<Department> entities, bool isSave = true) { _repository.Add(entities, isSave); }

        public void Remove(int id, bool isSave = true) { _repository.Remove(id, isSave); }

        public void Remove(Department entity, bool isSave = true) { _repository.Remove(entity, isSave); }

        public void Remove(IEnumerable<Department> entities, bool isSave = true) { _repository.Remove(entities, isSave); }

        public void Remove(Expression<Func<Department, bool>> predicate, bool isSave = true) { _repository.Remove(predicate, isSave); }

        public void Update(Department entity, bool isSave = true) { _repository.Update(entity, isSave); }

        public void Update(Expression<Func<Department, dynamic>> predicate, Department entity, bool isSave = true) { _repository.Update(predicate, entity, isSave); }
    }
}
