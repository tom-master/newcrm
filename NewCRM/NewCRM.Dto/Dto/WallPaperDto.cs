using System;

namespace NewCRM.Dto.Dto
{
    public sealed class WallpaperDto
    {
        public Int32 Id { get; set; }
        public String Title { get; set; }
        public String Url { get; set; }
        public String SmallUrl { get; set; }
        public String WallpaperType { get; set; }
        public String WallpaperWebUrl { get; set; }
        public String Description { get; set; }
        public Int32 Width { get; set; }
        public Int32 Heigth { get; set; }
        public Boolean IsSystem { get; set; }
        public Int32 UploaderId { get; set; }
        public String WallpaperShowType { get; set; }
    }
}
