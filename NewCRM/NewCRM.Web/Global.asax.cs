using System;
using System.ComponentModel.Composition;
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



            catalog.Catalogs.Add(new AssemblyCatalog(@"E:\NewCRM\NewCRM\NewCRM.Application.Interface\bin\Debug\NewCRM.Application.Interface.dll"));

            catalog.Catalogs.Add(new AssemblyCatalog(@"E:\NewCRM\NewCRM\NewCRM.ApplicationService\bin\Debug\NewCRM.Application.Services.dll"));

            catalog.Catalogs.Add(new AssemblyCatalog(@"E:\NewCRM\NewCRM\NewCRM.Domain\bin\Debug\NewCRM.Domain.Entities.dll"));

            catalog.Catalogs.Add(new AssemblyCatalog(@"E:\NewCRM\NewCRM\NewCRM.Domain.Interface\bin\Debug\NewCRM.Domain.Interface.dll"));

            catalog.Catalogs.Add(new AssemblyCatalog(@"E:\NewCRM\NewCRM\NewCRM.DomainService\bin\Debug\NewCRM.Domain.Services.dll"));

            catalog.Catalogs.Add(new AssemblyCatalog(@"E:\NewCRM\NewCRM\NewCRM.Repository\bin\Debug\NewCRM.Repository.dll"));

            catalog.Catalogs.Add(new AssemblyCatalog(@"E:\NewCRM\NewCRM\NewCRM.Web\bin\NewCRM.Web.dll"));

            var container = new CompositionContainer(catalog);

            container.ComposeParts(this);

            DependencyResolver.SetResolver(new MefDependencySolver(container.Catalog));

        }
    }
}
