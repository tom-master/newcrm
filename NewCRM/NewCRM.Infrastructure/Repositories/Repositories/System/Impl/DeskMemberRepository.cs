using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Domain.Repositories;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.System.Impl
{
    public class DeskMemberRepository : IDeskMemberRepository
    {
        private readonly IRepository<DeskMember> _repository;

        public DeskMemberRepository()
        {
            _repository = new EfRepositoryBase<DeskMember>();

        }

        public IQueryable<DeskMember> Entities
        {
            get
            {
                return _repository.Entities.Where(deskMember => deskMember.IsDeleted == false);
            }
        }

        public void Add(DeskMember entity, bool isSave = true) { _repository.Add(entity, isSave); }

        public void Add(IEnumerable<DeskMember> entities, bool isSave = true) { _repository.Add(entities, isSave); }

        public void Remove(int id, bool isSave = true) { _repository.Remove(id, isSave); }

        public void Remove(DeskMember entity, bool isSave = true) { _repository.Remove(entity, isSave); }

        public void Remove(IEnumerable<DeskMember> entities, bool isSave = true) { _repository.Remove(entities, isSave); }

        public void Remove(Expression<Func<DeskMember, bool>> predicate, bool isSave = true) { _repository.Remove(predicate, isSave); }

        public void Update(DeskMember entity, bool isSave = true) { _repository.Update(entity, isSave); }

        public void Update(Expression<Func<DeskMember, dynamic>> predicate, DeskMember entity, bool isSave = true) { _repository.Update(predicate, entity, isSave); }
    }
}
