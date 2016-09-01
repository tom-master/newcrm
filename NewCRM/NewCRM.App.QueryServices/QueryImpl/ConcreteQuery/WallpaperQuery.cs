using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.Repositories;
using NewCRM.QueryServices.DomainSpecification;
using NewCRM.QueryServices.Query;

namespace NewCRM.QueryServices.QueryImpl.ConcreteQuery
{
    public sealed class WallpaperQuery : IQuery<Wallpaper>
    {
        [Import]
        private IRepository<Wallpaper> WallpaperRepository { get; set; }


        public IEnumerable<Wallpaper> Find(ISpecification<Wallpaper> specification)
        {
            return WallpaperRepository.Entities.Where(wallpaper => specification.IsSatisfiedBy(wallpaper));
        }

        public IEnumerable<Wallpaper> PageBy(ISpecification<Wallpaper> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            throw new NotImplementedException();
        }
    }
}
