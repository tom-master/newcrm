using System;
using NewCRM.Domain.Entities.ValueObject;

namespace NewCRM.Domain.Entities.DomainModel.System
{
    public partial class App
    {
        #region public method

        /// <summary>
        /// 更新用户使用数
        /// </summary>
        public App ModifyUserCount()
        {
            ++UserCount;
            return this;

        }

        /// <summary>
        /// 更新评价等级
        /// </summary>
        /// <param name="startCount"></param>
        public App ModifyStartCount(Int32 startCount)
        {
            if (startCount <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(startCount)}:不能为0");
            }
            StartCount += startCount;
            return this;
        }


        /// <summary>
        /// app审核通过
        /// </summary>
        public App Pass()
        {
            AppAuditState = AppAuditState.Pass;
            return this;
        }

        /// <summary>
        /// app审核未通过
        /// </summary>
        public App Deny()
        {
            AppAuditState = AppAuditState.Deny;
            return this;
        }

        /// <summary>
        /// 修改是否最大化
        /// </summary>
        /// <param name="isMax"></param>
        /// <returns></returns>
        public App ModifyIsMax(Boolean isMax)
        {
            IsMax = isMax;
            return this;
        }

        /// <summary>
        /// 修改打开后是否铺满全屏
        /// </summary>
        /// <param name="isFull"></param>
        /// <returns></returns>
        public App ModifyIsFull(Boolean isFull)
        {
            IsFull = isFull;
            return this;
        }

        /// <summary>
        /// 修改加载后是否显示工具条
        /// </summary>
        /// <param name="isSetbar"></param>
        /// <returns></returns>
        public App ModifyIsSetbar(Boolean isSetbar)
        {
            IsSetbar = isSetbar;
            return this;
        }

        /// <summary>
        /// 修改打开是否最大化
        /// </summary>
        /// <param name="isOpenMax"></param>
        /// <returns></returns>
        public App ModifyIsOpenMax(Boolean isOpenMax)
        {
            IsOpenMax = isOpenMax;
            return this;
        }

        /// <summary>
        /// 修改是否锁定
        /// </summary>
        /// <param name="isLock"></param>
        /// <returns></returns>
        public App ModifyIsLock(Boolean isLock)
        {
            IsLock = isLock;
            return this;
        }

        /// <summary>
        /// 修改是否为福莱希
        /// </summary>
        /// <param name="isFlash"></param>
        /// <returns></returns>
        public App ModifyIsFlash(Boolean isFlash)
        {
            IsFlash = isFlash;
            return this;
        }

        /// <summary>
        /// 修改是否可以拖动
        /// </summary>
        /// <param name="isDraw"></param>
        /// <returns></returns>
        public App ModifyIsDraw(Boolean isDraw)
        {
            IsDraw = isDraw;
            return this;
        }

        /// <summary>
        /// 修改是否可以拉伸
        /// </summary>
        /// <param name="isResize"></param>
        /// <returns></returns>
        public App ModifyIsResize(Boolean isResize)
        {
            IsResize = isResize;
            return this;
        }

        public void Remove()
        {
            IsDeleted = true;
        }

        #endregion
    }
}
