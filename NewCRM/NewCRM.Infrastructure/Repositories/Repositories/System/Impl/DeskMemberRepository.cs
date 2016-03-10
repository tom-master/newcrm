using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.System.Impl
{
    internal class DeskMemberRepository : EfRepositoryBase<DeskMember, Int32>
    {
        public override void Add(DeskMember entity, bool isSave = true)
        {
            base.Add(entity, isSave);
        }

        public override void Remove(IEnumerable<DeskMember> entities, bool isSave = true)
        {
            base.Remove(entities, isSave);
        }

        public override void Update(DeskMember entity, bool isSave = true)
        {
            base.Update(entity, isSave);
        }
        
    }
}
