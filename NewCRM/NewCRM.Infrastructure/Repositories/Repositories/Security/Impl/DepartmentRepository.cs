using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain.DomainModel.Security;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.Security.Impl
{
    internal class DepartmentRepository : EfRepositoryBase<Department, Int32>
    {
        public override void Add(Department entity, bool isSave = true)
        {
            base.Add(entity, isSave);
        }

        public override void Remove(IEnumerable<Department> entities, bool isSave = true)
        {
            base.Remove(entities, isSave);
        }

        public override void Update(Department entity, bool isSave = true)
        {
            base.Update(entity, isSave);
        }
        
    }
}
