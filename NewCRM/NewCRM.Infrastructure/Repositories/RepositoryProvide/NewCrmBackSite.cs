using System.Data.Entity;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Domain.DomainModel.Security;
using NewCRM.Domain.DomainModel.System;

namespace NewCRM.Infrastructure.Repositories.RepositoryProvide
{
    public sealed class NewCrmBackSite : DbContext
    {
        private DbSet<UserConfigure> _configs;
        private DbSet<Department> _depts;
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

        public DbSet<UserConfigure> Configs
        {
            get { return _configs; }
            set { _configs = value; }
        }

        public DbSet<Department> Depts
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
            //用户配置<---->用户
            modelBuilder.Entity<UserConfigure>().
                         HasRequired(a => a.User).
                         WithMany().
                         HasForeignKey(a => a.User);

            modelBuilder.Entity<User>().
                        HasRequired(a => a.UserConfigure).
                        WithMany().
                        HasForeignKey(a => a.UserConfigure);

            //用户配置----->壁纸
            modelBuilder.Entity<UserConfigure>().HasRequired(a => a.Wallpaper).WithMany().HasForeignKey(a => a.Wallpaper);


            //用户---->部门
            modelBuilder.Entity<User>().
                         HasRequired(a => a.Department).
                         WithMany().
                         HasForeignKey(a => a.Department);
            //用户---->头衔
            modelBuilder.Entity<User>().HasRequired(a => a.Title).WithMany().HasForeignKey(a => a.Title);

            //用户桌面
            modelBuilder.Entity<User>().HasMany(b => b.Desks).WithMany(c => c.Users).Map(m =>
            {
                m.MapLeftKey("UserId");
                m.MapRightKey("DeskId");
                m.ToTable("UserDesks");
            });

            //文件夹----->桌面
            modelBuilder.Entity<Folder>().
                       HasRequired(a => a.Desk).
                       WithMany().
                       HasForeignKey(a => a.Desk);



            //App---->桌面
            modelBuilder.Entity<App>().
                      HasRequired(a => a.Desk).
                      WithMany().
                      HasForeignKey(a => a.Desk);





            //App---->文件夹
            modelBuilder.Entity<App>().
                     HasRequired(a => a.Folder).
                     WithMany().
                     HasForeignKey(a => a.Folder);


            //App类型--->App
            modelBuilder.Entity<AppType>().HasMany(a => a.Apps).WithRequired(a => a.AppType);


            //日志---->用户
            modelBuilder.Entity<Log>().
                        HasRequired(a => a.User).
                        WithMany().
                        HasForeignKey(a => a.User);




            //用户角色
            modelBuilder.Entity<User>().HasMany(b => b.Roles).WithMany(c => c.Users).Map(m =>
            {
                m.MapLeftKey("UserId");
                m.MapRightKey("RoleId");
                m.ToTable("UserRole");
            });

            modelBuilder.Entity<Role>().HasMany(b => b.Powers).WithMany(c => c.Roles).Map(m =>
            {
                m.MapLeftKey("RoleId");
                m.MapRightKey("PowerId");
                m.ToTable("RolePower");
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
