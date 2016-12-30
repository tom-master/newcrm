using System;
using System.Collections.Generic;
using System.ComponentModel;
using NewCRM.Domain.ValueObject;

namespace NewCRM.Domain.Entitys.System
{

    [Serializable, Description("应用")]
    public partial class App : DomainModelBase, IAggregationRoot
    {
        #region public property

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// 图标地址
        /// </summary>
        public String IconUrl { get; private set; }

        /// <summary>
        /// app地址
        /// </summary>
        public String AppUrl { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
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
        /// app类型Id
        /// </summary>
        public Int32 AppTypeId { get; private set; }


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

        public virtual ICollection<AppStar> AppStars { get; private set; }

       // public virtual ICollection<AppRole> AppRoles { get; private set; }

        /// <summary>
        /// app类型
        /// </summary>
        public virtual AppType AppType { get; private set; }



        #endregion

        #region ctor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">app名称</param>
        /// <param name="iconUrl">app图标路径</param>
        /// <param name="appUrl">app地址</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="appTypeId"></param>
        /// <param name="appAuditState"></param>
        /// <param name="appStyle"></param>
        /// <param name="accountId"></param>
        /// <param name="remark">备注</param>
        /// <param name="isMax">是否最大化</param>
        /// <param name="isFull">是否全屏</param>
        /// <param name="isSetbar">是否显示app底部的按钮</param>
        /// <param name="isOpenMax">是否打开默认最大化</param>
        /// <param name="isFlash">是否为flash</param>
        /// <param name="isDraw">是否可以任意修改app窗体的大小</param>
        /// <param name="isResize"></param>
        public App(String name,
            String iconUrl,
            String appUrl,
            Int32 width,
            Int32 height,
            Int32 appTypeId,
            AppAuditState appAuditState,
            AppStyle appStyle = AppStyle.App,
            Int32 accountId = default(Int32),
            String remark = default(String),
            Boolean isMax = default(Boolean),
            Boolean isFull = default(Boolean),
            Boolean isSetbar = default(Boolean),
            Boolean isOpenMax = default(Boolean),
            Boolean isFlash = default(Boolean),
            Boolean isDraw = default(Boolean),
            Boolean isResize = default(Boolean)):this()
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
            if (accountId == 0)
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
            AppReleaseState = AppReleaseState.UnRelease;
            UseCount = 0;
            AppStars = new List<AppStar>();
           // AppRoles = new List<AppRole>();
            IsRecommand = false;
        }

        public App()
        {
            AddTime = DateTime.Now;
        }
        #endregion
    }
}
