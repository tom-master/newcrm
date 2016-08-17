using System;
using System.Collections.Generic;

namespace NewCRM.Dto.Dto
{
    public sealed class UserDto : BaseDto
    {


        /// <summary>
        /// 用户配置Id
        /// </summary>
        public Int32 ConfigId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        public String Title { get; set; }


        /// <summary>
        /// 是否在线
        /// </summary>
        public Boolean IsOnline { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// 皮肤
        /// </summary>
        public String Skin { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public String UserFace { get; set; }

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
        public String AppXy { get; set; }

        /// <summary>
        /// 码头位置
        /// </summary>
        public String DockPosition { get; set; }

        /// <summary>
        /// 壁纸
        /// </summary>
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
        public String WallpaperSource { get; set; }
        /// <summary>
        /// 壁纸的展示模式
        /// </summary>
        public String WallpaperMode { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public String UserType { get; set; }

        /// <summary>
        /// 桌面
        /// </summary>
        public List<DeskDto> Desks { get; set; }

        /// <summary>
        /// 用户角色Id
        /// </summary>
        public IEnumerable<Int32> RoleIds { get; set; }
    }
}
