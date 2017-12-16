using NewCRM.Infrastructure.CommonTools;
using NewLib;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NewCRM.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<NewCrmContext>());

            ProfileManager.Init();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityMvcActivator.Start();
        }
    }
}
