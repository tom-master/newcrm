using System;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;

namespace NewCRM.Web
{
    public sealed class AutoFac
    {


        public static void Init()
        {
            
            var web = Assembly.LoadFile(@"E:\NewCRM\NewCRM\NewCRM.Web\bin\NewCRM.Web.dll");

            var applicationInterface = Assembly.LoadFile(@"E:\NewCRM\NewCRM\NewCRM.Application.Interface\bin\Debug\NewCRM.Application.Interface.dll");

            var applicationServices = Assembly.LoadFile(@"E:\NewCRM\NewCRM\NewCRM.ApplicationService\bin\Debug\NewCRM.Application.Services.dll");

            var domain = Assembly.LoadFile(@"E:\NewCRM\NewCRM\NewCRM.Domain\bin\Debug\NewCRM.Domain.dll");

            var domainInterface = Assembly.LoadFile(@"E:\NewCRM\NewCRM\NewCRM.Domain.Interface\bin\Debug\NewCRM.Domain.Interface.dll");

            var domainServices = Assembly.LoadFile(@"E:\NewCRM\NewCRM\NewCRM.DomainService\bin\Debug\NewCRM.Domain.Services.dll");

            var repository = Assembly.LoadFile(@"E:\NewCRM\NewCRM\NewCRM.Repository\bin\Debug\NewCRM.Repository.dll");





            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterAssemblyTypes(web, applicationInterface, applicationServices, domain, domainInterface, domainServices, repository)
                .AsImplementedInterfaces();
            
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }

    }
}