using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Domain.DomainModel.Security;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Domain.Repositories;
using NewCRM.Domain.Test.TestRepository;
using NewCRM.Domain.ValueObject;
using NewCRM.Repository;

namespace NewCRM.Domain.Test
{
    [TestClass]
    public class UnitTestUser
    {
        [TestMethod]
        public void TestMethod()
        {
            #region AddTestData


            //IRepository<Title> titleRepository = new TitleRepository();

            //titleRepository.Add(new Title(nameof(Title)));

            //var titleResult = titleRepository.Entities.FirstOrDefault();


            //IRepository<App> appRepository = new AppRepository();

            //appRepository.Add(new App(nameof(App), "http://www.test.com/", "http://www.test2.com/"));

            //var appResult = appRepository.Entities.FirstOrDefault();



            //IRepository<AppType> appTypeRepository = new AppTypeRepository();

            //appTypeRepository.Add(new AppType(nameof(AppType)));

            //IRepository<Online> onlineRepository = new OnlineRepository();

            //IRepository<Power> powerRepository = new PowerRepository();

            //powerRepository.Add(new List<Power>
            //{
            //    new Power(nameof(Power)+1),
            //    new Power(nameof(Power)+2),
            //    new Power(nameof(Power)+3),
            //    new Power(nameof(Power)+4),
            //    new Power(nameof(Power)+5),
            //    new Power(nameof(Power)+6),
            //});

            //var powerResults = powerRepository.Entities.ToList();



            //IRepository<Role> roleRepository = new RoleRepository();

            //roleRepository.Add(new List<Role>
            //{
            //    new Role(nameof(Role)+1),
            //    new Role(nameof(Role)+2),
            //});

            //var roleResults = roleRepository.Entities.ToList();

            //var roleResult1 = roleResults[0];
            //roleResult1.AddPower(powerResults.Take(3).ToArray());

            //var roleResult2 = roleResults[1];
            //roleResult2.AddPower(powerResults.Skip(3).Take(3).ToArray());


            //roleRepository.Update(roleResult1);
            //roleRepository.Update(roleResult2);




            //IRepository<User> userRepository = new UserRepository();

            //userRepository.Add(new User(nameof(User), nameof(User) + 123));

            //var userResult = userRepository.Entities.FirstOrDefault();

            //userResult.AddTitle(titleResult);

            //userResult.AddRole(roleResults.ToArray());

            //userResult.Config.AddDesk();

            //userResult.Config.Desks.ToList()[0].AddDeskMember(new Member(appResult.Name, appResult.IconUrl, appResult.Id, appResult.Width, appResult.Height));




            //userRepository.Update(userResult);


            //IRepository<Wallpaper> wallpapeRepository = new WallpaperRepository();

            //wallpapeRepository.Add(new Wallpaper(nameof(Wallpaper), "http://www.wallpaper.com", "", 800, 600));

            //var wallPeperResult = wallpapeRepository.Entities.FirstOrDefault();

            //userResult.Config.AddWallpaper(wallPeperResult);

            //userRepository.Update(userResult);



            #endregion

            #region SelectTestData

            //IRepository<User> userRepository = new UserRepository();

            //var result = userRepository.Entities.ToList();


            //foreach (var user in result)
            //{
            //    foreach (var desk in user.Config.Desks)
            //    {
            //        foreach (var member in desk.Members)
            //        {
            //            var m = member;
            //        }
            //    }
            //}

            #endregion
        }
    }
}
