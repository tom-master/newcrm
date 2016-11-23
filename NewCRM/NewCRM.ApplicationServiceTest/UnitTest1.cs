using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NewCRM.Application.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            IList<TestModel> _list = new List<TestModel>
            {
                new TestModel {Name = "1",Value = "a"},
                new TestModel{Name = "2",Value = "b"},
                new TestModel{Name = "3",Value = "c"},
            };

            var result = _list.Select(s => new
            {
                s.Name,
                s.Value
            }).Cast<TestModel>();


          
        }

    }

    public class TestModel 
    {
        public String Name { get; set; }

        public String Value { get; set; }
    }
}
