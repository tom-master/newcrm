using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewCRM.Domain.Entitys.Agent;
using Newtonsoft.Json;

namespace NewCRM.Domain.Test
{
    [TestClass]
    public class UnitTestUser
    {
        [TestMethod]
        public void TestMethod()
        {
            //var settings = new JsonSerializerSettings
            //{
            //    ContractResolver = new PrivateSetterContractResolver()
            //};


            List<Object> currencyListCast = new List<Object>
            {
                new
                    {Id = "1"},
                new
                    {Id = "2"},
                new
                    {Id = "3"}
            };

            var a = (Currency)currencyListCast.FirstOrDefault();

        }
    }
    public class Currency
    {
        public String Id { get; private set; }

        public String Name { get; private set; }

        public DateTime AddTime { get; set; } = DateTime.Now;
    }
}
