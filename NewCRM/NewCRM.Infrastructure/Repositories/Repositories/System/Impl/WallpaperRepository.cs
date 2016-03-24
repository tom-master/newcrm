using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Domain.Repositories;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.System.Impl
{
    public class WallpaperRepository : IWallpaperRepository
    {
        private readonly IRepository<Wallpaper> _repository;

        public WallpaperRepository()
        {
            _repository = new EfRepositoryBase<Wallpaper>();

        }

        public IQueryable<Wallpaper> Entities=> _repository.Entities.Where(wallPaper => wallPaper.IsSystem && !wallPaper.IsDeleted);

        public void Add(Wallpaper entity, bool isSave = true) { _repository.Add(entity, isSave); }

        public void Add(IEnumerable<Wallpaper> entities, bool isSave = true) { _repository.Add(entities, isSave); }

        public void Remove(int id, bool isSave = true) { _repository.Remove(id, isSave); }

        public void Remove(Wallpaper entity, bool isSave = true) { _repository.Remove(entity, isSave); }

        public void Remove(IEnumerable<Wallpaper> entities, bool isSave = true) { _repository.Remove(entities, isSave); }

        public void Remove(Expression<Func<Wallpaper, bool>> predicate, bool isSave = true) { _repository.Remove(predicate, isSave); }

        public void Update(Wallpaper entity, bool isSave = true) { _repository.Update(entity, isSave); }

        public void Update(Expression<Func<Wallpaper, dynamic>> predicate, Wallpaper entity, bool isSave = true) { _repository.Update(predicate, entity, isSave); }
    }
}
