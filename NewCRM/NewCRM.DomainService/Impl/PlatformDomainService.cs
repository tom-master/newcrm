using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Infrastructure.CommonTools.CustemException;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.Infrastructure.Repositories.Repositories.Account;
using NewCRM.Infrastructure.Repositories.Repositories.Account.Impl;
using NewCRM.Infrastructure.Repositories.Repositories.System;

namespace NewCRM.DomainService.Impl
{
    /// <summary>
    /// 平台服务
    /// </summary>
    public class PlatformDomainService : BaseService, IPlatformDomainService
    {
        private readonly IWallpaperRepository _wallpaperRepository;
        private readonly IUserRepository _userRepository;

        public PlatformDomainService(IWallpaperRepository wallpaperRepository)
        {
            _wallpaperRepository = wallpaperRepository;
            _userRepository = new UserRepository();
        }
        /// <summary>
        /// 获取所有的壁纸
        /// </summary>
        /// <returns> ICollection<Wallpaper/></returns>
        public ICollection<Wallpaper> GetWallpapers()
        {
            return _wallpaperRepository.Entities.Where(wallPaper => wallPaper.IsSystem == true).ToList();
        }

        /// <summary>
        /// 设置壁纸
        /// </summary>
        /// <param name="wallpaperId">壁纸Id</param>
        /// <param name="wallPaperShowType">壁纸的显示方式</param>
        /// <param name="userId">用户Id</param>
        public void SetWallPaper(Int32 wallpaperId, String wallPaperShowType, Int32 userId)
        {
            var userEntity = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);
            if (userEntity == null)
            {
                throw new RepositoryException(String.Format("更换壁纸失败，请重试"));
            }
            if (wallpaperId != 0)
            {
                userEntity.Wallpaper = _wallpaperRepository.Entities.FirstOrDefault(wallPaper => wallPaper.Id == wallpaperId && wallPaper.IsDeleted == false);
            }
            userEntity.WallpaperShowType = wallPaperShowType;
            _userRepository.Update(userEntity);
        }

        /// <summary>
        /// 获取用户上传的壁纸
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>ICollection<WallpaperDto/></returns>
        public ICollection<Wallpaper> GetUserUploadWallPaper(Int32 userId)
        {
            return _wallpaperRepository.Entities.Where(wallPaper => wallPaper.UploaderId == userId && wallPaper.IsDeleted == false).ToList();
        }

