using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Linq;
using NewCRM.Domain.Repositories;

namespace TestCenter
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;

            var assemblys = Assembly.LoadFile($"{basePath}/NewCRM.Repository.dll");
            var repositorys = assemblys.DefinedTypes.ToList().Where(w => w.Name.EndsWith("Repository"));
        }
    }
}
