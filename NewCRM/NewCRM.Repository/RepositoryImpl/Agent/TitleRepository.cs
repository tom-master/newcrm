using System.ComponentModel.Composition;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Repositories;
using NewCRM.Domain.Repositories.IRepository.Agent;
using NewCRM.Repository.DataBaseProvider;

namespace NewCRM.Repository.RepositoryImpl.Agent
{
    [Export(typeof(IRepository<>)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class TitleRepository : EntityFrameworkProvider<Title>, ITitleRepository
    {
    }
}
