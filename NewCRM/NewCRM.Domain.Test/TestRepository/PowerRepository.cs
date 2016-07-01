using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainModel.Security;

namespace NewCRM.Domain.Test.TestRepository
{
    internal class PowerRepository : BaseRepository<Power>
    {

        public override void Add(Power entity, bool isSave = true)
        {
            base.Add(entity);
        }

        public override void Add(IEnumerable<Power> entities, bool isSave = true)
        {
            base.Add(entities);
        }

        public override void Remove(int id, bool isSave = true)
        {
            base.Remove(id);
        }

        public override void Remove(Power entity, bool isSave = true)
        {
            base.Remove(entity);
        }

        public override void Remove(IEnumerable<Power> entities, bool isSave = true)
        {
            base.Remove(entities);
        }

        public override void Remove(Expression<Func<Power, bool>> predicate, bool isSave = true)
        {
            base.Remove(predicate);
        }

        public override void Update(Power entity, bool isSave = true)
        {
            base.Update(entity);
        }

        public override void Update(Expression<Func<Power, Boolean>> propertyExpression, Power entity, bool isSave = true)
        {
            base.Update(propertyExpression, entity);
        }
    }
}
