using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using ConsoleApplication1.Mapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewCRM.Domain.Entitys.Agent;

namespace TestCenter
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var accountRoles = new List<AccountRole>
            {
                new AccountRole(1,2),
                new AccountRole(1,3)
            };
            var account = new Account("xiaofan", "xiaofan123", accountRoles);
            Insert(account);

            account.ModifyLockScreenPassword("wasd123").ModifyLoginPassword("123wasd");
            Modify(account, a => a.Id == 1 || a.Name == "123");
        }

        public void Insert<TModel>(TModel model) where TModel : class, new()
        {
            var modelType = model.GetType();
            var sqlBuilder = new InsertBuilder<TModel>(model);

            var sql = sqlBuilder.ParseToSql();
            var sqlParameters = sqlBuilder.GetParameters();

        }

        public void Modify<TModel>(TModel model, Expression<Func<TModel, Boolean>> where = null) where TModel : class, new()
        {
            var sqlBuilder = new UpdateBuilder<TModel>(model, where);
            var sql = sqlBuilder.ParseToSql();
            var sqlParameters = sqlBuilder.GetParameters();
        }
    }
}
