using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomExtension;

namespace TestCenter
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var a =EnumExtensions.ParseToEnum<AppStyle>(1);
        }
    }
}
