using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.Account.Impl
{
    internal class TitleRepository : EfRepositoryBase<Title, Int32>
    {
        public override void Add(Title entity, bool isSave = true)
        {
            base.Add(entity, isSave);
        }

        public override void Remove(IEnumerable<Title> entities, bool isSave = true)
        {
            base.Remove(entities, isSave);
        }

        public override void Update(Title entity, bool isSave = true)
        {
            base.Update(entity, isSave);
        }
        
    }
}
