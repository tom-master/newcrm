using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Application.Services.IApplicationService
{
    public interface ISkinApplicationServices
    {
        #region skin

        /// <summary>
        /// 获取全部的皮肤
        /// </summary>
        /// <param name="skinPath"></param>
        /// <returns></returns>
        IDictionary<String, dynamic> GetAllSkin(String skinPath);

        /// <summary>
        /// 修改默认显示的皮肤
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newSkin"></param>
        void ModifySkin(Int32 userId, String newSkin);
        #endregion
    }
}
