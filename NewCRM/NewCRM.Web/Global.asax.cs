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


            var catalog = new AggregateCatalog();

            catalog.Catalogs.Add(new DirectoryCatalog(@"E:\NewCRM\NewCRM\NewCRM.Application.Interface\bin\Debug"));

            catalog.Catalogs.Add(new DirectoryCatalog(@"E:\NewCRM\NewCRM\NewCRM.ApplicationService\bin\Debug"));

            catalog.Catalogs.Add(new DirectoryCatalog(@"E:\NewCRM\NewCRM\NewCRM.Domain\bin\Debug"));

            catalog.Catalogs.Add(new DirectoryCatalog(@"E:\NewCRM\NewCRM\NewCRM.Domain.Interface\bin\Debug"));

            catalog.Catalogs.Add(new DirectoryCatalog(@"E:\NewCRM\NewCRM\NewCRM.DomainService\bin\Debug"));

            catalog.Catalogs.Add(new DirectoryCatalog(@"E:\NewCRM\NewCRM\NewCRM.Repository\bin\Debug"));

            catalog.Catalogs.Add(new DirectoryCatalog(@"E:\NewCRM\NewCRM\NewCRM.App.QueryServices\bin\Debug"));

            catalog.Catalogs.Add(new DirectoryCatalog(@"E:\NewCRM\NewCRM\NewCRM.Web\bin"));

            var container = new CompositionContainer(catalog/*, CompositionOptions.DisableSilentRejection*/);

            DependencyResolver.SetResolver(new MefDependencySolver(container.Catalog));

        }
    }
}
