using NewCRM.Repository.DataBaseProvider.EF;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NewCRM.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
	        Database.SetInitializer(new CreateDatabaseIfNotExists<NewCrmBackSite>());

			AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);

			var catalog = new AggregateCatalog();

			catalog.Catalogs.Add(new AssemblyCatalog(@"E:\dev\NewCRM-v1.0\NewCRM\NewCRM.Web\bin\NewCRM.Web.dll"));

			catalog.Catalogs.Add(new AssemblyCatalog(@"E:\dev\NewCRM-v1.0\NewCRM\NewCRM.Application.Interface\bin\Debug\NewCRM.Application.Interface.dll"));

			catalog.Catalogs.Add(new AssemblyCatalog(@"E:\dev\NewCRM-v1.0\NewCRM\NewCRM.ApplicationService\bin\Debug\NewCRM.Application.Services.dll"));

			catalog.Catalogs.Add(new AssemblyCatalog(@"E:\dev\NewCRM-v1.0\NewCRM\NewCRM.Domain\bin\Debug\NewCRM.Domain.dll"));

			catalog.Catalogs.Add(new AssemblyCatalog(@"E:\dev\NewCRM-v1.0\NewCRM\NewCRM.Domain.Interface\bin\Debug\NewCRM.Domain.Interface.dll"));

			catalog.Catalogs.Add(new AssemblyCatalog(@"E:\dev\NewCRM-v1.0\NewCRM\NewCRM.DomainService\bin\Debug\NewCRM.Domain.Services.dll"));

			catalog.Catalogs.Add(new AssemblyCatalog(@"E:\dev\NewCRM-v1.0\NewCRM\NewCRM.Repository\bin\Debug\NewCRM.Repository.dll"));

			var container = new CompositionContainer(catalog, true);

			container.ComposeParts(this);

			DependencyResolver.SetResolver(new DependencySolver(container.Catalog));


		}
    }
}
