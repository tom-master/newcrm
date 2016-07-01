using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainModel.System;

namespace NewCRM.Domain.Test.TestRepository
{
    internal class AppTypeRepository : BaseRepository<AppType>
    {
        public override void Add(AppType entity, bool isSave = true)
        {
            base.Add(entity);
        }

        public override void Add(IEnumerable<AppType> entities, bool isSave = true)
        {
            base.Add(entities);
        }

        public override void Remove(int id, bool isSave = true)
        {
            base.Remove(id);
        }

        public override void Remove(AppType entity, bool isSave = true)
        {
            base.Remove(entity);
        }

        public override void Remove(IEnumerable<AppType> entities, bool isSave = true)
        {
            base.Remove(entities);
        }

        public override void Remove(Expression<Func<AppType, bool>> predicate, bool isSave = true)
        {
            base.Remove(predicate);
        }

        public override void Update(AppType entity, bool isSave = true)
        {
            base.Update(entity);
        }

        public override void Update(Expression<Func<AppType, Boolean>> propertyExpression, AppType entity, bool isSave = true)
        {
            base.Update(propertyExpression, entity);
        }
    }
}
