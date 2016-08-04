using System;
using NewCRM.Domain.Entities.ValueObject;

namespace NewCRM.Dto.Dto
{
    public sealed class AppDto : BaseDto
    {
        #region public property

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 图标地址
        /// </summary>
        public String IconUrl { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }

        /// <summary>
        /// 使用人数
        /// </summary>
        public Int32 UserCount { get; set; }

        /// <summary>
        /// 评价星级
        /// </summary>
        public Double StartCount { get; set; }

        /// <summary>
        /// 是否安装
        /// </summary>
        public Boolean IsInstall { get; set; }

        /// <summary>
        /// 开发者（用户）Id
        /// </summary>
        public Int32 UserId { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public AppAuditState AppAuditState { get; set; }

        ///// <summary>
        ///// 发布状态
        ///// </summary>
        public AppReleaseState AppReleaseState { get; set; }

        /// <summary>
        /// app类型Id
        /// </summary>
        public Int32 AppTypeId { get; set; }

        /// <summary>
        /// app样式
        /// </summary>
        public AppStyle AppStyle { get; set; }

        /// <summary>
        /// app分类
        /// </summary>
        public String AppType { get; set; }

        public String AddTime { get; set; }

        #endregion
    }
}
