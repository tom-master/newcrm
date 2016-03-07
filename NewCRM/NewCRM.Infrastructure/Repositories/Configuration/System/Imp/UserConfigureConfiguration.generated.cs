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
            HasRequired(a => a.Wallpaper).WithMany(a => a.UserConfigures);
            HasRequired(a => a.User).WithRequiredDependent(a => a.UserConfigure);
            HasMany(a => a.Desks).WithMany(a => a.UserConfigures).Map(a => a.ToTable("UserConfigureDesk").MapLeftKey("ConfigId").MapRightKey("DeskId").MapRightKey("IsDefault"));
        }



        public void RegistTo(ConfigurationRegistrar configurations) { configurations.Add(this); }
    }
}
