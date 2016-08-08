using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Domain.DomainModel.Security;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Domain.Repositories;
using NewCRM.Domain.ValueObject;
using NewCRM.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewCRM.Domain.Test
{
    [TestClass]
    public class UnitTestUser
    {
        [TestMethod]
        public void TestMethod()
        {
            var a =AppStyle.Widget;
        }

        public IEnumerable<TT> Bb()
        {
            var r = Aa();

            foreach (var o in r)
            {
                yield return new TT
                {
                    Id = o.Id,
                    Name = o.Value,
                    Type = o.Type
                };
            }
        }


        private List<dynamic> Aa()
        {
            return typeof(AppStyle).GetFields().Where(field => field.CustomAttributes.Any()).Select(s => new { s.CustomAttributes.ToArray()[0].ConstructorArguments[0].Value, Id = s.GetRawConstantValue(), Type = typeof(AppStyle).Name }).Cast<dynamic>().ToList();
        }
    }

    public sealed class TT
    {
        public Int32 Id { get; set; }

        public String Name { get; set; }

        public String Type { get; set; }

    }
}
