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
    }
}
