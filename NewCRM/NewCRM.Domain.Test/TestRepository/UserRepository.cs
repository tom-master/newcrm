using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainModel.Account;

namespace NewCRM.Domain.Test.TestRepository
{

    internal class UserRepository : BaseRepository<User>
    {
        public override void Add(User entity, bool isSave = true)
        {
            base.Add(entity);
        }

        public override void Add(IEnumerable<User> entities, bool isSave = true)
        {
            base.Add(entities);
        }

        public override void Remove(int id, bool isSave = true)
        {
            base.Remove(id);
        }

        public override void Remove(User entity, bool isSave = true)
        {
            base.Remove(entity);
        }

        public override void Remove(IEnumerable<User> entities, bool isSave = true)
        {
            base.Remove(entities);
        }

        public override void Remove(Expression<Func<User, bool>> predicate, bool isSave = true)
        {
            base.Remove(predicate);
        }

        public override void Update(User entity, bool isSave = true)
        {
            base.Update(entity);
        }

        public override void Update(Expression<Func<User, Boolean>> propertyExpression, User entity, bool isSave = true)
        {
            base.Update(propertyExpression, entity);
        }
    }
}
