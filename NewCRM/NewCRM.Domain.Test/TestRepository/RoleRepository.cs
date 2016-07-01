using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainModel.Security;

namespace NewCRM.Domain.Test.TestRepository
{
    internal class RoleRepository : BaseRepository<Role>
    {


        public override void Add(Role entity, bool isSave = true)
        {
            base.Add(entity);
        }

        public override void Add(IEnumerable<Role> entities, bool isSave = true)
        {
            base.Add(entities);
        }

        public override void Remove(int id, bool isSave = true)
        {
            base.Remove(id);
        }

        public override void Remove(Role entity, bool isSave = true)
        {
            base.Remove(entity);
        }

        public override void Remove(IEnumerable<Role> entities, bool isSave = true)
        {
            base.Remove(entities);
        }

        public override void Remove(Expression<Func<Role, bool>> predicate, bool isSave = true)
        {
            base.Remove(predicate);
        }

        public override void Update(Role entity, bool isSave = true)
        {
            base.Update(entity);
        }

        public override void Update(Expression<Func<Role, Boolean>> propertyExpression, Role entity, bool isSave = true)
        {
            base.Update(propertyExpression, entity);
        }
    }
}
