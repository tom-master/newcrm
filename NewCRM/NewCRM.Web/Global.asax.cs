using System;
using System.ComponentModel.Composition.Hosting;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NewCRM.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            DependencyResolver.SetResolver(new MefDependencySolver(new DirectoryCatalog(AppDomain.CurrentDomain.SetupInformation.PrivateBinPath)));


        }
    }
}
