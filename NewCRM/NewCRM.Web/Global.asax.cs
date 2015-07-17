using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NewCRM.Infrastructure;

namespace NewCRM.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

      
            InitializeDataBase.Initialize();
        }
    }
}
