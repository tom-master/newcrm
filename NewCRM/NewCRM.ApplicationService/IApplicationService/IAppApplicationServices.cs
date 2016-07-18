using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Application.Services.IApplicationService
{
    public interface IAppApplicationServices
    {
        IDictionary<Int32, IList<dynamic>> GetUserApp(Int32 userId);

        #region app
        /// <summary>
        /// 修改app排列方向
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="direction"></param>
        void ModifyAppDirection(Int32 userId, String direction);

        /// <summary>
        /// 修改app图标大小
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newSize"></param>
        void ModifyAppIconSize(Int32 userId, Int32 newSize);

        /// <summary>
        /// 修改app垂直间距
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newSize"></param>
        void ModifyAppVerticalSpacing(Int32 userId, Int32 newSize);

        /// <summary>
        /// 修改app水平间距
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newSize"></param>
        void ModifyAppHorizontalSpacing(Int32 userId, Int32 newSize);
        #endregion

    }
}
