using System;
using NewCRM.Application.Services;
using NewCRM.Application.Services.Interface;
using NewCRM.Domain.Services.BoundedContext;
using NewCRM.Domain.Services.Interface;
using NewCRM.Repository.DataBaseProvider.Redis.InternalHelper;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Injection;

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

            container.RegisterType<IAccountServices, AccountServices>(new PerRequestLifetimeManager());
            container.RegisterType<ISecurityServices, SecurityServices>(new PerRequestLifetimeManager());
            container.RegisterType<IAppServices, AppServices>(new PerRequestLifetimeManager());
            container.RegisterType<IDeskServices, DeskServices>(new PerRequestLifetimeManager());
            container.RegisterType<IWallpaperServices, WallpaperServices>(new PerRequestLifetimeManager());
            container.RegisterType<ISkinServices, SkinServices>(new PerRequestLifetimeManager());
            container.RegisterType<ILoggerServices, LoggerServices>(new PerRequestLifetimeManager());

            container.RegisterType<IAccountContext, AccountContext>(new PerRequestLifetimeManager());
            container.RegisterType<IAppContext, AppContext>(new PerRequestLifetimeManager());
            container.RegisterType<IDeskContext, DeskContext>(new PerRequestLifetimeManager());
            container.RegisterType<ILoggerContext, LoggerContext>(new PerRequestLifetimeManager());
            container.RegisterType<IMemberContext, MemberContext>(new PerRequestLifetimeManager());
            container.RegisterType<ISecurityContext, SecurityContext>(new PerRequestLifetimeManager());
            container.RegisterType<ISkinContext, SkinContext>(new PerRequestLifetimeManager());
            container.RegisterType<IWallpaperContext, WallpaperContext>(new PerRequestLifetimeManager());
            container.RegisterType<ICacheQueryProvider, DefaultRedisQueryProvider>();

        }
    }
}