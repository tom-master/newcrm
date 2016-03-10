using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain.DomainModel.Security;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.Security.Impl
{
    internal class PowerRepository : EfRepositoryBase<Power, Int32>
    {
        public override void Add(Power entity, bool isSave = true)
        {
            base.Add(entity, isSave);
        }

        public override void Remove(IEnumerable<Power> entities, bool isSave = true)
        {
            base.Remove(entities, isSave);
        }

        public override void Update(Power entity, bool isSave = true)
        {
            base.Update(entity, isSave);
        }
        
    }
}
