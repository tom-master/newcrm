using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Infrastructure.Repositories;

namespace NewCrm.InfrastructureTest
{
    [TestClass]
    public class UnitTest1
    {
        [ TestMethod ]
        public void TestMethod1 ()
        {
            RepositoryFactory<User>.CreateRepository();
        }
    }
}
