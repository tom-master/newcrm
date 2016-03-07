using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Configuration.Account.Imp
{
    internal partial class TitleConfiguration : EntityTypeConfiguration<Title>, IEntityMapper
    {
        public TitleConfiguration()
        {
            HasKey(a => a.Id);

         
        }
        public void RegistTo(ConfigurationRegistrar configurations) { configurations.Add(this); }
    }
}
