using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Repositories.Account.Impl
{
    public class UserRepository : EfRepositoryBase<User, Int32>, IUserRepository
    {
        public override void Add(User entity, bool isSave = true)
        {
            base.Add(entity, isSave);
        }

        public override void Remove(IEnumerable<User> entities, bool isSave = true)
        {
            base.Remove(entities, isSave);
        }

        public override void Update(User entity, bool isSave = true)
        {
            base.Update(entity, isSave);
        }

    }
}
