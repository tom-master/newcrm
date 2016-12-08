using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NewCRM.Application.Test
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod1()
        {

            
        }

    }

    public class TestModel
    {
        [Required]
        public String Name { get; set; } = "11111";

        [Required, StringLength(10)]
        public String Value { get; set; } = "132213";
    }
}
