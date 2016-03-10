using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Configuration.System.Imp
{
    internal partial class UserConfigureConfiguration : EntityTypeConfiguration<UserConfigure>, IEntityMapper
    {
        public UserConfigureConfiguration()
        {
            HasKey(a => a.Id);
            HasRequired(a => a.Wallpaper).WithMany(a => a.UserConfigures).Map(a => a.MapKey("WallpaperId"));
        }



        public void RegistTo(ConfigurationRegistrar configurations) { configurations.Add(this); }
    }
}
