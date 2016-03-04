using System.Data.Entity;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Domain.DomainModel.Security;
using NewCRM.Domain.DomainModel.System;

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

        public DbSet<Folder> Folders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
