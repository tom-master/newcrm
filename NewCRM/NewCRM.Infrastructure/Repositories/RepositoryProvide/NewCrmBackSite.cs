using System.Data.Entity;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Domain.DomainModel.Security;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Infrastructure.Repositories.Configurations;
using AppConfiguration = NewCRM.Infrastructure.Repositories.Configuration.System.Imp.AppConfiguration;
using AppTypeConfiguration = NewCRM.Infrastructure.Repositories.Configuration.System.Imp.AppTypeConfiguration;
using DepartmentConfiguration = NewCRM.Infrastructure.Repositories.Configuration.Security.Imp.DepartmentConfiguration;
using PowerConfiguration = NewCRM.Infrastructure.Repositories.Configuration.Security.Imp.PowerConfiguration;
using RoleConfiguration = NewCRM.Infrastructure.Repositories.Configuration.Security.Imp.RoleConfiguration;
using TitleConfiguration = NewCRM.Infrastructure.Repositories.Configuration.Account.Imp.TitleConfiguration;
using UserConfiguration = NewCRM.Infrastructure.Repositories.Configuration.Account.Imp.UserConfiguration;
using UserConfigureConfiguration = NewCRM.Infrastructure.Repositories.Configuration.System.Imp.UserConfigureConfiguration;

namespace NewCRM.Infrastructure.Repositories.RepositoryProvide
{
    public sealed class NewCrmBackSite : DbContext
    {
        public NewCrmBackSite()
            : base("Data Source=localhost;Initial Catalog=NewCrmBackSite;Persist Security Info=True;User ID=sa;Password=sasa;MultipleActiveResultSets=true;")
        {

        }

        public DbSet<UserConfigure> UserConfigures { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Title> Titles { get; set; }

        public DbSet<Online> Onlines { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<Power> Powers { get; set; }

        public DbSet<App> Apps { get; set; }

        public DbSet<Wallpaper> Wallpapers { get; set; }

        public DbSet<AppType> AppTypes { get; set; }

        public DbSet<DeskMember> DeskMembers { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new TitleConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new DepartmentConfiguration());
            modelBuilder.Configurations.Add(new PowerConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new AppConfiguration());
            modelBuilder.Configurations.Add(new AppTypeConfiguration());
            modelBuilder.Configurations.Add(new UserConfigureConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
