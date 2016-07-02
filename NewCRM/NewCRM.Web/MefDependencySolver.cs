using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Web;
using System.Web.Mvc;

namespace NewCRM.Web
{
    public class MefDependencySolver : IDependencyResolver
    {
        private readonly ComposablePartCatalog _catalog;

        private static readonly String _containerKey = "MefKey";


        public MefDependencySolver(ComposablePartCatalog composablePartCatalog)
        {
            _catalog = composablePartCatalog;
        }



        public CompositionContainer Container
        {
            get
            {
                if (!HttpContext.Current.Items.Contains(_containerKey))
                {
                    HttpContext.Current.Items.Add(_containerKey, new CompositionContainer(_catalog));
                }

                var container = HttpContext.Current.Items[_containerKey] as CompositionContainer;
                HttpContext.Current.Application["Container"] = container;
                return container;
            }
        }


        public Object GetService(Type serviceType)
        {
            var contractName = AttributedModelServices.GetContractName(serviceType);

            return Container.GetExportedValueOrDefault<Object>(contractName);

        }

        public IEnumerable<Object> GetServices(Type serviceType)
        {
            return Container.GetExportedValues<Object>(serviceType.FullName);
        }
    }
}