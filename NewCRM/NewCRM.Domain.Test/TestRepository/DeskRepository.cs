using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.DomainModel.System;

namespace NewCRM.Domain.Test.TestRepository
{
    internal class DeskRepository : BaseRepository<Desk>
    {
        public override void Add(Desk entity, bool isSave = true)
        {
            base.Add(entity);
        }

        public override void Add(IEnumerable<Desk> entities, bool isSave = true)
        {
            base.Add(entities);
        }

        public override void Remove(int id, bool isSave = true)
        {
            base.Remove(id);
        }

        public override void Remove(Desk entity, bool isSave = true)
        {
            base.Remove(entity);
        }

        public override void Remove(IEnumerable<Desk> entities, bool isSave = true)
        {
            base.Remove(entities);
        }

        public override void Remove(Expression<Func<Desk, bool>> predicate, bool isSave = true)
        {
            base.Remove(predicate);
        }

        public override void Update(Desk entity, bool isSave = true)
        {
            base.Update(entity);
        }

        public override void Update(Expression<Func<Desk, Boolean>> propertyExpression, Desk entity, bool isSave = true)
        {
            base.Update(propertyExpression, entity);
        }
    }
}
