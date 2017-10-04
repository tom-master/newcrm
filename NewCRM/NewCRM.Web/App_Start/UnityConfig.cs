using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using NewCRM.Application.Services.Interface;
using NewCRM.Application.Services;
using NewCRM.Domain;
using NewCRM.Domain.Factory;
using NewCRM.Domain.Factory.DomainQuery.Query;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.Services.BoundedContext.Agent;
using NewCRM.Domain.UnitWork;
using NewCRM.Repository.UnitOfWorkProvide;
using NewCRM.Domain.Factory.DomainQuery.ConcreteQuery;
using NewCRM.Domain.Factory.DomainSpecification.ConcreteSpecification;
using NewCRM.Domain.Factory.DomainSpecification.Factory;
using NewCRM.Repository;
using NewCRM.Domain.Repositories;
using NewCRM.Repository.DataBaseProvider.EF;
using NewCRM.Repository.DataBaseProvider.Mongodb;
using NewCRM.Domain.Factory.DomainCreate;
using NewCRM.Domain.Factory.DomainCreate.ConcreteFactory;
using NewCRM.Repository.DataBaseProvider.Redis;
using NewCRM.Repository.DataBaseProvider.Redis.InternalHelper;

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
			container.RegisterType<QueryBase, DefaultQueryFromMongodb>("DefaultQueryFromMongodb");

			container.RegisterType<IAccountApplicationServices, AccountApplicationServices>();
			container.RegisterType<IAccountContext, AccountContext>();
			container.RegisterType<IUnitOfWork, EfUnitOfWorkContext>();

			container.RegisterType<IDomainModelQueryProvider, DefaultMongodbQueryProvider>();
			container.RegisterType<IDomainModelQueryProvider, QueryProvider>();
			container.RegisterType<IDomainModelQueryProviderFormCache, QueryProviderFormCache>();

			container.RegisterType<ICacheQueryProvider, DefaultRedisQueryProvider>();

			container.RegisterType<SpecificationFactory, DefaultSpecificationFactory>();
			container.RegisterType<RepositoryFactory, DefaultRepositoryFactory>();
			container.RegisterType<DomainFactory, DefaultDomainFactory>();

		}
	}
}
