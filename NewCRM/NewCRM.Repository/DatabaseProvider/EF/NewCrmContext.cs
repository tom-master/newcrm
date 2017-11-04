using System.Data.Entity;
using EntityFramework.DynamicFilters;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Domain.Entitys.System;

namespace NewCRM.Repository.DatabaseProvider.EF.Context
{
	public sealed class NewCrmContext : DbContext
	{
		public NewCrmContext() : base("name=NewCrm")
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

            //Config
            modelBuilder.Entity<Config>()
				.HasRequired(c => c.Wallpaper)
				.WithMany(w => w.Configs).Map(m => m.MapKey("WallpaperId"));

			//App
			modelBuilder.Entity<App>()
				.HasMany(a => a.AppStars)
				.WithRequired(a => a.App).Map(m => m.MapKey("AppId"));


			base.OnModelCreating(modelBuilder);
		}
	}
}
