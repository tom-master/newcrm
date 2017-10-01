using System.ComponentModel.Composition;
using System.Data.Entity;
using EntityFramework.DynamicFilters;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Domain.Entitys.System;

namespace NewCRM.Repository.DataBaseProvider.EF
{
    public sealed class NewCrmBackSite : DbContext
    {
        public NewCrmBackSite() : base("name=NewCrm")
        {
            Configuration.AutoDetectChangesEnabled = true;
        }
        public DbSet<Title> Titles { get; set; }

        public DbSet<Account> Accounts { get; set; }
        
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

        //public DbSet<AppRole> AppRoles { get; set; }

        public DbSet<AppStar> AppStars { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Filter("IsDeleted", (DomainModelBase modelBase) => modelBase.IsDeleted, false);

            modelBuilder.Filter("IsDisable", (Account account) => account.IsDisable, false);

            //Account
            modelBuilder.Entity<Account>()
                .HasRequired(a => a.Title)
                .WithMany(t => t.Accounts)
                .Map(m => m.MapKey("TitleId"));

            modelBuilder.Entity<Account>()
                .HasRequired(a => a.Config)
                .WithMany()
                .Map(m => m.MapKey("ConfigId"));

            //modelBuilder.Entity<Account>()
            //    .HasMany(a => a.AccountRoles)
            //    .WithMany(r => r.Accounts)
            //    .Map(m => m.MapLeftKey("AccountId").MapRightKey("RoleId").ToTable("AccountRole"));


            //Config
            modelBuilder.Entity<Config>()
                .HasRequired(c => c.Wallpaper)
                .WithMany(w => w.Configs).Map(m => m.MapKey("WallpaperId"));

            ////Role
            //modelBuilder.Entity<Role>()
            //    .HasMany(r => r.Powers)
            //    .WithMany(p => p.Roles)
            //    .Map(m => m.MapLeftKey("RoleId").MapRightKey("PowerId").ToTable("RolePower"));

            //App
            modelBuilder.Entity<App>()
                .HasMany(a => a.AppStars)
                .WithRequired(a => a.App).Map(m => m.MapKey("AppId"));


            base.OnModelCreating(modelBuilder);
        }
    }
}
