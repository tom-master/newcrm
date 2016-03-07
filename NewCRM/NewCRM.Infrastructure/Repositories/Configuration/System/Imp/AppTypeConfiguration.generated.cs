using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Configuration.System.Imp
{
    internal partial class AppTypeConfiguration : EntityTypeConfiguration<AppType>, IEntityMapper
    {
        public AppTypeConfiguration()
        {
            HasKey(a => a.Id);

        }


        public void RegistTo(ConfigurationRegistrar configurations) { configurations.Add(this); }
    }
}
