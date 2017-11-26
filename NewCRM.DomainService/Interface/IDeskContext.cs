using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Domain.Services.Interface
{
    public interface IDeskContext
    {
        /// <summary>
        /// 修改成员排列方向X轴
        /// </summary>
        void ModifyMemberDirectionToX(Int32 accountId);

        /// <summary>
        /// 修改成员排列方向X轴
        /// </summary>
        void ModifyMemberDirectionToY(Int32 accountId);


        /// <summary>
        /// 修改成员图标显示大小
        /// </summary>
        void ModifyMemberDisplayIconSize(Int32 accountId, Int32 newSize);

        /// <summary>
        /// 修改成员之间的垂直间距
        /// </summary>
        void ModifyMemberVerticalSpacing(Int32 accountId, Int32 newSize);

        /// <summary>
        /// 修改成员之间的水平间距
        /// </summary>
        void ModifyMemberHorizontalSpacing(Int32 accountId, Int32 newSize);

        /// <summary>
        /// 修改默认显示的桌面编号
        /// </summary>
        void ModifyDefaultDeskNumber(Int32 accountId, Int32 newDefaultDeskNumber);

    }
}
