using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.Repositories.IRepository.Account;
using NewCRM.Repository.RepositoryProvide;

namespace NewCRM.Repository.RepositoryImpl.Account
{


    public class UserRepository : EfRepositoryBase<User>, IUserRepository
    {
        public override IQueryable<User> Entities
        {
            get { return base.Entities.Where(w => w.IsDisable == false && w.IsDeleted == false); }
        }
    }
}
