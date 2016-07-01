using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.DomainModel.System;

namespace NewCRM.Domain.Test.TestRepository
{
    internal class WallpaperRepository : BaseRepository<Wallpaper>
    {
        public override void Add(Wallpaper entity, bool isSave = true)
        {
            base.Add(entity);
        }

        public override void Add(IEnumerable<Wallpaper> entities, bool isSave = true)
        {
            base.Add(entities);
        }

        public override void Remove(int id, bool isSave = true)
        {
            base.Remove(id);
        }

        public override void Remove(Wallpaper entity, bool isSave = true)
        {
            base.Remove(entity);
        }

        public override void Remove(IEnumerable<Wallpaper> entities, bool isSave = true)
        {
            base.Remove(entities);
        }

        public override void Remove(Expression<Func<Wallpaper, bool>> predicate, bool isSave = true)
        {
            base.Remove(predicate);
        }

        public override void Update(Wallpaper entity, bool isSave = true)
        {
            base.Update(entity);
        }

        public override void Update(Expression<Func<Wallpaper, Boolean>> propertyExpression, Wallpaper entity, bool isSave = true)
        {
            base.Update(propertyExpression, entity);
        }
    }
}
