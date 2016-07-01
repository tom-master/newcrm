using System;

namespace NewCRM.Domain.DomainModel.System
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
        /// app移入文件夹
        /// </summary>
        /// <param name="folderId"></param>
        public Member AppInFolder(Int32 folderId)
        {
            if (folderId <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(folderId)}:不能为0");
            }
            FolderId = folderId;
            return this;
        }

        /// <summary>
        /// app移出文件夹
        /// </summary>
        public Member AppOutFolder()
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

        #endregion

    }
}
