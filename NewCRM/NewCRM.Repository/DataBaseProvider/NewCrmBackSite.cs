using System.ComponentModel.Composition;
using System.Data.Entity;
using EntityFramework.DynamicFilters;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.Entitys.Account;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Domain.Entitys.System;

namespace NewCRM.Repository.DataBaseProvider
{
    [Export(typeof(DbContext))]
    public sealed class NewCrmBackSite : DbContext
    {
        public NewCrmBackSite()
            : base("Data Source=localhost;Initial Catalog=NewCrmBackSite;Persist Security Info=True;User ID=sa;Password=sasa;MultipleActiveResultSets=true;")
        {
        }
        public DbSet<Title> Titles { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Power> Powers { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<App> Apps { get; set; }

        public DbSet<AppType> AppTypes { get; set; }

        public DbSet<Desk> Desks { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<Online> Onlines { get; set; }

        public DbSet<Config> Configs { get; set; }

        public DbSet<Wallpaper> Wallpapers { get; set; }

        public DbSet<AccountRole> AccountRoles { get; set; }

        public DbSet<RolePower> RolePowers { get; set; }

        public DbSet<AppStar> AppStars { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Filter("IsDeleted", (DomainModelBase modelBase) => modelBase.IsDeleted, false);

            modelBuilder.Filter("IsDisable", (Account account) => account.IsDisable, false);


            base.OnModelCreating(modelBuilder);
        }
    }
}
