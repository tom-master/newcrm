using System;
using NewCRM.Domain.Entities.ValueObject;

namespace NewCRM.Dto.Dto
{
    public sealed class WallpaperDto
    {
        public Int32 Id { get; set; }


        public Int32 UserId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public String Url { get; set; }

        public String ShortUrl { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public WallpaperSource Source { get; set; }

        /// <summary>
        /// 图片的宽
        /// </summary>
        public Int32 Width { get; set; }

        /// <summary>
        /// 图片的高
        /// </summary>
        public Int32 Heigth { get; set; }

    }
}
