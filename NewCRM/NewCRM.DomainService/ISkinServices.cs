using System;

namespace NewCRM.Domain.Services
{
    public interface ISkinServices
    {
        #region skin

        void ModifySkin(Int32 userId, String newSkin);

        #endregion
    }
}
