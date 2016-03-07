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
    internal partial class WallpaperConfiguration : EntityTypeConfiguration<Wallpaper>, IEntityMapper
    {
        public WallpaperConfiguration()
        {
            HasKey(a => a.Id);
            HasMany(a => a.UserConfigures).WithRequired(a => a.Wallpaper);
        }

        public void RegistTo(ConfigurationRegistrar configurations) { configurations.Add(this); }
    }
}
