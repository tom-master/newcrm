﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Linq;
using NewCRM.Domain.Repositories;
using NewCRM.Infrastructure.CommonTools;

namespace TestCenter
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ProfileManager.Init(AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
