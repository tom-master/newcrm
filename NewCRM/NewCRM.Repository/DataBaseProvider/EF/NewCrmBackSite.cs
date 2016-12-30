using System.ComponentModel.Composition;
using System.Data.Entity;
using EntityFramework.DynamicFilters;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Domain.Entitys.System;

namespace NewCRM.Repository.DataBaseProvider.EF
{
    [Export(typeof(DbContext))]
    public sealed class NewCrmBackSite : DbContext
    {
        public NewCrmBackSite() : base("name=NewCrm")
        {
            Configuration.AutoDetectChangesEnabled = true;
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
                .HasForeignKey(a => a.TitleId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasRequired(a => a.Config)
                .WithMany()
                .HasForeignKey(a => a.ConfigId);



            //modelBuilder.Entity<Account>()
            //    .HasMany(a => a.AccountRoles)
            //    .WithMany(r => r.Accounts)
            //    .Map(m => m.MapLeftKey("AccountId").MapRightKey("RoleId").ToTable("AccountRole"));


            //Config
            modelBuilder.Entity<Config>()
                .HasRequired(c => c.Wallpaper)
                .WithMany(w => w.Configs).HasForeignKey(c => c.WallpaperId);

            ////Role
            //modelBuilder.Entity<Role>()
            //    .HasMany(r => r.Powers)
            //    .WithMany(p => p.Roles)
            //    .Map(m => m.MapLeftKey("RoleId").MapRightKey("PowerId").ToTable("RolePower"));

            //App
            modelBuilder.Entity<App>()
                .HasMany(a => a.AppStars)
                .WithRequired(a => a.App)
                .HasForeignKey(a => a.AppId);

            modelBuilder.Entity<App>()
                .HasRequired(a => a.AppType)
                .WithMany(a => a.Apps)
                .HasForeignKey(a => a.AppTypeId);

            //Desk
            modelBuilder.Entity<Desk>()
                .HasMany(d => d.Members)
                .WithRequired(m => m.Desk).HasForeignKey(m => m.DeskId);


            base.OnModelCreating(modelBuilder);
        }
    }
}
