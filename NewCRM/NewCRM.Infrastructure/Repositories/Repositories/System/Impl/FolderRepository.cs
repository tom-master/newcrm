using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.System.Impl
{
    internal class FolderRepository : EfRepositoryBase<Folder, Int32>
    {
        public override void Add(Folder entity, bool isSave = true)
        {
            base.Add(entity, isSave);
        }

        public override void Remove(IEnumerable<Folder> entities, bool isSave = true)
        {
            base.Remove(entities, isSave);
        }

        public override void Update(Folder entity, bool isSave = true)
        {
            base.Update(entity, isSave);
        }

        public override IQueryable<Folder> Entities { get; }
    }
}
