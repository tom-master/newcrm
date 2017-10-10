using System;
using System.ComponentModel;
using NewCRM.Domain.ValueObject;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entitys.System
{
    [Description("用户配置"), Serializable]
    public partial class Config : DomainModelBase
    {
        #region public property

        /// <summary>
        /// 皮肤
        /// </summary>
        [Required(), MaxLength(6)]
        public String Skin { get; private set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        [Required()]
        public String AccountFace { get; private set; }

        /// <summary>
        /// app尺寸
        /// </summary>
        public Int32 AppSize { get; private set; }

        /// <summary>
        /// app垂直间距
        /// </summary>
        public Int32 AppVerticalSpacing { get; private set; }

        /// <summary>
        /// app水平间距
        /// </summary>
        public Int32 AppHorizontalSpacing { get; private set; }

        /// <summary>
        /// 默认桌面编号
        /// </summary>
        public Int32 DefaultDeskNumber { get; private set; }

        /// <summary>
        /// 默认桌面数量
        /// </summary>
        public Int32 DefaultDeskCount { get; private set; }

        /// <summary>
        /// 壁纸的展示模式
        /// </summary>
        public WallpaperMode WallpaperMode { get; private set; }

        /// <summary>
        /// app排列方向
        /// </summary>
        public AppAlignMode AppXy { get; private set; }

        /// <summary>
        /// 码头位置
        /// </summary>
        public DockPostion DockPosition { get; private set; }

        /// <summary>
        /// 壁纸
        /// </summary>
        public virtual Wallpaper Wallpaper { get; private set; }
 
        #endregion

        #region ctor

        public Config()
        {
            AppXy = AppAlignMode.X;
            DockPosition = DockPostion.Top;
            Skin = "default";
            WallpaperMode = WallpaperMode.Fill;
            AccountFace = "";
            AppSize = 48;
            AppVerticalSpacing = 50;
            AppHorizontalSpacing = 50;
            DefaultDeskNumber = 1;
            DefaultDeskCount = 5;
        }

        #endregion
    }
}
