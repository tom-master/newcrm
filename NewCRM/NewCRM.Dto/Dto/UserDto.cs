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
        public String Desk1 { get; set; }
        public String Desk2 { get; set; }
        public String Desk3 { get; set; }
        public String Desk4 { get; set; }
        public String Desk5 { get; set; }
        public String AppSize { get; set; }
        public String WallpaperShowType { get; set; }
        public String UserFace { get; set; }
        public Boolean Enabled { get; set; }
        public DateTime LastLoginTime { get; set; }
        public Int32 DefaultDesk { get; set; }

        public Configs Config { get; set; }
        public Title Title { get; set; }
        public Depts Dept { get; set; }
        public Wallpaper Wallpaper { get; set; }
        public ICollection<App> Apps { get; set; }
        public ICollection<Folder> Folders { get; set; }
        public ICollection<Role> Roles { get; set; }
    }
}
