using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Domain.Services.Interface
{
    public interface ISkinContext
    {
        /// <summary>
        /// 修改皮肤
        /// </summary>
        void ModifySkin(Int32 accountId, String newSkin);
    }
}
