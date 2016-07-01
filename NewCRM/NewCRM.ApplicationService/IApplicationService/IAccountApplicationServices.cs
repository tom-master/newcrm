using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.ApplicationService.IApplicationService
{
    public interface IAccountApplicationServices
    {
        void Login(String userName, String password);

        void Logout(Int32 userId);

        void Enable(Int32 userId);

        void Disable(Int32 userId);

    }
}
