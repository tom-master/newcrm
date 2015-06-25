using System.Web;
using System.Web.Mvc;

namespace NewCRM.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new GetException());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
