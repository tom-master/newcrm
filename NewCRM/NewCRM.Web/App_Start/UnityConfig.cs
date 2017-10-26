using Microsoft.Practices.Unity;
using NewCRM.Application.Services;
using NewCRM.Application.Services.Interface;
using NewCRM.Domain.Factory;
using NewCRM.Domain.Factory.DomainCreate;
using NewCRM.Domain.Factory.DomainCreate.ConcreteFactory;
using NewCRM.Domain.Factory.DomainQuery.ConcreteQuery;
using NewCRM.Domain.Factory.DomainQuery.Query;
using NewCRM.Domain.Factory.DomainSpecification.ConcreteSpecification;
using NewCRM.Domain.Factory.DomainSpecification.Factory;
using NewCRM.Domain.Repositories;
using NewCRM.Domain.Services.BoundedContext.Agent;
using NewCRM.Domain.Services.BoundedContextMember;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.UnitWork;
using NewCRM.Repository;
using NewCRM.Repository.DatabaseProvider.EF.Context;
using NewCRM.Repository.DataBaseProvider.EF;
using NewCRM.Repository.DataBaseProvider.Redis;
using NewCRM.Repository.DataBaseProvider.Redis.InternalHelper;
using NewCRM.Repository.UnitOfWorkProvide;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace NewCRM.Web.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();

            container.RegisterType<QueryBase, DefaultQuery>("DefaultQuery");
            container.RegisterType<QueryBase, DefaultQueryFormCache>("DefaultQueryFormCache");

            container.RegisterType<IAccountServices, AccountServices>();
            container.RegisterType<ISecurityServices, SecurityServices>();
            container.RegisterType<IAppServices, AppServices>();
            container.RegisterType<IDeskServices, DeskServices>();
            container.RegisterType<IWallpaperServices, WallpaperServices>();
            container.RegisterType<ISkinServices, SkinServices>();
            container.RegisterType<ILoggerServices, LoggerServices>();
            container.RegisterType<IInstallAppServices, InstallAppServices>();
            container.RegisterType<IModifyAppInfoServices, ModifyAppInfoServices>();
            container.RegisterType<IModifyAppTypeServices, ModifyAppTypeServices>();
            container.RegisterType<IAccountContext, AccountContext>();
            container.RegisterType<IModifyDeskMemberServices, ModifyDeskMemberServices>();
            container.RegisterType<IModifyDockPostionServices, ModifyDockPostionServices>();
            container.RegisterType<ICreateNewFolderServices, CreateNewFolderServices>();
            container.RegisterType<IModifyDeskMemberPostionServices, ModifyDeskMemberPostionServices>();
            container.RegisterType<IModifyWallpaperServices, ModifyWallpaperServices>();

            container.RegisterType<IUnitOfWork, EfUnitOfWorkContext>();

            container.RegisterType<IDomainModelQueryProvider, QueryProvider>();
            container.RegisterType<IDomainModelQueryProviderFormCache, QueryProviderFormCache>();

            container.RegisterType<ICacheQueryProvider, DefaultRedisQueryProvider>();

            container.RegisterType<DbContext, NewCrmContext>();
            container.RegisterType<SpecificationFactory, DefaultSpecificationFactory>();
            container.RegisterType<DomainFactory, DefaultDomainFactory>();
        }
    }
}
