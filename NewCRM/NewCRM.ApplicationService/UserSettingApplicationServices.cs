using System;
using System.ComponentModel.Composition;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Domain.Services;
using NewCRM.Infrastructure.CommonTools.CustomHelper;

namespace NewCRM.Application.Services
{
    [Export(typeof(IUserSettingApplicationServices))]
    internal class UserSettingApplicationServices : IUserSettingApplicationServices
    {
        [Import]
        private IUserSettingServices _userSettingServices;

        private readonly Parameter _validateParameter = new Parameter();

        public void ModifyDefaultDeskNumber(Int32 userId, Int32 newDefaultDeskNumber)
        {
            _validateParameter.Validate(userId).Validate(newDefaultDeskNumber);
            _userSettingServices.ModifyDefaultShowDesk(userId, newDefaultDeskNumber);
        }

        public void ModifyAppDirection(Int32 userId, String direction)
        {
            _validateParameter.Validate(userId).Validate(direction);
            _userSettingServices.ModifyAppDirection(userId, direction);
        }
    }
}
