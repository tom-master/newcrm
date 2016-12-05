using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewCRM.Web.Controllers.ControllerHelper;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var assembly = Assembly.LoadFile(@"E:\NewCRM\NewCRM\NewCRM.Web\bin\NewCRM.Web.dll");

            var types = assembly.GetType();

            var a = types.IsSubclassOf(typeof(BaseController));

        }
    }
}
