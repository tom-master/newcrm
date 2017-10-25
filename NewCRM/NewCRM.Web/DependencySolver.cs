using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewCRM.Web
{
    public class DependencySolver : IDependencyResolver
    {
        private readonly ComposablePartCatalog _catalog;
        private const String _httpContextKey = "MefContainerKey";

        public DependencySolver(ComposablePartCatalog catalog)
        {
            _catalog = catalog;
        }

        public CompositionContainer Container
        {
            get
            {
                if (!HttpContext.Current.Items.Contains(_httpContextKey))
                {
                    HttpContext.Current.Items.Add(_httpContextKey, new CompositionContainer(_catalog));
                }

                CompositionContainer container = (CompositionContainer)HttpContext.Current.Items[_httpContextKey];
                HttpContext.Current.Application["Container"] = container;

                return container;
            }
        }

        #region IDependencyResolver Members

        public Object GetService(Type serviceType)
        {
            String contractName = AttributedModelServices.GetContractName(serviceType);
            return Container.GetExportedValueOrDefault<Object>(contractName);
        }

        public IEnumerable<Object> GetServices(Type serviceType)
        {
            return Container.GetExportedValues<Object>(serviceType.FullName);
        }


        #endregion
    }
}