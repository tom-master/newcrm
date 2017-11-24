using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NewCRM.Domain.ValueObject;

namespace NewCRM.Domain.Entitys.System
{
    /// <summary>
    /// 成员
    /// </summary>
    [Serializable, Description("成员")]
    public partial class Member : DomainModelBase
    {
        #region public property

        /// <summary>
        /// 应用Id
        /// </summary>
        public Int32 AppId { get; private set; }

        /// <summary>
        /// 成员的宽
        /// </summary>
        public Int32 Width { get; private set; }

        /// <summary>
        /// 成员的高
        /// </summary>
        public Int32 Height { get; private set; }

        /// <summary>
        /// 文件夹Id
        /// </summary>
        public Int32 FolderId { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required, MaxLength(6)]
        public String Name { get; private set; }

        /// <summary>
        /// 图标地址
        /// </summary>
        [Required]
        public String IconUrl { get; private set; }

        /// <summary>
        /// app地址
        /// </summary>
        public String AppUrl { get; private set; }

        /// <summary>
        /// 成员是否在应用码头上
        /// </summary>
        public Boolean IsOnDock { get; private set; }

        /// <summary>
        /// 是否能最大化
        /// </summary>
        public Boolean IsMax { get; private set; }

        /// <summary>
        /// 是否打开后铺满全屏
        /// </summary>
        public Boolean IsFull { get; private set; }

        /// <summary>
        /// 是否显示app底部的按钮
        /// </summary>
        public Boolean IsSetbar { get; private set; }

        /// <summary>
        /// 是否打开最大化
        /// </summary>
        public Boolean IsOpenMax { get; private set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public Boolean IsLock { get; private set; }

        /// <summary>
        /// 是否为福莱希
        /// </summary>
        public Boolean IsFlash { get; private set; }

        /// <summary>
        /// 是否可以拖动
        /// </summary>
        public Boolean IsDraw { get; private set; }

        /// <summary>
        /// 是否可以拉伸
        /// </summary>
        public Boolean IsResize { get; private set; }

        /// <summary>
        /// 成员类型
        /// </summary>
        public MemberType MemberType { get; private set; }

        /// <summary>
        /// 桌面索引
        /// </summary>
        public Int32 DeskIndex { get; set; }

        /// <summary>
        /// 账户Id
        /// </summary>
        public Int32 AccountId { get; set; }

        #endregion

        #region public ctor

        /// <summary>
        /// 实例化一个成员对象
        /// </summary>
        public Member(
            String name,
            String iconUrl,
            String appUrl,
            Int32 appId,
            Int32 width,
            Int32 height,
            Boolean isLock = default(Boolean),
            Boolean isMax = default(Boolean),
            Boolean isFull = default(Boolean),
            Boolean isSetbar = default(Boolean),
            Boolean isOpenMax = default(Boolean),
            Boolean isFlash = default(Boolean),
            Boolean isDraw = default(Boolean),
            Boolean isResize = default(Boolean))
        {
            AppId = appId;
            Width = width > 800 ? 800 : width;
            Height = height > 600 ? 600 : height;
            IsDraw = isDraw;
            IsOpenMax = isOpenMax;
            IsSetbar = isSetbar;
            IsMax = isMax;
            IsFull = isFull;
            IsFlash = isFlash;
            IsResize = isResize;
            IsLock = isLock;
            Name = name;
            IconUrl = iconUrl;
            AppUrl = appUrl;
            MemberType = appId == 0 ? MemberType.Folder : MemberType.App;
        }

        /// <summary>
        /// 实例化一个成员对象
        /// </summary>
        public Member(String name, String iconUrl, Int32 appId)
        {
            AppId = appId;
            Width = 800;
            Height = 600;
            IsDraw = false;
            IsOpenMax = false;
            Name = name;
            IconUrl = iconUrl;
            MemberType = appId == 0 ? MemberType.Folder : MemberType.App;
        }

        public Member()
        {
        }

        #endregion
    }
}
