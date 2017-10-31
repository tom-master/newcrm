using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Repository.DatabaseProvider.EF.Context;
using NewCRM.Repository.DataBaseProvider.EF;

namespace NewCRM.Repository.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<NewCrmContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;

        }

        protected override void Seed(NewCrmContext context)
        {
            //  This method will be called after migrating to the lates  t version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            if (Debugger.IsAttached == false)
            {
                Debugger.Launch();
            }
            try
            {
                var account = context.Accounts.Add(new Account("admin", PasswordUtil.CreateDbPassword("adminadmin"), AccountType.Admin));
                var wallpaper = context.Wallpapers.Add(new Wallpaper("wallpaper2.jpg", "/Scripts/HoorayUI/img/wallpaper/wallpaper2.jpg", "",
                    1920, 1080, null, 0, WallpaperSource.System));
                var configs = context.Configs.Add(new Config());
                var role = context.Roles.Add(new Role("Admin","Admin"));
                context.AccountRoles.Add(new AccountRole(account.Id, role.Id));
                var appType = context.AppTypes.Add(new AppType("系统"));
                var apps = context.Apps.AddRange(new List<App>
                {
                    new App("账户管理","/Scripts/HoorayUI/img/ui/system-gear.png","/AccountManager/Index",800,600,appType.Id,AppAuditState.Pass,AppReleaseState.Release),
                    new App("应用类型管理","/Scripts/HoorayUI/img/ui/system-gear.png","/AppTypes/Index",800,600,appType.Id,AppAuditState.Pass,AppReleaseState.Release),
                    new App("权限管理","/Scripts/HoorayUI/img/ui/system-gear.png","/security/Index",800,600,appType.Id,AppAuditState.Pass,AppReleaseState.Release),
                    new App("应用管理","/Scripts/HoorayUI/img/ui/system-gear.png","/AppManager/index",800,600,appType.Id,AppAuditState.Pass,AppReleaseState.Release),
                    new App("测试","/Scripts/HoorayUI/img/ui/system-gear.png","/test/index",800,600,appType.Id,AppAuditState.Pass,AppReleaseState.Release)
                });

                var rolePowers = new List<RolePower>();
                foreach (var app in apps)
                {
                    rolePowers.Add(new RolePower(role.Id, app.Id));
                }
                context.RolePowers.AddRange(rolePowers);

                context.Desks.AddRange(new List<Desk>
                {
                    new Desk(1,account.Id),
                    new Desk(2,account.Id),
                    new Desk(3,account.Id),
                    new Desk(4,account.Id),
                    new Desk(5,account.Id)
                });
                context.Members.AddRange(new List<Member>
                {
                    new Member("账户管理","/Scripts/HoorayUI/img/ui/system-gear.png","/AccountManager/Index",apps.FirstOrDefault(w => w.Name=="账户管理").Id,800,600),
                    new Member("应用类型管理","/Scripts/HoorayUI/img/ui/system-gear.png","/AppTypes/Index",apps.FirstOrDefault(w => w.Name=="应用类型管理").Id,800,600),
                    new Member("权限管理","/Scripts/HoorayUI/img/ui/system-gear.png","/security/Index",apps.FirstOrDefault(w => w.Name=="权限管理").Id,800,600),
                    new Member("应用管理","/Scripts/HoorayUI/img/ui/system-gear.png","/AppManager/index",apps.FirstOrDefault(w => w.Name=="应用管理").Id,800,600),
                    new Member("测试","/Scripts/HoorayUI/img/ui/system-gear.png","/test/index",apps.FirstOrDefault(w => w.Name=="测试").Id,800,600)
                });
                context.SaveChanges();
            }
            catch (Exception e)
            {
                if (e is DbEntityValidationException)
                {

                    if (File.Exists($@"{AppDomain.CurrentDomain.BaseDirectory}\aa.txt"))
                    {
                        File.Delete($@"{AppDomain.CurrentDomain.BaseDirectory}\aa.txt");
                    }

                    var ee = e as DbEntityValidationException;
                    var builder = new StringBuilder();
                    foreach (var dbEntityValidationResult in ee.EntityValidationErrors)
                    {
                        foreach (var validationError in dbEntityValidationResult.ValidationErrors)
                        {
                            var m = $@"{validationError.PropertyName}|||||||{validationError.ErrorMessage}{Environment.NewLine}";

                            builder.Append(m);
                           
                        }
                    }

                    using (var fileStream = new FileStream($@"{AppDomain.CurrentDomain.BaseDirectory}\aa.txt", FileMode.Create))
                    {
                        var bs = new byte[builder.ToString().Length];
                        bs = Encoding.UTF8.GetBytes(builder.ToString());
                        fileStream.Write(bs, 0, bs.Length);
                    }
                }
            }
        }
    }
}
