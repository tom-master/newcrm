using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NewCRM.Web.Controllers.ControllerHelper;
using Newtonsoft.Json;

namespace NewCRM.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Dictionary<String, IEnumerable<String>> powers = new Dictionary<String, IEnumerable<String>>();

            //var internalAssembly = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsSubclassOf(typeof(BaseController)));

            //foreach (var type in internalAssembly)
            //{
            //    var internalMethods = type.GetMethods().Where(w => w.ReturnType == typeof(ActionResult)).ToList().Select(method => method.Name);

            //    powers.Add(type.Name, internalMethods);
            //}

            //var a = JsonConvert.SerializeObject(powers);


            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var catalog = new AggregateCatalog();

            catalog.Catalogs.Add(new AssemblyCatalog(@"E:\NewCRM\NewCRM\NewCRM.Application.Interface\bin\Debug\NewCRM.Application.Interface.dll"));

            catalog.Catalogs.Add(new AssemblyCatalog(@"E:\NewCRM\NewCRM\NewCRM.ApplicationService\bin\Debug\NewCRM.Application.Services.dll"));

            catalog.Catalogs.Add(new AssemblyCatalog(@"E:\NewCRM\NewCRM\NewCRM.Domain\bin\Debug\NewCRM.Domain.dll"));

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
