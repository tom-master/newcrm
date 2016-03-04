using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure.Repositories.Configuration.Account.Imp
{
    internal partial class UserConfiguration : EntityTypeConfiguration<User>, IEntityMapper
    {
        public UserConfiguration()
        {
            HasKey(a => a.Id);

            HasMany(a => a.Titles).WithOptional(a => a.User);

            HasRequired(a => a.Department).WithMany(a => a.Users);

            HasRequired(a => a.UserConfigure).WithRequiredDependent(a => a.User);

            HasMany(a => a.Roles).
                WithMany(a => a.Users).
                Map(a => a.ToTable("UserRole").MapLeftKey("UserId").MapRightKey("RoleId"));

            HasMany(a => a.Logs).WithRequired(a => a.User);

        }
        public void RegistTo(ConfigurationRegistrar configurations) { configurations.Add(this); }
    }
}
