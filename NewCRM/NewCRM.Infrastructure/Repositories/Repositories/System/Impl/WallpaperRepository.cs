using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.System.Impl
{
    internal class WallpaperRepository : EfRepositoryBase<Wallpaper, Int32>
    {
        public override void Add(Wallpaper entity, bool isSave = true)
        {
            base.Add(entity, isSave);
        }

        public override void Remove(IEnumerable<Wallpaper> entities, bool isSave = true)
        {
            base.Remove(entities, isSave);
        }

        public override void Update(Wallpaper entity, bool isSave = true)
        {
            base.Update(entity, isSave);
        }

        public override IQueryable<Wallpaper> Entities { get; }
    }
}
