using System;
using System.ComponentModel;
using NewCRM.Domain.Entities.ValueObject;

namespace NewCRM.Domain.Entities.DomainModel.System
{
    /// <summary>
    /// 成员会有两种类型 1：app，2：文件夹
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
        /// 是否拖动
        /// </summary>
        public Boolean IsDraw { get; private set; }

        /// <summary>
        /// 是否打开最大化
        /// </summary>
        public Boolean IsOpenMax { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// 图标地址
        /// </summary>
        public String IconUrl { get; private set; }


        /// <summary>
        /// 成员类型
        /// </summary>
        public MemberType MemberType { get; private set; }

        #endregion

        #region public ctor
        /// <summary>
        /// 实例化一个成员对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="iconUrl"></param>
        /// <param name="appId"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Member(
            String name,
            String iconUrl,
            Int32 appId,
            Int32 width,
            Int32 height)
        {
            AppId = appId;
            Width = width > 800 ? 800 : width;
            Height = height > 600 ? 600 : height;
            IsDraw = false;
            IsOpenMax = false;
            Name = name;
            IconUrl = iconUrl;
            MemberType = appId == 0 ? MemberType.Folder : MemberType.App;
        }

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
