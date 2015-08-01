using System.Data.Entity;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Domain.DomainModel.Security;
using NewCRM.Domain.DomainModel.System;

namespace NewCRM.Infrastructure.Repositories.RepositoryProvide
{
    public sealed class NewCrmBackSite : DbContext
    {
        private DbSet<Configs> _configs;
        private DbSet<Depts> _depts;
        private DbSet<User> _users;
        private DbSet<Role> _roles;
        private DbSet<Title> _titles;
        private DbSet<Online> _onlines;
        private DbSet<Log> _logs;
        private DbSet<Power> _powers;
        private DbSet<App> _apps;
        private DbSet<Wallpaper> _wallpaper;
        private DbSet<AppType> _appClass;
        private DbSet<Folder> _folder;

        public NewCrmBackSite()
            : base("Data Source=localhost;Initial Catalog=NewCrmBackSite;Persist Security Info=True;User ID=sa;Password=sasa;MultipleActiveResultSets=true;")
        {

        }

        public DbSet<Configs> Configs
        {
            get { return _configs; }
            set { _configs = value; }
        }

        public DbSet<Depts> Depts
        {
            get { return _depts; }
            set { _depts = value; }
        }

        public DbSet<User> Users
        {
            get { return _users; }
            set { _users = value; }
        }

        public DbSet<Role> Roles
        {
            get { return _roles; }
            set { _roles = value; }
        }

        public DbSet<Title> Titles
        {
            get { return _titles; }
            set { _titles = value; }
        }

        public DbSet<Online> Onlines
        {
            get { return _onlines; }
            set { _onlines = value; }
        }

        public DbSet<Log> Logs
        {
            get { return _logs; }
            set { _logs = value; }
        }

        public DbSet<Power> Powers
        {
            get { return _powers; }
            set { _powers = value; }
        }

        public DbSet<App> Apps
        {
            get { return _apps; }
            set { _apps = value; }
        }

        public DbSet<Wallpaper> Wallpaper
        {
            get { return _wallpaper; }
            set { _wallpaper = value; }
        }

        public DbSet<AppType> AppClass
        {
            get { return _appClass; }
            set { _appClass = value; }
        }

        public DbSet<Folder> Folder
        {
            get { return _folder; }
            set { _folder = value; }
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasRequired(u => u.Config).WithOptional(c => c.User).Map(m => m.MapKey("ConfigId"));

            modelBuilder.Entity<Title>().HasMany(t => t.Users).WithOptional(t => t.Title).Map(m => m.MapKey("TitleId"));

            modelBuilder.Entity<User>()
                .HasOptional(u => u.Dept)
                .WithMany(d => d.Users)
                .Map(d => d.MapKey("DeptId"));

            modelBuilder.Entity<User>()
                .HasOptional(u => u.Wallpaper)
                .WithMany(w => w.Users)
                .Map(w => w.MapKey("WallpaperId"));

            modelBuilder.Entity<User>().HasMany(u => u.Apps).WithMany(a => a.Users).Map(s =>
            {
                s.ToTable("UserApp");
                s.MapLeftKey("UserId");
                s.MapRightKey("AppId");
            });

            modelBuilder.Entity<User>()
                .HasMany(u => u.Folders).WithMany(f => f.Users).Map(m =>
                {
                    m.MapLeftKey("UserId");
                    m.MapRightKey("FolderId");
                    m.ToTable("UserFolders");
                });


            modelBuilder.Entity<User>().HasMany(u => u.Roles).WithMany(r => r.Users).Map(m =>
            {
                m.ToTable("UserRole");
                m.MapLeftKey("UserId");
                m.MapRightKey("RoleId");
            });



            modelBuilder.Entity<Folder>().HasMany(f => f.Apps).WithMany(a => a.Folder).Map(m=>m.ToTable("FolderApps").MapLeftKey("FolderId").MapRightKey("AppId"));


            modelBuilder.Entity<AppType>().HasMany(a => a.Apps).WithOptional(a => a.AppType).Map(m => m.MapKey("AppTypeId"));


            modelBuilder.Entity<Power>().HasMany(p => p.Roles).WithMany(r => r.Powers).Map(m =>
            {
                m.ToTable("RolePowers").MapLeftKey("RoleId").MapRightKey("PowerId");
            });


            modelBuilder.Entity<Folder>()
                .HasMany(f => f.Powers).WithMany().Map(m =>
                {
                    m.ToTable("FolderPowers").MapLeftKey("FolderId").MapRightKey("PowerId");
                });



            modelBuilder.Entity<App>()
                .HasMany(a => a.Powers).WithMany().Map(m =>
                {
                    m.ToTable("AppPowers").MapLeftKey("AppId").MapRightKey("PowerId");
                });

            modelBuilder.Entity<Power>()
                .HasOptional(p => p.PowerType)
                .WithMany(pt => pt.Powers)
                .Map(m => m.MapKey("PowerTypeId"));
        }
    }
}
