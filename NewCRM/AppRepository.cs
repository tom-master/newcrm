using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.System.Impl
{
    public class AppRepository : EfRepositoryBase<App, Int32>
    {
        public override void Add(App entity, bool isSave = true)
        {
            base.Add(entity, isSave);
        }

        public override void Remove(IEnumerable<App> entities, bool isSave = true)
        {
            base.Remove(entities, isSave);
        }

        public override void Update(App entity, bool isSave = true)
        {
            base.Update(entity, isSave);
        }
        
    }
}
