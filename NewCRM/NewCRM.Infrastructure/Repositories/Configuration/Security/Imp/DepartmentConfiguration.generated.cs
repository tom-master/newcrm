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
    internal partial class DepartmentConfiguration : EntityTypeConfiguration<Department>, IEntityMapper
    {
        public DepartmentConfiguration()
        {
            HasKey(a => a.Id);

            HasMany(a => a.Users).WithRequired(a => a.Department);
        }

        public void RegistTo(ConfigurationRegistrar configurations) { configurations.Add(this); }
    }
}
