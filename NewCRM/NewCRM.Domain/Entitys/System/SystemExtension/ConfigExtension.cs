using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain.ValueObject;

namespace NewCRM.Domain.Entitys.System
{
    public partial class Config
    {


        #region public method
        /// <summary>
        /// 更新壁纸的显示模式
        /// </summary>
        /// <param name="wallpaperMode"></param>
        public Config ModifyDisplayMode(WallpaperMode wallpaperMode)
        {
            WallpaperMode = wallpaperMode;
            return this;
        }


        /// <summary>
        /// 修改应用的排列方向
        /// </summary>
        public Config ModifyAppDirectionToY()
        {
            AppXy = AppAlignMode.Y;
            return this;
        }

        public Config ModifyAppDirectionToX()
        {
            AppXy = AppAlignMode.X;
            return this;
        }

        /// <summary>
        /// 更新显示的图标大小
        /// </summary>
        public Config ModifyDisplayIconLength(Int32 newSize)
        {
            AppSize = newSize <= 50 ? newSize : 50;
            return this;
        }

        /// <summary>
        /// 更新应用的垂直间距
        /// </summary>
        public Config ModifyAppVerticalSpacingLength(Int32 newLength)
        {
            AppVerticalSpacing = newLength < 100 ? newLength : 100;
            return this;
        }

        /// <summary>
        /// 更新应用的水平间距
        /// </summary>

        public Config ModifyAppHorizontalSpacingLength(Int32 newLength)
        {
            AppHorizontalSpacing = newLength < 100 ? newLength : 100;

            return this;
        }

        /// <summary>
        /// 修改应用码头位置
        /// </summary>
        /// <param name="deckDockPostionEnum"></param>
        public Config ModifyDockPostion(DockPostion deckDockPostionEnum)
        {
            DockPosition = deckDockPostionEnum;
            return this;
        }

        /// <summary>
        /// 修改用户头像
        /// </summary>
        /// <param name="faceUrl"></param>
        public Config ModifyAccountFace(String faceUrl)
        {
            AccountFace = faceUrl;
            return this;
        }

        /// <summary>
        /// 修改皮肤
        /// </summary>
        /// <param name="skinName"></param>
        /// <returns></returns>
        public Config ModifySkin(String skinName)
        {
            Skin = skinName;
            return this;
        }

        /// <summary>
        /// 修改显示的壁纸
        /// </summary>
        /// <param name="newWallpaper"></param>
        /// <returns></returns>
        public Config ModifyWallpaper(Wallpaper newWallpaper)
        {
            if (newWallpaper == null)
            {
                throw new ArgumentNullException($"{nameof(newWallpaper)}不能为空");
            }
            Wallpaper = newWallpaper;
            return this;
        }

        /// <summary>
        /// 更新默认第几个桌面
        /// </summary>
        /// <param name="deskNumber"></param>
        public Config ModifyDefaultDesk(Int32 deskNumber)
        {
            if (deskNumber > _maxDeskNumber)
            {
                throw new ArgumentException("设置的默认显示桌面号不能超出当前所有可用的桌面总数");
            }

            DefaultDeskNumber = deskNumber;

            return this;
        }


        /// <summary>
        /// 更新默认桌面数
        /// </summary>
        /// <param name="deskNumber"></param>
        public Config ModifyDefaultDeskCount(Int32 deskNumber)
        {
            if ((DefaultDeskCount + deskNumber) > _maxDeskNumber)
            {
                throw new ArgumentException("设置的默认显示桌面号不能超出当前所有可用的桌面总数");
            }

            DefaultDeskCount += deskNumber;

            return this;
        }

        public void Remove()
        {
            IsDeleted = true;
        }

        /// <summary>
        /// 每个用户最多能有10个桌面
        /// </summary>
        private static readonly Int32 _maxDeskNumber = 10;



        #endregion
    }
}
