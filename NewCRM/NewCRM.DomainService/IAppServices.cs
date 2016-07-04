using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Domain.Services
{
    public interface IAppServices
    {
        IDictionary<Int32, IList<dynamic>> GetApp(Int32 userId);
    }
}
