using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Application.Services.IApplicationService
{
    public interface IDeskApplicationServices
    {
        #region desk

        /// <summary>
        /// 修改默认显示的桌面
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newDefaultDeskNumber"></param>
        void ModifyDefaultDeskNumber(Int32 userId, Int32 newDefaultDeskNumber);

        /// <summary>
        /// 修改码头的位置
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="defaultDeskNumber"></param>
        /// <param name="newPosition"></param>
        void ModifyDockPosition(Int32 userId, Int32 defaultDeskNumber, String newPosition);
        #endregion
    }
}
