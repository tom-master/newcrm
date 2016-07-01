using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainModel.System;

namespace NewCRM.Domain.Test.TestRepository
{
    internal class AppRepository:BaseRepository<App>
    {

        public override void Add(App entity, bool isSave = true)
        {
            base.Add(entity);
        }

        public override void Add(IEnumerable<App> entities, bool isSave = true)
        {
            base.Add(entities);
        }

        public override void Remove(int id, bool isSave = true)
        {
            base.Remove(id);
        }

        public override void Remove(App entity, bool isSave = true)
        {
            base.Remove(entity);
        }

        public override void Remove(IEnumerable<App> entities, bool isSave = true)
        {
            base.Remove(entities);
        }

        public override void Remove(Expression<Func<App, bool>> predicate, bool isSave = true)
        {
            base.Remove(predicate);
        }

        public override void Update(App entity, bool isSave = true)
        {
            base.Update(entity);
        }

        public override void Update(Expression<Func<App, Boolean>> propertyExpression, App entity, bool isSave = true)
        {
            base.Update(propertyExpression, entity);
        }
    }
}
