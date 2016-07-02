using System.Data.Entity;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainModel.Security;
using NewCRM.Domain.Entities.DomainModel.System;


namespace NewCRM.Repository
{
    public sealed class NewCrmBackSite : DbContext
    {
        public NewCrmBackSite()
            : base("Data Source=localhost;Initial Catalog=NewCrmBackSite;Persist Security Info=True;User ID=sa;Password=sasa;MultipleActiveResultSets=true;")
        {
        }
        public DbSet<Title> Titles { get; set; }

        public DbSet<User> Users { get; set; }

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

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<RolePower> RolePowers { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Desk>().HasMany(desk => desk.Members).WithRequired(member => member.Desk);


            //modelBuilder.Entity<Config>().HasMany(userConfig => userConfig.Desks);
            //modelBuilder.Entity<Config>().HasRequired(userConfig => userConfig.Wallpaper);



            //modelBuilder.Entity<User>().HasRequired(user => user.Title);
            //modelBuilder.Entity<User>().HasRequired(user => user.Config);

            //modelBuilder.Entity<App>().HasRequired(app => app.AppType);


            //modelBuilder.Entity<Role>()
            //    .HasMany(role => role.Powers)
            //    .WithMany(power => power.Roles)
            //    .Map(map => map.ToTable("RolePower").MapLeftKey("RoleId").MapRightKey("PowerId"));


            //modelBuilder.Entity<User>()
            //    .HasMany(user => user.Roles)
            //    .WithMany(role => role.Users)
            //    .Map(map => map.ToTable("UserRole").MapLeftKey("UserId").MapRightKey("RoleId"));



            base.OnModelCreating(modelBuilder);
        }
    }
}
