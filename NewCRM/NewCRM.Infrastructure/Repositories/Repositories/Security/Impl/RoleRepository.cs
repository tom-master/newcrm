using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain.DomainModel.Security;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.Security.Impl
{
    internal class RoleRepository : EfRepositoryBase<Role, Int32>
    {
        public override void Add(Role entity, bool isSave = true)
        {
            base.Add(entity, isSave);
        }

        public override void Remove(IEnumerable<Role> entities, bool isSave = true)
        {
            base.Remove(entities, isSave);
        }

        public override void Update(Role entity, bool isSave = true)
        {
            base.Update(entity, isSave);
        }
        
    }
}
