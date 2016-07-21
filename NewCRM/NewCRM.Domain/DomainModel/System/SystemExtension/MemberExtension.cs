using System;

namespace NewCRM.Domain.Entities.DomainModel.System
{
    public partial class Member
    {
        #region public method
        /// <summary>
        /// 修改成员的宽
        /// </summary>
        /// <param name="width"></param>
        public Member ModifyMemberWidth(Int32 width)
        {
            Width = width >= 800 ? 800 : width;

            return this;
        }

        /// <summary>
        /// 修改成员的高
        /// </summary>
        /// <param name="height"></param>
        public Member ModifyMemberHeight(Int32 height)
        {
            Height = height >= 600 ? 600 : height;
            return this;
        }

        /// <summary>
        /// 修改成员图标
        /// </summary>
        /// <param name="iconUrl"></param>
        public Member ModifyMemberIcon(String iconUrl)
        {
            IconUrl = iconUrl;
            return this;
        }

        /// <summary>
        /// 修改成员名称
        /// </summary>
        /// <param name="newMemberName"></param>
        public Member ModifyMemberName(String newMemberName)
        {
            Name = newMemberName;
            return this;
        }

        /// <summary>
        /// 修改成员是否可以拉伸
        /// </summary>
        /// <param name="isDraw"></param>
        public Member ModifyMemberIsDraw(Boolean isDraw)
        {
            IsDraw = isDraw;
            return this;
        }

        /// <summary>
        /// 修改成员是否在打开时默认最大化
        /// </summary>
        /// <param name="isOpenMax"></param>
        public Member ModifyMemberIsOpenMax(Boolean isOpenMax)
        {
            IsOpenMax = isOpenMax;
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
                throw new ArgumentOutOfRangeException($"{nameof(folderId)}:不能为0");
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
        public void RemoveMember()
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
                throw new ArgumentOutOfRangeException($"{nameof(deskId)}:不能为0");
            }

            DeskId = deskId;

            return this;
        }

        #endregion
    }
}
