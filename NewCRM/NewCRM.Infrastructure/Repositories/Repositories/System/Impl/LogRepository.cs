using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Domain.Repositories;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.System.Impl
{
    public class LogRepository : ILogRepository
    {
        private readonly IRepository<Log> _repository;

        public LogRepository()
        {
            _repository = new EfRepositoryBase<Log>();

        }

        public IQueryable<Log> Entities { get; }

        public void Add(Log entity, bool isSave = true) { _repository.Add(entity, isSave); }

        public void Add(IEnumerable<Log> entities, bool isSave = true) { _repository.Add(entities, isSave); }

        public void Remove(int id, bool isSave = true) { _repository.Remove(id, isSave); }

        public void Remove(Log entity, bool isSave = true) { _repository.Remove(entity, isSave); }

        public void Remove(IEnumerable<Log> entities, bool isSave = true) { _repository.Remove(entities, isSave); }

        public void Remove(Expression<Func<Log, bool>> predicate, bool isSave = true) { _repository.Remove(predicate, isSave); }

        public void Update(Log entity, bool isSave = true) { _repository.Update(entity, isSave); }

        public void Update(Expression<Func<Log, dynamic>> predicate, Log entity, bool isSave = true) { _repository.Update(predicate, entity, isSave); }
    }
}
