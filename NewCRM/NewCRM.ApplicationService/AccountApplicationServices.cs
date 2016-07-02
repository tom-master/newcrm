using System;
using System.ComponentModel.Composition;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.DomainService;

namespace NewCRM.Application.Services
{
    [Export(typeof(IAccountApplicationServices))]
    internal class AccountApplicationServices : IAccountApplicationServices
    {
        [Import]
        private IAccountServices _accountServices;

        public void Login(String userName, String password)
        {
            _accountServices.Validate(userName, password);
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
