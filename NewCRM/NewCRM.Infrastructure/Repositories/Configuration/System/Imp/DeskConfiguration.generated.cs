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
    internal partial class DeskConfiguration : EntityTypeConfiguration<Desk>, IEntityMapper
    {

        public DeskConfiguration()
        {
            HasKey(a => a.Id);



            HasMany(a => a.Folders).WithMany(a => a.Desks).Map(a => a.ToTable("DeskFolder").MapLeftKey("DeskId").MapRightKey("FolderId"));

        }

        public void RegistTo(ConfigurationRegistrar configurations)
        {
            configurations.Add(this);
        }
    }
}
