using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCenter
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var a1 = Convert.ToBoolean(1);
            var a2 = Convert.ToBoolean(0);
            var a3 = Convert.ToBoolean("1");
            var a4 = Convert.ToBoolean("0");
        }
    }
}
