using NewCRM.Application.Services;
using NewCRM.Application.Services.Interface;
using NewCRM.Domain.Factory.DomainCreate;
using NewCRM.Domain.Factory.DomainCreate.ConcreteFactory;
using NewCRM.Domain.Factory.DomainQuery.ConcreteQuery;
using NewCRM.Domain.Factory.DomainQuery.Query;
using NewCRM.Domain.Factory.DomainSpecification.ConcreteSpecification;
using NewCRM.Domain.Factory.DomainSpecification.Factory;
using NewCRM.Domain.Repositories;
using NewCRM.Domain.Repositories.IRepository.Agent;
using NewCRM.Domain.Repositories.IRepository.Security;
using NewCRM.Domain.Repositories.IRepository.System;
using NewCRM.Domain.Services.BoundedContext.Agent;
using NewCRM.Domain.Services.BoundedContextMember;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.UnitWork;
using NewCRM.Repository.DataBaseProvider.EF;
using NewCRM.Repository.DataBaseProvider.Redis;
using NewCRM.Repository.DataBaseProvider.Redis.InternalHelper;
using NewCRM.Repository.RepositoryImpl.Agent;
using NewCRM.Repository.RepositoryImpl.Security;
using NewCRM.Repository.RepositoryImpl.System;
using NewCRM.Repository.UnitOfWorkProvide;
using System;

using Unity;
using Unity.AspNet.Mvc;
using Unity.Lifetime;

namespace NewCRM.Web
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();

            container.RegisterType<QueryBase, DefaultQuery>("DefaultQuery");
            container.RegisterType<QueryBase, DefaultQueryFormCache>("DefaultQueryFormCache");

            container.RegisterType<IAccountServices, AccountServices>(new PerRequestLifetimeManager());
            container.RegisterType<ISecurityServices, SecurityServices>(new PerRequestLifetimeManager());
            container.RegisterType<IAppServices, AppServices>(new PerRequestLifetimeManager());
            container.RegisterType<IDeskServices, DeskServices>(new PerRequestLifetimeManager());
            container.RegisterType<IWallpaperServices, WallpaperServices>(new PerRequestLifetimeManager());
            container.RegisterType<ISkinServices, SkinServices>(new PerRequestLifetimeManager());
            container.RegisterType<ILoggerServices, LoggerServices>(new PerRequestLifetimeManager());
            container.RegisterType<IInstallAppServices, InstallAppServices>(new PerRequestLifetimeManager());
            container.RegisterType<IModifyAppInfoServices, ModifyAppInfoServices>(new PerRequestLifetimeManager());
            container.RegisterType<IModifyAppTypeServices, ModifyAppTypeServices>(new PerRequestLifetimeManager());
            container.RegisterType<IAccountContext, AccountContext>(new PerRequestLifetimeManager());
            container.RegisterType<IModifyDeskMemberServices, ModifyDeskMemberServices>(new PerRequestLifetimeManager());
            container.RegisterType<IModifyDockPostionServices, ModifyDockPostionServices>(new PerRequestLifetimeManager());
            container.RegisterType<ICreateNewFolderServices, CreateNewFolderServices>(new PerRequestLifetimeManager());
            container.RegisterType<IModifyDeskMemberPostionServices, ModifyDeskMemberPostionServices>(new PerRequestLifetimeManager());
            container.RegisterType<IModifyWallpaperServices, ModifyWallpaperServices>(new PerRequestLifetimeManager());

            container.RegisterType<IUnitOfWork, EfUnitOfWorkContext>(new ContainerControlledLifetimeManager());

            container.RegisterType<IDomainModelQueryProvider, QueryProvider>(new PerRequestLifetimeManager());
            container.RegisterType<IDomainModelQueryProviderFormCache, QueryProviderFormCache>(new PerRequestLifetimeManager());

            container.RegisterType<ICacheQueryProvider, DefaultRedisQueryProvider>("ICacheQueryProvider", new PerRequestLifetimeManager());

            container.RegisterType<SpecificationFactory, DefaultSpecificationFactory>(new PerRequestLifetimeManager());
            container.RegisterType<DomainFactory, DefaultDomainFactory>(new PerRequestLifetimeManager());


            container.RegisterType<IAccountRepository, AccountRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITitleRepository, TitleRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IRoleRepository, RoleRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAppRepository, AppRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAppTypeRepository, AppTypeRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDeskRepository, DeskRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILogRepository, LogRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IOnlineRepository, OnlineRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IWallpaperRepository, WallpaperRepository>(new ContainerControlledLifetimeManager());
        }
    }
}