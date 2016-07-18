using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Domain.Services;
using NewCRM.Infrastructure.CommonTools.CustomHelper;

namespace NewCRM.Application.Services
{
    [Export(typeof(IAppApplicationServices))]
    internal class AppApplicationServices : IAppApplicationServices
    {
        private readonly Parameter _validateParameter = new Parameter();

        [Import]
        private IAppServices _appServices;


        public IDictionary<Int32, IList<dynamic>> GetUserApp(Int32 userId)
        {
            _validateParameter.Validate(userId);
            return _appServices.GetApp(userId);
        }


        public void ModifyAppDirection(Int32 userId, String direction)
        {
            _validateParameter.Validate(userId).Validate(direction);
            _appServices.ModifyAppDirection(userId, direction);
        }

        public void ModifyAppIconSize(Int32 userId, Int32 newSize)
        {
            _validateParameter.Validate(userId).Validate(newSize);
            _appServices.ModifyAppIconSize(userId, newSize);
        }

        public void ModifyAppVerticalSpacing(Int32 userId, Int32 newSize)
        {
            _validateParameter.Validate(userId).Validate(newSize);
            _appServices.ModifyAppVerticalSpacing(userId, newSize);
        }

        public void ModifyAppHorizontalSpacing(Int32 userId, Int32 newSize)
        {
            _validateParameter.Validate(userId).Validate(newSize);
            _appServices.ModifyAppHorizontalSpacing(userId, newSize);
        }
    }
}
