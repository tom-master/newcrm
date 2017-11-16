using System;
using System.Linq;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Domain.Entitys.System
{
    public partial class App
    {
        #region public method

        /// <summary>
        /// 增加使用数
        /// </summary>
        public App AddUseCount()
        {
            ++UseCount;
            return this;
        }

        /// <summary>
        /// 减小使用数
        /// </summary>
        
        public App SubtractUseCount()
        {
            --UseCount;
            return this;
        }

        /// <summary>
        /// 增加app的评价分数
        /// </summary> 
        public App AddStar(Int32 accountId, Int32 startCount)
        {
            if (accountId <= 0)
            {
                throw new BusinessException($"{nameof(accountId)}:不能为0");
            }

            if (startCount <= 0)
            {
                throw new BusinessException($"{nameof(startCount)}:不能为0");
            }

            if (AppStars.Any(appStar => appStar.AccountId == accountId))
            {
                throw new BusinessException("您已对这个应用打过分");
            }

            var score = startCount * 1.0;
            AppStars.Add(new AppStar(accountId, score));

            return this;
        }

        /// <summary>
        /// 减小app的评价分数
        /// </summary>
        public App SubtractStar(Int32 accountId)
        {
            if (accountId <= 0)
            {
                throw new BusinessException($"{nameof(accountId)}:不能为0");
            }

            var appStar = AppStars.FirstOrDefault(star => star.AccountId == accountId);
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
        public App Release()
        {
            AppReleaseState = AppReleaseState.Release;
            return this;
        }

        /// <summary>
        /// 暂不提交审核
        /// </summary>
        public App DontSentAudit()
        {
            AppAuditState = AppAuditState.Wait;
            return this;
        }

        /// <summary>
        /// 提交审核
        /// </summary>
        public App SentAudit()
        {
            AppAuditState = AppAuditState.UnAuditState;
            return this;
        }

        /// <summary>
        /// 修改app的宽
        /// </summary>
        public App ModifyWidth(Int32 width)
        {
            if (width <= 0)
            {
                throw new BusinessException($"app宽不能小于或等于0");
            }
            Width = width;
            return this;
        }

        /// <summary>
        /// 修改app的高
        /// </summary>
        public App ModifyHeight(Int32 height)
        {
            if (height <= 0)
            {
                throw new BusinessException($"app的高不能小于或等于0");
            }

            Height = height;

            return this;
        }

        /// <summary>
        /// 修改app的指向地址
        /// </summary>
        public App ModifyUrl(String newUrl)
        {
            if ((newUrl + "").Length <= 0)
            {
                throw new BusinessException($"app的url：{newUrl}不能为空");
            }

            AppUrl = newUrl;
            return this;
        }

        /// <summary>
        /// 修改app的图标
        /// </summary>
        public App ModifyIconUrl(String newIconUrl)
        {
            if ((newIconUrl + "").Length <= 0)
            {
                throw new BusinessException($"app的图标路径:{newIconUrl}不能为空");
            }

            IconUrl = newIconUrl;
            return this;

        }

        /// <summary>
        /// 修改app名称
        /// </summary>
        public App ModifyName(String newName)
        {
            Name = newName;
            return this;
        }

        /// <summary>
        /// 修改是否最大化
        /// </summary>
        public App ModifyIsMax(Boolean isMax)
        {
            IsMax = isMax;
            return this;
        }

        /// <summary>
        /// 修改打开后是否铺满全屏
        /// </summary>
        public App ModifyIsFull(Boolean isFull)
        {
            IsFull = isFull;
            return this;
        }

        /// <summary>
        /// 修改加载后是否显示工具条
        /// </summary>
        public App ModifyIsSetbar(Boolean isSetbar)
        {
            IsSetbar = isSetbar;
            return this;
        }

        /// <summary>
        /// 修改打开是否最大化
        /// </summary>
        public App ModifyIsOpenMax(Boolean isOpenMax)
        {
            IsOpenMax = isOpenMax;
            return this;
        }

        /// <summary>
        /// 修改是否锁定
        /// </summary>
        public App ModifyIsLock(Boolean isLock)
        {
            IsLock = isLock;
            return this;
        }

        /// <summary>
        /// 修改是否为福莱希
        /// </summary>
        public App ModifyIsFlash(Boolean isFlash)
        {
            IsFlash = isFlash;
            return this;
        }

        /// <summary>
        /// 修改是否可以拖动
        /// </summary>
        public App ModifyIsDraw(Boolean isDraw)
        {
            IsDraw = isDraw;
            return this;
        }

        /// <summary>
        /// 修改是否可以拉伸
        /// </summary>
        public App ModifyIsResize(Boolean isResize)
        {
            IsResize = isResize;
            return this;
        }

        /// <summary>
        /// 修改app类型
        /// </summary>
        public App ModifyAppType(Int32 appTypeId)
        {
            if (appTypeId <= 0)
            {
                throw new BusinessException($"应用类型不能为空");
            }
            AppTypeId = appTypeId;
            return this;
        }

        /// <summary>
        /// 修改app介绍
        /// </summary>
        public App ModifyAppRemake(String remake)
        {
            Remark = remake;
            return this;
        }

        /// <summary>
        /// 修改app样式
        /// </summary>
        public App ModifyAppStyle(AppStyle appStyle)
        {
            AppStyle = appStyle;
            return this;
        }

        /// <summary>
        /// 设置今日推荐
        /// </summary>
        public App SetTodayRecommandApp()
        {
            IsRecommand = true;
            return this;
        }

        /// <summary>
        /// 取消今日推荐
        /// </summary>
        public App CancelTodayRecommandApp()
        {
            IsRecommand = false;
            return this;
        }

        /// <summary>
        /// 移除App
        /// </summary>
        public void Remove()
        {
            IsDeleted = true;
        }

        public override String KeyGenerator()
        {
            return $"NewCRM:{nameof(App)}:Id:{Id}";
        }

        #endregion
    }
}
