using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.Factory;

namespace NewCRM.Repository.Test
{
    [TestClass]
    public class UnitTest1
    {

        [Import]
        private RepositoryFactory Repository { get; set; }


        [TestMethod]
        public void TestMethod1()
        {
            Compose();

            var r = Repository.Create<Account>();
        }

        private void Compose()
        {
            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            CompositionContainer container = new CompositionContainer(new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory));
            container.ComposeParts(this);
        }
    }
}
