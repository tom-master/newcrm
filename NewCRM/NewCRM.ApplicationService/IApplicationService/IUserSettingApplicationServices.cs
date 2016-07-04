using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Application.Services.IApplicationService
{
    public interface IUserSettingApplicationServices
    {
        void ModifyDefaultDeskNumber(Int32 userId, Int32 newDefaultDeskNumber);

        void ModifyAppDirection(Int32 userId, String direction);
    }
}
