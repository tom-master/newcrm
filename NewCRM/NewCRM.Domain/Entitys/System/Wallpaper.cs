using System;
using System.Collections.Generic;
using System.ComponentModel;
using NewCRM.Domain.ValueObject;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entitys.System
{
    [Serializable, Description("壁纸")]
    public partial class Wallpaper : DomainModelBase, IAggregationRoot
    {
        #region public property

        /// <summary>
        /// 标题
        /// </summary>
        [Required, MaxLength(6)]
        public String Title { get; private set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        [Required]
        public String Url { get; private set; }

        /// <summary>
        /// 短地址
        /// </summary> 
        public String ShortUrl { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public WallpaperSource Source { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(50)]
        public String Description { get; private set; }

        /// <summary>
        /// 图片的宽
        /// </summary>
        public Int32 Width { get; private set; }

        /// <summary>
        /// 图片的高
        /// </summary>
        public Int32 Height { get; private set; }

        /// <summary>
        /// 上传者（用户）
        /// </summary>
        public Int32 AccountId { get; private set; }

        /// <summary>
        /// md5
        /// </summary>
        public String Md5 { get; private set; }

        public virtual ICollection<Config> Configs { get; set; }

        #endregion

        #region ctor

        /// <summary>
        /// 实例化一个壁纸对象
        /// </summary>
        public Wallpaper(String title, String url, String description, Int32 width, Int32 height, String md5, Int32 accountId = default(Int32), WallpaperSource wallpaperSource = default(WallpaperSource))
        {
            Title = title;
            Url = url;
            Description = description;
            Width = width;
            Height = height;
            Source = wallpaperSource;
            AccountId = accountId;
            Md5 = md5;
        }

        public Wallpaper()
        {
            
        }

        #endregion
    }
}
