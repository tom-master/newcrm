using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.System.Impl
{
    internal class AppTypeRepository : EfRepositoryBase<AppType, Int32>
    {
        public override void Add(AppType entity, bool isSave = true)
        {
            base.Add(entity, isSave);
        }

        public override void Remove(IEnumerable<AppType> entities, bool isSave = true)
        {
            base.Remove(entities, isSave);
        }

        public override void Update(AppType entity, bool isSave = true)
        {
            base.Update(entity, isSave);
        }
        
    }
}