        /// <summary>
        /// 获取所有的皮肤
        /// </summary>
        /// <param name="skinFullPath">皮肤路径</param>
        /// <returns>IDictionary<String, dynamic/> </returns>
        public IDictionary<String, dynamic> GetAllSkin(String skinFullPath)
        {
            IDictionary<String, dynamic> dataDictionary = new Dictionary<String, dynamic>();
            Directory.GetFiles(skinFullPath, "*.css").ToList().ForEach(path =>
            {
                //获取皮肤名称
                var fileName = Get(path, x => x.LastIndexOf(@"\", StringComparison.OrdinalIgnoreCase) + 1).Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0];
                dataDictionary.Add(fileName, new
                {
                    //皮肤路径
                    skinPath = path.Substring(path.LastIndexOf("script", StringComparison.OrdinalIgnoreCase) - 1).Replace(@"\", "/"),
                    //皮肤的预览图
                    imgPath = GetImage(fileName, skinFullPath),
                });
            });
            return dataDictionary;
        }
        private String GetImage(String fileName, String fullPath)
        {
            var dic =
                Directory.GetFiles(fullPath, "preview.png",
                    SearchOption.AllDirectories).ToList();
            foreach (var dicItem in from dicItem in dic let regex = new Regex(fileName) where regex.IsMatch(dicItem) select dicItem)
            {
                return dicItem.Substring(dicItem.LastIndexOf("script", StringComparison.OrdinalIgnoreCase) - 1).Replace(@"\", "/");
            }
            return "";
        }

        private String Get(String path, Func<String, Int32> filterFunc)
        {
            return path.Substring(filterFunc(path));
        }

        /// <summary>
        /// 修改平台的皮肤
        /// </summary>
        /// <param name="skinName">皮肤名称</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public Boolean UpdateSkin(String skinName, Int32 userId)
        {
            var userEntity = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);
            if (userEntity == null)
            {
                throw new RepositoryException(String.Format("未能查询到Id为：{0}的用户，请重试", userId));
            }
            userEntity.Skin = skinName;
            _userRepository.Update(userEntity);
            return true;
        }

        /// <summary>
        /// 更新默认显示的桌面
        /// </summary>
        /// <param name="deskNum">默认桌面编号</param>
        /// <param name="userId">用户Id</param>
        /// <returns>Boolean</returns>
        public Boolean UpdateDefaultDesk(Int32 deskNum, Int32 userId)
        {
            var userEntity = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);
            if (userEntity == null)
            {
                throw new RepositoryException("更换默认桌面失败");
            }
            userEntity.DefaultDesk = deskNum;
            _userRepository.Update(userEntity);
            return true;
        }

        /// <summary>
        /// 更新应用的排列方向
        /// </summary>
        /// <param name="direction">排列方向</param>
        /// <param name="userId">用户Id</param>
        /// <returns>Boolean</returns>
        public Boolean UpdateAppXy(String direction, Int32 userId)
        {
            var userEntity = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);
            if (userEntity == null)
            {
                throw new RepositoryException("更换应用排列方向失败");
            }
            userEntity.AppXy = direction;
            _userRepository.Update(userEntity);
            return true;
        }

        /// <summary>
        /// 更新应用大小
        /// </summary>
        /// <param name="appSize">应用大小</param>
        /// <param name="userId">用户Id</param>
        /// <returns>Boolean</returns>
        public Boolean UpdateAppSize(Int32 appSize, Int32 userId)
        {
            var userEntity = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);
            if (userEntity == null)
            {
                throw new RepositoryException("更换应用大小失败");
            }
            userEntity.AppSize = appSize;
            _userRepository.Update(userEntity);
            return true;
        }

        /// <summary>
        /// 更新应用码头的位置
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="deskNum"></param>
        /// <param name="userId"></param>
        /// <returns>Boolean</returns>
        public Boolean UpdateDockPosition(String pos, Int32 deskNum, Int32 userId)
        {
            var userEntity = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);
            if (userEntity == null)
            {
                throw new RepositoryException("更换应用码头位置失败");
            }
            userEntity.DockPosition = pos;
            userEntity.DefaultDesk = deskNum;
            _userRepository.Update(userEntity);
            return true;
        }

        /// <summary>
        /// 更新应用图标的垂直间距
        /// </summary>
        /// <param name="appVertical">垂直艰巨</param>
        /// <param name="userId">用户Id</param>
        /// <returns>Boolean</returns>
        public Boolean UpdateAppVertical(Int32 appVertical, Int32 userId)
        {
            var userEntity = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);
            if (userEntity == null)
            {
                throw new RepositoryException("更换应用图标的垂直间距失败");
            }
            userEntity.AppVerticalSpacing = appVertical;
            _userRepository.Update(userEntity);
            return true;
        }

        /// <summary>
        /// 更新应用图标的水平间距
        /// </summary>
        /// <param name="appHorizontal">水平间距</param>
        /// <param name="userId">用户Id</param>
        /// <returns>Boolean</returns>
        public Boolean UpdateAppHorizontal(Int32 appHorizontal, Int32 userId)
        {
            var userEntity = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);
            if (userEntity == null)
            {
                throw new RepositoryException("更换应用图标的水平间距失败");
            }
            userEntity.AppHorizontalSpacing = appHorizontal;
            _userRepository.Update(userEntity);
            return true;
        }
        /// <summary>
        /// 获取当前用户下的桌面中的app
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="desk">桌面</param>
        /// <returns></returns>
        public List<String> GetAppsInDeskByUserId(Int32 userId, String desk)
        {
            var internalAppStr = new List<String>();
            //var userEntity = _userRepository.Entities.Select(s =>
            //{
            //    if (desk == "Desk1")
            //    {
            //        internalAppStr = s.Desk1.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();
            //    }
            //});

            return internalAppStr;
        }
    }
}