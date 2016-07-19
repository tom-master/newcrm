using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Domain.Services
{
    public interface IAppServices
    {
        /// <summary>
        /// 根据用户id获取app
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IDictionary<Int32, IList<dynamic>> GetApp(Int32 userId);


        /// <summary>
        /// 修改app的排列方向
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="direction"></param>
        void ModifyAppDirection(Int32 userId, String direction);

        /// <summary>
        /// 修改app图标的大小
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newSize"></param>
        void ModifyAppIconSize(Int32 userId, Int32 newSize);

        /// <summary>
        /// 修改app垂直距离
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newSize"></param>
        void ModifyAppVerticalSpacing(Int32 userId, Int32 newSize);

        /// <summary>
        /// 修改app水平距离
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newSize"></param>
        void ModifyAppHorizontalSpacing(Int32 userId, Int32 newSize);
    }
}
