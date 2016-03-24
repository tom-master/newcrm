using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Domain.Repositories;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.Account.Impl
{
    public class TitleRepository : ITitleRepository
    {
        private readonly IRepository<Title> _repository;

        public TitleRepository()
        {
            _repository = new EfRepositoryBase<Title>();

        }

        public IQueryable<Title> Entities { get; }

        public void Add(Title entity, bool isSave = true) { _repository.Add(entity, isSave); }

        public void Add(IEnumerable<Title> entities, bool isSave = true) { _repository.Add(entities, isSave); }

        public void Remove(int id, bool isSave = true) { _repository.Remove(id, isSave); }

        public void Remove(Title entity, bool isSave = true) { _repository.Remove(entity, isSave); }

        public void Remove(IEnumerable<Title> entities, bool isSave = true) { _repository.Remove(entities, isSave); }

        public void Remove(Expression<Func<Title, bool>> predicate, bool isSave = true) { _repository.Remove(predicate, isSave); }

        public void Update(Title entity, bool isSave = true) { _repository.Update(entity, isSave); }

        public void Update(Expression<Func<Title, dynamic>> predicate, Title entity, bool isSave = true) { _repository.Update(predicate, entity, isSave); }
    }
}
