using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewCRM.Domain.Entities.DomainModel;
using NewCRM.Domain.Entities.DomainSpecification;
using NewCRM.Domain.Entities.DomainSpecification.ConcreteSpecification;

namespace NewCRM.Domain.Test
{
    [TestClass]
    public class UnitTestUser
    {
        [TestMethod]
        public void TestMethod()
        {
            IList<Entity> entities = new List<Entity>
            {

                new Entity {Id=1,Age=10,Name="xiaofan14" },
                new Entity {Id=2,Age=9,Name="xiaofan12" },
                new Entity {Id=3,Age=8,Name="xiaofan13" },
                new Entity {Id=4,Age=7,Name="xiaofan14" },
                new Entity {Id=5,Age=6,Name="xiaofan15" },
                new Entity {Id=6,Age=5,Name="xiaofan16" },
                new Entity {Id=7,Age=4,Name="xiaofan17" },
                new Entity {Id=8,Age=3,Name="xiaofan18" },
                new Entity {Id=9,Age=2,Name="xiaofan19" },
                new Entity {Id=10,Age=1,Name="xiaofan10" }

            };
            

            var specification = new DefaultSpecificationFactory().Create<Entity>();
            specification.OrderByDescending(d => d.Id);

        }
    }


    public class Entity : DomainModelBase, IAggregationRoot
    {
        public Int32 Id { get; set; }
        public Int32 Age { get; set; }

        public String Name { get; set; }
    }
}
