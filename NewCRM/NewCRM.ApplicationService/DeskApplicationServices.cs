using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Domain.Services;
using NewCRM.Infrastructure.CommonTools.CustomHelper;

namespace NewCRM.Application.Services
{
    [Export(typeof(IDeskApplicationServices))]
    internal class DeskApplicationServices : IDeskApplicationServices
    {
        [Import]
        private IDeskServices _deskServices;


        private readonly Parameter _validateParameter = new Parameter();
        public void ModifyDefaultDeskNumber(Int32 userId, Int32 newDefaultDeskNumber)
        {
            _validateParameter.Validate(userId).Validate(newDefaultDeskNumber);
            _deskServices.ModifyDefaultShowDesk(userId, newDefaultDeskNumber);
        }


        public void ModifyDockPosition(Int32 userId, Int32 defaultDeskNumber, String newPosition)
        {
            _validateParameter.Validate(userId).Validate(defaultDeskNumber).Validate(newPosition);

            _deskServices.ModifyDockPosition(userId, defaultDeskNumber, newPosition);
        }
    }
}
