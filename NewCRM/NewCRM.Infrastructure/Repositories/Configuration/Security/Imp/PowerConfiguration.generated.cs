using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.DomainModel.Security;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Configuration.Security.Imp
{
    internal partial class PowerConfiguration : EntityTypeConfiguration<Power>, IEntityMapper
    {

        public PowerConfiguration()
        {
            HasKey(a => a.Id);

            HasMany(a => a.Roles).
                WithMany(a => a.Powers).
                Map(a => a.ToTable("PowerRole").MapLeftKey("PowerId").MapRightKey("RoleId"));
        }


        public void RegistTo(ConfigurationRegistrar configurations) { configurations.Add(this); }
    }
}
