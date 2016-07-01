using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.DomainModel.Account;

namespace NewCRM.Domain.Test.TestRepository
{
    internal class TitleRepository : BaseRepository<Title>
    {
        public override void Add(Title entity, bool isSave = true)
        {
            base.Add(entity);
        }

        public override void Add(IEnumerable<Title> entities, bool isSave = true)
        {
            base.Add(entities);
        }

        public override void Remove(int id, bool isSave = true)
        {
            base.Remove(id);
        }

        public override void Remove(Title entity, bool isSave = true)
        {
            base.Remove(entity);
        }

        public override void Remove(IEnumerable<Title> entities, bool isSave = true)
        {
            base.Remove(entities);
        }

        public override void Remove(Expression<Func<Title, bool>> predicate, bool isSave = true)
        {
            base.Remove(predicate);
        }

        public override void Update(Title entity, bool isSave = true)
        {
            base.Update(entity);
        }

        public override void Update(Expression<Func<Title, Boolean>> propertyExpression, Title entity, bool isSave = true)
        {
            base.Update(propertyExpression, entity);
        }
    }
}
