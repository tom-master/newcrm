using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Domain.Repositories.IRepository.Account;
using NewCRM.Repository.RepositoryProvide;

namespace NewCRM.Repository.RepositoryImpl.Account
{
    [Export(typeof(ITitleRepository))]
    public class TitleRepository : EfRepositoryBase<Title>, ITitleRepository
    {
    }
}
