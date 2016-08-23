using System;
using System.Linq;
using NewCRM.Domain.Entities.ValueObject;

namespace NewCRM.Domain.Entities.DomainModel.System
{
    public partial class App
    {
        #region public method

        /// <summary>
        /// 增加用户使用数
        /// </summary>
        public App AddUserCount()
        {
            UserCount = UserCount + 1;
            return this;
        }

        /// <summary>
        /// 减小用户使用数
        /// </summary>
        /// <returns></returns>
        public App SubtractUserCount()
        {
            UserCount = UserCount - 1;
            return this;
        }

        /// <summary>
        /// 增加app的评价分数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startCount"></param>
        public App AddStar(Int32 userId, Int32 startCount)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(userId)}:不能为0");
            }

            if (startCount <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(startCount)}:不能为0");
            }

            if (AppStars.Any(appStar => appStar.UserId == userId ))
            {
                throw new ArgumentException($"您已对这个应用打过分");
            }
            var score = startCount * 1.0;
            AppStars.Add(new AppStar(userId, score));
            return this;
        }

        /// <summary>
        /// 减小app的评价分数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public App SubtractStar(Int32 userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(userId)}:不能为0");
            }

            var appStar = AppStars.FirstOrDefault(star => star.UserId == userId );

            appStar?.RemoveStar();

            return this;

        }

        /// <summary>
        /// 审核通过
        /// </summary>
        public App Pass()
        {
            AppAuditState = AppAuditState.Pass;
            return this;
        }

        /// <summary>
        /// 审核未通过
        /// </summary>
        public App Deny()
        {
            AppAuditState = AppAuditState.Deny;
            return this;
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <returns></returns>
        public App Release()
        {
            AppReleaseState = AppReleaseState.Release;
            return this;
        }

        /// <summary>
        /// 暂不提交审核
        /// </summary>
        /// <returns></returns>
        public App DontSentAudit()
        {
            AppAuditState = AppAuditState.UnAuditState;
            return this;
        }

        /// <summary>
        /// 提交审核
        /// </summary>
        /// <returns></returns>
        public App SentAudit()
        {
            AppAuditState = AppAuditState.Wait;
            return this;
        }

        /// <summary>
        /// 修改app的宽
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        public App ModifyWidth(Int32 width)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException($"app宽不能小于或等于0");
            }
            Width = width;
            return this;
        }

        /// <summary>
        /// 修改app的高
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        public App ModifyHeight(Int32 height)
        {
            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException($"app的高不能小于或等于0");
            }

            Height = height;

            return this;
        }

        /// <summary>
        /// 修改app的指向地址
        /// </summary>
        /// <param name="newUrl"></param>
        /// <returns></returns>
        public App ModifyUrl(String newUrl)
        {
            if ((newUrl + "").Length <= 0)
            {
                throw new ArgumentOutOfRangeException($"app的url：{newUrl}不能为空");
            }

            AppUrl = newUrl;
            return this;
        }

        /// <summary>
        /// 修改app的图标
        /// </summary>
        /// <param name="newIconUrl"></param>
        /// <returns></returns>
        public App ModifyIconUrl(String newIconUrl)
        {
            if ((newIconUrl + "").Length <= 0)
            {
                throw new ArgumentOutOfRangeException($"app的图标路径:{newIconUrl}不能为空");
            }

            IconUrl = newIconUrl;
            return this;

        }

        /// <summary>
        /// 修改app名称
        /// </summary>
        /// <param name="newName"></param>
        /// <returns></returns>
        public App ModifyName(String newName)
        {
            Name = newName;
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

        /// <summary>
        /// 修改app类型
        /// </summary>
        /// <param name="appTypeId"></param>
        /// <returns></returns>
        public App ModifyAppType(Int32 appTypeId)
        {
            if (appTypeId <= 0)
            {
                throw new ArgumentOutOfRangeException($"应用类型不能为空");
            }
            AppTypeId = appTypeId;
            return this;
        }

        /// <summary>
        /// 修改app介绍
        /// </summary>
        /// <param name="remake"></param>
        /// <returns></returns>
        public App ModifyAppRemake(String remake)
        {
            Remark = remake;
            return this;
        }

        /// <summary>
        /// 修改app样式
        /// </summary>
        /// <param name="appStyle"></param>
        /// <returns></returns>
        public App ModifyAppStyle(AppStyle appStyle)
        {
            AppStyle = appStyle;
            return this;
        }

        public void Remove()
        {
            IsDeleted = true;
        }

        #endregion
    }
}
