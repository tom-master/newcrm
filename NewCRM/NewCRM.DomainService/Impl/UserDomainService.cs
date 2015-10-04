using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustemException;
using NewCRM.Infrastructure.Repositories.Repositories.Account;
using NewCRM.Infrastructure.Repositories.Repositories.System;
using NewCRM.Infrastructure.Repositories.Repositories.System.Impl;

namespace NewCRM.DomainService.Impl
{
    /// <summary>
    /// 领域服务层
    /// </summary>
    public class UserDomainService : BaseService, IUserDomainService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOnlineRepository _onlineRepository;
        private readonly IAppRepository _appRepository;
        private readonly IFolderRepository _folderRepository;
        private readonly IWallpaperRepository _wallpaperRepository;
        public UserDomainService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _onlineRepository = new OnlineRepository();
            _appRepository = new AppRepository();
            _folderRepository = new FolderRepository();
            _wallpaperRepository = new WallPaperRepository();

        }

        /// <summary>
        /// 验证当前准备登录的用户是否合法
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        public User ValidateUser(String userName, String userPassword)
        {
            var userData = _userRepository.Entities.FirstOrDefault(user => user.Name == userName);
            if (userData == null)
            {
                throw new RepositoryException("用户不存在");
            }
            if (userData.IsDeleted)
            {
                throw new RepositoryException("该用户已被禁用");
            }
            //检查用户的密码是否正确
            if (!PasswordUtil.ComparePasswords(userData.Password, userPassword))
            {
                throw new RepositoryException("用户名或密码错误");
            }
            //插入一条在线的信息
            _onlineRepository.Add(new Online
            {
                IpAdddress = "127.0.0.1",
                UserId = userData.Id
            });
            return userData;
        }

        /// <summary>
        /// 根据用户唯一标识得到壁纸
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User Wallpaper(Int32 userId)
        {
            var firstOrDefault = _userRepository.Entities.FirstOrDefault(user => user.Id == userId && user.IsDeleted == false);
            return firstOrDefault;
        }
        /// <summary>
        /// 根据用户实体得到app
        /// </summary>
        /// <param name="entityUser"></param>
        /// <returns></returns>
        public String App(User entityUser)
        {
            var user = entityUser;
            if (user == null)
            {
                return "";
            }
            StringBuilder builder = new StringBuilder("{");
            String currentDesk = String.Empty;
            IList<String> docks = new List<string> { "dock" }, deskApps;
            for (int i = 0; i < user.Desks.Count; i++)
            {
                docks.Add("desk" + (i + 1));
            }
            for (Int32 i = 0; i < (user.Desks.Count + 1); i++)
            {
                builder.AppendFormat("\"{0}\":[", docks[i]);
                //switch (i)
                //{
                //    case 0: currentDesk = user.Dock; break;
                //    case 1: currentDesk = user.Desk1; break;
                //    case 2: currentDesk = user.Desk2; break;
                //    case 3: currentDesk = user.Desk3; break;
                //    case 4: currentDesk = user.Desk4; break;
                //    case 5: currentDesk = user.Desk5; break;
                //}

                currentDesk = i == 0 ? user.Dock : (user.Desks.ToList()[i - 1]).App;


                if ((currentDesk + "").Length > 0)
                {
                    deskApps = currentDesk.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var deskApp in deskApps.Where(deskApp => deskApp.Length != 0))
                    {
                        builder.Append("{");
                        if (deskApp.Split('_')[1] == "folder")
                        {
                            Int32 folderId = Convert.ToInt32(deskApp.Split('_')[0]);
                            var folderEntity = user.Folders.FirstOrDefault(f => f.Id == folderId && f.IsDeleted == false);
                            if (folderEntity == null)
                            {
                                throw new ArgumentException("folderEntity is null");
                            }
                            builder.AppendFormat("\"id\":\"{0}\",\"name\":\"{1}\",\"icon\":\"{2}\",\"type\":\"{3}\"",
                                folderEntity.Id, folderEntity.Name, folderEntity.Icon, "folder");
                        }
                        else
                        {
                            Int32 appId = Convert.ToInt32(deskApp.Split('_')[0]);
                            var appEntity = user.Apps.FirstOrDefault(a => a.Id == appId && a.IsDeleted == false);
                            if (appEntity == null)
                            {
                                throw new ArgumentException("appEntity is null");
                            }
                            builder.AppendFormat("\"id\":\"{0}\",\"name\":\"{1}\",\"icon\":\"{2}\",\"type\":\"{3}\"",
                               appEntity.Id, appEntity.Name, appEntity.ImageUrl, "app");
                        }
                        builder.Append("},");
                    }
                    builder.Remove(builder.Length - 1, 1);
                }
                builder.AppendFormat("],");
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Append("}");
            return builder.ToString();
        }

        /// <summary>
        /// 根据传入的应用id和应用类型来创建窗体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public String BuildWindow(Int32 id, String type)
        {
            var result = "{";
            if (type.Equals("app"))
            {
                result = BuilderAppWindow(id, result);
            }
            else if (type.Equals("folder"))
            {
                result = BuilderFolderWindow(id, result);
            }
            result += "}";
            return result;
        }
        /// <summary>
        /// 构建应用窗体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private String BuilderFolderWindow(Int32 id, String result)
        {
            var folderEntity = _folderRepository.Entities.FirstOrDefault(folder => folder.Id == id && folder.IsDeleted == false);
            if (folderEntity == null)
            {
                throw new BusinessException(String.Format("未能找到编号为：{0}的文件夹，请重试", id));
            }
            result += string.Format(
                "\"type\":\"{0}\",\"id\":{1},\"name\":\"{2}\",\"icon\":\"{3}\",\"width\":650,\"height\":400",
                "folder", folderEntity.Id, folderEntity.Name, folderEntity.Icon);
            return result;
        }
        /// <summary>
        /// 构建文件夹窗体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private String BuilderAppWindow(Int32 id, String result)
        {
            var appEntity = _appRepository.Entities.FirstOrDefault(app => app.Id == id && app.IsDeleted == false);
            if (appEntity == null)
            {
                throw new BusinessException(String.Format("未能找到编号为：{0}的应用，请重试", id));
            }
            result +=
                string.Format(
                    "\"type\":\"{0}\",\"id\":{1},\"name\":\"{2}\",\"icon\":\"{3}\",\"url\":\"{4}\",\"width\":{5},\"height\":{6},\"isresize\":\"{7}\",\"issetbar\":\"{8}\",\"ismin\":\"{9}\",\"isfull\":\"{10}\",\"isopenmax\":\"{11}\",\"islock\":\"{12}\"",
                    appEntity.AppInternalClass, appEntity.Id, appEntity.Name, appEntity.ImageUrl, appEntity.NavigateUrl,
                    appEntity.Width, appEntity.Height,
                    appEntity.IsMax, appEntity.IsSetbar, appEntity.IsMin, appEntity.IsFull, appEntity.IsOpenMax,
                    appEntity.IsLock);
            return result;
        }

        /// <summary>
        /// 删除用户上传的壁纸
        /// </summary>
        /// <param name="wallPaperId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Boolean DeleteWallPaper(Int32 wallPaperId, Int32 userId)
        {
            var result = _userRepository.Entities.Any(user => user.Wallpaper.Id == wallPaperId && user.Id == userId);
            if (result)
            {
                //当前用户正在使用要删除的壁纸
                return false;
            }
            var wallPaperEntity = _wallpaperRepository.Entities.FirstOrDefault(wallPaper => wallPaper.Id == wallPaperId);
            if (wallPaperEntity == null)
            {
                throw new RepositoryException("当前要删除的壁纸可能已被删除，请刷新后重试！");
            }
            wallPaperEntity.IsDeleted = true;
            _wallpaperRepository.Update(wallPaperEntity);

            File.Delete(wallPaperEntity.WallpaperWebUrl);
            return true;
        }

        /// <summary>
        /// 上传壁纸
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="wallpaper">壁纸Id</param>
        /// <returns>dynamic</returns>
        public Wallpaper UploadWallPaper(Int32 userId, Wallpaper wallpaper)
        {
            var wallPaperCount = _wallpaperRepository.Entities.Where(wallPaper => wallPaper.IsDeleted == false && wallPaper.UploaderId == userId);

            if (wallPaperCount.Count() == 6)
            {
                throw new BusinessException("最多只能上传6张壁纸");
            }
            _wallpaperRepository.Add(wallpaper);
            return wallpaper;
        }

        /// <summary>
        /// 设置网络图片作为壁纸
        /// </summary>
        /// <param name="wallpaper"></param>
        public Int32 SetWebWallPaper(Wallpaper wallpaper)
        {
            _wallpaperRepository.Add(wallpaper);
            return wallpaper.Id;
        }

        /// <summary>
        /// 获取登录的用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>User</returns>
        public User GetUser(int userId)
        {
            var userResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);
            if (userResult == null)
            {
                throw new RepositoryException("获取当前登录用户失败");
            }
            return userResult;
        }
    }
}
