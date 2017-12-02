using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NewCRM.Domain.ValueObject;

namespace NewCRM.Domain.Entitys.System
{

    [Serializable, Description("应用")]
    public partial class App : DomainModelBase
    {
        #region public property

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
        /// 备注
        /// </summary>
        [MaxLength(50)]
        public String Remark { get; private set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public Int32 Width { get; private set; }

        /// <summary>
        /// 高度
        /// </summary>
        public Int32 Height { get; private set; }

        /// <summary>
        /// 使用数
        /// </summary>
        public Int32 UseCount { get; private set; }

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
        /// 是否为系统应用
        /// </summary>
        public Boolean IsSystem { get; private set; }

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
        /// 开发者(用户)Id
        /// </summary>
        public Int32 AccountId { get; private set; }

        /// <summary>
        /// App类型Id
        /// </summary>
        public Int32 AppTypeId { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        public Boolean IsRecommand { get; private set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public AppAuditState AppAuditState { get; private set; }

        /// <summary>
        /// 发布状态
        /// </summary>
        public AppReleaseState AppReleaseState { get; private set; }

        /// <summary>
        /// app样式
        /// </summary>
        public AppStyle AppStyle { get; private set; }

        public Boolean IsInstall { get; private set; }

        public Int32 StarCount { get; private set; }

        public String AccountName { get; set; }

        #endregion

        #region ctor

        /// <summary>
        /// 实例化一个app对象
        /// </summary>
        public App(String name,
            String iconUrl,
            String appUrl,
            Int32 width,
            Int32 height,
            Int32 appTypeId,
            AppAuditState appAuditState,
            AppReleaseState appReleaseState,
            AppStyle appStyle = AppStyle.App,
            Int32 accountId = default(Int32),
            String remark = default(String),
            Boolean isMax = default(Boolean),
            Boolean isFull = default(Boolean),
            Boolean isSetbar = default(Boolean),
            Boolean isOpenMax = default(Boolean),
            Boolean isFlash = default(Boolean),
            Boolean isDraw = default(Boolean),
            Boolean isResize = default(Boolean))
        {
            Name = name;
            IconUrl = iconUrl;
            AppUrl = appUrl;
            Width = width > 800 ? 800 : width;
            Height = height > 600 ? 600 : height;
            IsMax = isMax;
            IsFull = isFull;
            IsSetbar = isSetbar;
            IsOpenMax = isOpenMax;
            IsFlash = isFlash;
            IsDraw = isDraw;
            IsResize = isResize;
            AppTypeId = appTypeId;
            AppStyle = appStyle;
            if(accountId == 0)
            {
                IsSystem = true;
            }
            else
            {
                IsSystem = false;
                AccountId = accountId;
            }

            IsLock = false;
            Remark = remark;
            AppAuditState = appAuditState;
            AppReleaseState = appReleaseState;
            UseCount = 0;
            IsRecommand = false;
        }

        public App() { }
        #endregion
    }
}
