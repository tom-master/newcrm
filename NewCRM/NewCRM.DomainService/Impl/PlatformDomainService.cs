using System;
using System.Collections.Generic;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.Repositories;

namespace NewCRM.Domain.Services.Impl
{
    public sealed class PlatformDomainService : IPlatformDomainService
    {

        private readonly IRepository<User> _userRepository;

        private readonly IRepository<Wallpaper> _wallpapeRepository;

        public PlatformDomainService()
        {
            
        }


        public User VaildateUser(String userName, String passWord)
        {
            return null;
            //var result =
            //    _userRepository.Entities.FirstOrDefault(user => user.Name == userName);

            //if (result == null)
            //{
            //    return null;
            //}
            //var isEquality = PasswordUtil.ComparePasswords(result.Password, passWord);
            //return !isEquality ? null : result;
        }

        public dynamic UserApp(Int32 userId)
        {

            //var configureResult = _userRepository.Entities.Where(user => user.Id == userId).Select(config => new
            //{
            //    config.UserConfigure.Dock,
            //    config.UserConfigure.Desk1,
            //    config.UserConfigure.Desk2,
            //    config.UserConfigure.Desk3,
            //    config.UserConfigure.Desk4,
            //    config.UserConfigure.Desk5,
            //}).FirstOrDefault();
            //var desks =
            //    configureResult.GetType().GetProperties().Where(prop => prop.Name.Contains("Desk") && (prop.GetValue(configureResult) != null && (prop.GetValue(configureResult) + "").Length != 0));

            //var deskMembers = desks.Select(userDesk => new
            //{
            //    deskName = userDesk.Name,
            //    deskMember = _deskMembeRepository.Entities.ToList().Where(deskMember => userDesk.GetValue(configureResult).ToString().Split(new[]
            //                                            {
            //                                                ','
            //                                           }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).Contains(deskMember.Id)).Select(member => new
            //                                           {
            //                                               type = member.MemberType.ToString(),
            //                                               memberId = member.Id,
            //                                               appId = member.AppId,
            //                                               name = member.Name,
            //                                               icon = member.Icon
            //                                           }),
            //});


            //var folders =
            //    _deskMembeRepository.Entities.Where(
            //        deskMember => deskMember.UserId == userId && deskMember.MemberType == MemberType.Folder).
            //                         Select(member => new
            //                         {
            //                             id = member.Id,
            //                             apps = _deskMembeRepository.Entities.Where(
            //                                        deskMember => deskMember.FolderId == member.Id).Select(m => new
            //                                        {
            //                                            type = m.MemberType.ToString(),
            //                                            memberId = member.Id,
            //                                            appId = member.AppId,
            //                                            name = member.Name,
            //                                            icon = member.Icon
            //                                        })
            //                         });

            //dynamic d = null;
            //var dock =
            //    configureResult.GetType().GetProperties().FirstOrDefault(prop => prop.Name.Contains("Dock") && (prop.GetValue(configureResult) != null && (prop.GetValue(configureResult) + "").Length != 0));
            //if (dock != null)
            //{
            //    d = new
            //    {
            //        deskName = dock.Name,
            //        deskMember = _deskMembeRepository.Entities.ToList().Where(deskMember => dock.GetValue(configureResult).ToString().Split(new[]
            //                 {
            //                    ','
            //                }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).Contains(deskMember.Id)).Select(member => new
            //                {
            //                    type = member.MemberType.ToString(),
            //                    memberId = member.Id,
            //                    appId = member.AppId,
            //                    name = member.Name,
            //                    icon = member.Icon
            //                })
            //    };
            //}
            //return new { deskMembers, folders, docks = d };
            return null;
        }

        public Boolean SetDefaultDesk(Int32 userId, Int32 deskId)
        {
            return false;
            //var configResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId).UserConfigure;
            //configResult.DefaultDesk = deskId;
            //try
            //{
            //    _userConfigureRepository.Update(configResult);
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }

