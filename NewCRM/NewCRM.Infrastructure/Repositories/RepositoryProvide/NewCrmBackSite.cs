using System.Data.Entity;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Domain.DomainModel.Security;
using NewCRM.Domain.DomainModel.System;
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

        public DbSet<UserConfigure> UserConfigure { get; set; }

        public DbSet<Department> Department { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<Role> Role { get; set; }

        public DbSet<Title> Title { get; set; }

        public DbSet<Online> Online { get; set; }

        public DbSet<Log> Log { get; set; }

        public DbSet<Power> Power { get; set; }

        public DbSet<App> App { get; set; }

        public DbSet<Wallpaper> Wallpaper { get; set; }

        public DbSet<AppType> AppType { get; set; }

        public DbSet<DeskMember> DeskMember { get; set; }


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
