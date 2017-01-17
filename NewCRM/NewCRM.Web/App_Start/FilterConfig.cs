using System.Web.Mvc;
using NewCRM.Web.Filter;

namespace NewCRM.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            filters.Add(new ErrorFilterAttribute());

           // filters.Add(new AuthFilter());

        }
    }
}
