using System;

namespace NewCRM.Domain.Entitys.System
{
    public partial class Member
    {
        #region public method
        /// <summary>
        /// 修改成员的宽
        /// </summary>
        /// <param name="width"></param>
        public Member ModifyWidth(Int32 width)
        {
            Width = width >= 800 ? 800 : width;

            return this;
        }

        /// <summary>
        /// 修改成员的高
        /// </summary>
        /// <param name="height"></param>
        public Member ModifyHeight(Int32 height)
        {
            Height = height >= 600 ? 600 : height;
            return this;
        }

        /// <summary>
        /// 修改成员图标
        /// </summary>
        /// <param name="iconUrl"></param>
        public Member ModifyIcon(String iconUrl)
        {
            IconUrl = iconUrl;
            return this;
        }

        /// <summary>
        /// 修改成员名称
        /// </summary>
        /// <param name="newMemberName"></param>
        public Member ModifyName(String newMemberName)
        {
            Name = newMemberName;
            return this;
        }

        /// <summary>
        /// 修改是否最大化
        /// </summary>
        /// <param name="isMax"></param>
        /// <returns></returns>
        public Member ModifyIsMax(Boolean isMax)
        {
            IsMax = isMax;
            return this;
        }

        /// <summary>
        /// 修改打开后是否铺满全屏
        /// </summary>
        /// <param name="isFull"></param>
        /// <returns></returns>
        public Member ModifyIsFull(Boolean isFull)
        {
            IsFull = isFull;
            return this;
        }

        /// <summary>
        /// 修改加载后是否显示工具条
        /// </summary>
        /// <param name="isSetbar"></param>
        /// <returns></returns>
        public Member ModifyIsSetbar(Boolean isSetbar)
        {
            IsSetbar = isSetbar;
            return this;
        }

        /// <summary>
        /// 修改打开是否最大化
        /// </summary>
        /// <param name="isOpenMax"></param>
        /// <returns></returns>
        public Member ModifyIsOpenMax(Boolean isOpenMax)
        {
            IsOpenMax = isOpenMax;
            return this;
        }

        /// <summary>
        /// 修改是否锁定
        /// </summary>
        /// <param name="isLock"></param>
        /// <returns></returns>
        public Member ModifyIsLock(Boolean isLock)
        {
            IsLock = isLock;
            return this;
        }

        /// <summary>
        /// 修改是否为福莱希
        /// </summary>
        /// <param name="isFlash"></param>
        /// <returns></returns>
        public Member ModifyIsFlash(Boolean isFlash)
        {
            IsFlash = isFlash;
            return this;
        }

        /// <summary>
        /// 修改是否可以拖动
        /// </summary>
        /// <param name="isDraw"></param>
        /// <returns></returns>
        public Member ModifyIsDraw(Boolean isDraw)
        {
            IsDraw = isDraw;
            return this;
        }

        /// <summary>
        /// 修改是否可以拉伸
        /// </summary>
        /// <param name="isResize"></param>
        /// <returns></returns>
        public Member ModifyIsResize(Boolean isResize)
        {
            IsResize = isResize;
            return this;
        }

        /// <summary>
        /// 桌面成员移入文件夹
        /// </summary>
        /// <param name="folderId"></param>
        public Member InFolder(Int32 folderId)
        {
            if (folderId <= 0)
            {
                throw new BusinessException($"{nameof(folderId)}:不能为0");
            }
            FolderId = folderId;
            return this;
        }

        /// <summary>
        /// 桌面成员移出文件夹
        /// </summary>
        public Member OutFolder()
        {
            FolderId = 0;
            return this;
        }

        /// <summary>
        /// 移除成员
        /// </summary>
        public void Remove()
        {
            IsDeleted = true;
        }

        /// <summary>
        /// 成员移动到码头
        /// </summary>
        /// <returns></returns>
        public Member InDock()
        {
            IsOnDock = true;
            return this;
        }

        /// <summary>
        /// 成员移出码头
        /// </summary>
        /// <returns></returns>
        public Member OutDock()
        {
            IsOnDock = false;
            return this;
        }

        /// <summary>
        /// 成员移动到其他桌面
        /// </summary>
        /// <param name="deskId"></param>
        /// <returns></returns>
        public Member ToOtherDesk(Int32 deskId)
        {
            if (deskId <= 0)
            {
                throw new BusinessException($"{nameof(deskId)}:不能为0");
            }

            DeskId = deskId;

            return this;
        }

        #endregion
    }
}
