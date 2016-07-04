using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Application.Services.IApplicationService
{
    public interface IAppApplicationServices
    {
        IDictionary<Int32, IList<dynamic>> GetUserApp(Int32 userId);

    }
}
