using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustomExtension;

namespace NewCrm.InfrastructureTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Object o = new List<String>();

            var r = o.GetType().IsAssignableToGenericType(typeof(IEnumerable<>));
        }
    }
}