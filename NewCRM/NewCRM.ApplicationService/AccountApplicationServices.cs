using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.ApplicationService.IApplicationService;

namespace NewCRM.ApplicationService
{
    [Export(typeof(IAccountApplicationServices))]
    internal class AccountApplicationServices : IAccountApplicationServices
    {
        public void Login(String userName, String password)
        {

        }

        public void Logout(Int32 userId)
        {

        }

        public void Enable(Int32 userId)
        {

        }

        public void Disable(Int32 userId)
        {

        }
    }
}
