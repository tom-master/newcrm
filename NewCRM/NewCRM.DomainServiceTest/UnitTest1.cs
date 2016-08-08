using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewCRM.Domain.Entities.ValueObject;
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
            //var fields = typeof(AppStyle).GetFields();

            //foreach (var fieldInfo in fields)
            //{
            //    var result = fieldInfo.CustomAttributes;
            //    if (result.Any())
            //    {
            //        var r = result.ToArray()[0].ConstructorArguments[0].Value;
            //    }
            //}

            //var result = typeof(AppStyle).GetFields().Where(w => w.CustomAttributes.Any()).Select(
            //     s => s.CustomAttributes.ToArray()[0].ConstructorArguments[0].Value).Cast<String>().ToArray();

            var r = typeof(AppStyle).BaseType;


        }
    }
}
