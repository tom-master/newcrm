using System;
using System.ComponentModel;
using NewCRM.Domain.ValueObject;

namespace NewCRM.Domain.DomainModel.System
{
    [Serializable,Description("壁纸")]
    public class Wallpaper : DomainModelBase, IAggregationRoot
    {
        #region public property

        /// <summary>
        /// 标题
        /// </summary>
        public String Title { get; private set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public String Url { get; private set; }

        /// <summary>
        /// 来源
        /// </summary>
        public WallpaperSource Source { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        public String Description { get; private set; }

        /// <summary>
        /// 图片的宽
        /// </summary>
        public Int32 Width { get; private set; }

        /// <summary>
        /// 图片的高
        /// </summary>
        public Int32 Heigth { get; private set; }

        #endregion

        #region ctor
        /// <summary>
        /// 实例化一个壁纸对象
        /// </summary>
        /// <param name="title"></param>
        /// <param name="url"></param>
        /// <param name="description"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="wallpaperSource"></param>
        public Wallpaper(String title, String url, String description, Int32 width, Int32 height, WallpaperSource wallpaperSource = default(WallpaperSource))
        {
            Title = title;
            Url = url;
            Description = description;
            Width = width;
            Heigth = height;
            Source = wallpaperSource;
        }

        public Wallpaper() { }




        #endregion

    }
}
