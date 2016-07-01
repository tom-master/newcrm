using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Domain.Repositories;

namespace NewCRM.Domain.Test.TestRepository
{
    internal class OnlineRepository : BaseRepository<Online>
    {

        public override void Add(Online entity, bool isSave = true)
        {
            base.Add(entity);
        }

        public override void Add(IEnumerable<Online> entities, bool isSave = true)
        {
            base.Add(entities);
        }

        public override void Remove(int id, bool isSave = true)
        {
            base.Remove(id);
        }

        public override void Remove(Online entity, bool isSave = true)
        {
            base.Remove(entity);
        }

        public override void Remove(IEnumerable<Online> entities, bool isSave = true)
        {
            base.Remove(entities);
        }

        public override void Remove(Expression<Func<Online, bool>> predicate, bool isSave = true)
        {
            base.Remove(predicate);
        }

        public override void Update(Online entity, bool isSave = true)
        {
            base.Update(entity);
        }

        public override void Update(Expression<Func<Online, Boolean>> propertyExpression, Online entity, bool isSave = true)
        {
            base.Update(propertyExpression, entity);
        }
    }
}
