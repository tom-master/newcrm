using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entities.ValueObject;

namespace NewCRM.Dto.Dto
{
    public class MemberDto
    {
        #region public property

        public Int32 Id { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public Int32 AppId { get; set; }

        /// <summary>
        /// 成员的宽
        /// </summary>
        public Int32 Width { get; set; }

        /// <summary>
        /// 成员的高
        /// </summary>
        public Int32 Height { get; set; }

        /// <summary>
        /// 文件夹Id
        /// </summary>
        public Int32 FolderId { get; set; }

        /// <summary>
        /// 是否拖动
        /// </summary>
        public Boolean IsDraw { get; set; }

        /// <summary>
        /// 是否打开最大化
        /// </summary>
        public Boolean IsOpenMax { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 图标地址
        /// </summary>
        public String IconUrl { get; set; }


        /// <summary>
        /// app地址
        /// </summary>
        public String AppUrl { get; set; }

        /// <summary>
        /// 成员是否在应用码头上
        /// </summary>

        public Boolean IsOnDock { get; set; }


        /// <summary>
        /// 是否显示app底部的按钮
        /// </summary>
        public Boolean IsSetbar { get; set; }


        /// <summary>
        /// 成员类型
        /// </summary>
        public String MemberType { get; set; }

        #endregion
    }
}
