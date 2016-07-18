using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Domain.Services
{
    public interface ISkinServices
    {
        #region skin

        void ModifySkin(Int32 userId, String newSkin);

        #endregion
    }
}
