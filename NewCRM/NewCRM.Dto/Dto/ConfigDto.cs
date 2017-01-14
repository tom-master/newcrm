using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Dto.Dto
{
    public sealed class ConfigDto : BaseDto
    {
        /// <summary>
        /// 皮肤
        /// </summary>
        [Required, StringLength(10)]
        public String Skin { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        [Required]
        public String AccountFace { get; set; }

        /// <summary>
        /// app尺寸
        /// </summary>
        public Int32 AppSize { get; set; }

        /// <summary>
        /// app垂直间距
        /// </summary>
        public Int32 AppVerticalSpacing { get; set; }

        /// <summary>
        /// app水平间距
        /// </summary>
        public Int32 AppHorizontalSpacing { get; set; }

        /// <summary>
        /// 默认桌面
        /// </summary>
        public Int32 DefaultDeskNumber { get; set; }

        /// <summary>
        /// app排列方向
        /// </summary>
        [Required]
        public String AppXy { get; set; }

        /// <summary>
        /// 码头位置
        /// </summary>
        [Required]
        public String DockPosition { get; set; }

        /// <summary>
        /// 壁纸
        /// </summary>
        [Required]
        public String WallpaperUrl { get; set; }

        /// <summary>
        /// 壁纸宽
        /// </summary>

        public Int32 WallpaperWidth { get; set; }

        /// <summary>
        ///壁纸高
        /// </summary>
        public Int32 WallpaperHeigth { get; set; }

        /// <summary>
        /// 壁纸来源
        /// </summary>
        [Required, StringLength(10)]
        public String WallpaperSource { get; set; }

        /// <summary>
        /// 壁纸的展示模式
        /// </summary>
        [Required, StringLength(10)]
        public String WallpaperMode { get; set; }

        /// <summary>
        /// 默认桌面数量
        /// </summary>
        public Int32 DefaultDeskCount { get; private set; }

    }
}
