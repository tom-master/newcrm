using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewCRM.DomainService;
using NewCRM.DomainService.Impl;

namespace NewCRM.DomainServiceTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            IPlatformDomainService platformDomainService = new PlatformDomainService();

            platformDomainService.UserApp(2);
        }
    }
}
