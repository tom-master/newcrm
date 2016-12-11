using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
          
        }
    }

    public abstract class Test1
    {
        public abstract void T1();
    }

    public abstract class Test2 : Test1
    {
        public override void T1()
        {
            throw new NotImplementedException();
        }


        public abstract void T2();
    }

    public class Test3 : Test2
    {
        public override void T2()
        {
            throw new NotImplementedException();
        }
    }



}
