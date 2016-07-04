using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Domain.Services
{
    public interface IUserSettingServices
    {
        void ModifyDefaultShowDesk(Int32 userId, Int32 newDefaultDeskNumber);

        void ModifyAppDirection(Int32 userId, String direction);
    }
}
