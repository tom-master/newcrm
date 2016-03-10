using System;
using System.Collections.Generic;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Domain.DomainModel.Security;
using NewCRM.Domain.DomainModel.System;

namespace NewCRM.Dto.Dto
{
    public sealed class UserDto
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Password { get; set; }
        public String AppXy { get; set; }
        public String DockPosition { get; set; }
        public String Skin { get; set; }
        public String Dock { get; set; }
        public Int32 AppSize { get; set; }
        public Int32 AppVerticalSpacing
        { get; set; }

        public Int32 AppHorizontalSpacing
        { get; set; }

        public String WallpaperShowType { get; set; }
        public String UserFace { get; set; }
        public Boolean Enabled { get; set; }
        public DateTime LastLoginTime { get; set; }
        public Int32 DefaultDesk { get; set; }

        /*public UserConfig Config { get; set; }*/
        public Title Title { get; set; }
      /*  public Depts Dept { get; set; }*/
        public Wallpaper Wallpaper { get; set; }
        public ICollection<App> Apps { get; set; }

        public ICollection<Role> Roles { get; set; }

     
    }
}