        public Boolean SetAppDirection(Int32 userId, String direction)
        {
            return false;
            //var configResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId).UserConfigure;
            //configResult.AppXy = direction;
            //try
            //{
            //    _userConfigureRepository.Update(configResult);
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }

        public Boolean SetAppSize(Int32 userId, Int32 appSize)
        {
            return false;
            //var configResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId).UserConfigure;
            //configResult.AppSize = appSize;
            //try
            //{
            //    _userConfigureRepository.Update(configResult);
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }

        public Boolean SetAppVertical(Int32 userId, Int32 appVertical)
        {
            return false;
            //var configResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId).UserConfigure;
            //configResult.AppVerticalSpacing = appVertical;
            //try
            //{
            //    _userConfigureRepository.Update(configResult);
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }

        public Boolean SetAppHorizontal(int userId, int appHorizontal)
        {
            return false;
            //var configResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId).UserConfigure;
            //configResult.AppHorizontalSpacing = appHorizontal;
            //try
            //{
            //    _userConfigureRepository.Update(configResult);
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }

        public Boolean SetDockPosition(Int32 userId, String pos, Int32 deskNum)
        {
            return false;
            //var configResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId).UserConfigure;
            //configResult.DockPosition = pos;
            //configResult.DefaultDesk = deskNum;
            //try
            //{
            //    _userConfigureRepository.Update(configResult);
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }

        public Boolean SetSkin(Int32 userId, String skin)
        {
            return false;
            //var configResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId).UserConfigure;
            //configResult.Skin = skin;
            //try
            //{
            //    _userConfigureRepository.Update(configResult);
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }

        public Boolean SetWebWallpaper(Int32 userId, dynamic imageValue)
        {
            return false;
            //var wallpaper = new Wallpaper
            //{
            //    Title = imageValue.imageTitle,
            //    Url = imageValue.webImgUrl,
            //    Width = imageValue.width,
            //    Heigth = imageValue.height,
            //    Source = WallpaperSource.System
            //};

            //_wallpapeRepository.Add(wallpaper);

            //var configResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId).UserConfigure;
            //configResult.Wallpaper = wallpaper;

            //try
            //{
            //    _userConfigureRepository.Update(configResult);
            //    return true;
            //}
            //catch
            //{

            //    return false;
            //}

        }

        public IDictionary<Int32, Tuple<String, String>> GetWallpapers()
        {
            return null;
            //IDictionary<Int32, Tuple<String, String>> wallpaperDictionary = new Dictionary<Int32, Tuple<String, String>>();
            //var wallpaperResult = _wallpapeRepository.Entities.Where(wallpaper => wallpaper.Source == WallpaperSource.System).ToList();
            //foreach (var wallpaper in wallpaperResult)
            //{
            //    wallpaperDictionary.Add(new KeyValuePair<Int32, Tuple<String, String>>(wallpaper.Id, new Tuple<String, String>(wallpaper.Url, wallpaper.Title)));
            //}
            //return wallpaperDictionary;
        }

        public Boolean SetWallpaper(Int32 userId, Int32 wallpaperId, String wallPaperShowType)
        {
            return false;
            //var configResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId).UserConfigure;

            //if (wallpaperId != 0)
            //{
            //    configResult.Wallpaper = _wallpapeRepository.Entities.FirstOrDefault(wallPaper => wallPaper.Id == wallpaperId);
            //}

            //configResult.WallpaperShowType = wallPaperShowType;

            //try
            //{
            //    _userConfigureRepository.Update(configResult);
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }

        public IList<Tuple<Int32, String>> GetUploadWallPaper(int userId)
        {
            return null;
            //IList<Tuple<Int32, String>> uploadWallpaperList = new List<Tuple<int, string>>();
            //var uploadWallpaperResult =
            //    _wallpapeRepository.Entities.Where(wallPaper => wallPaper.Source == WallpaperSource.Upload);
            //uploadWallpaperResult.ToList().ForEach(wallPaper =>
            //{
            //    uploadWallpaperList.Add(new Tuple<int, string>(wallPaper.Id, wallPaper.Url));
            //});
            //return uploadWallpaperList;
        }
    }
}