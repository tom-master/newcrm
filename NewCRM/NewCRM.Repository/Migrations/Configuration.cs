using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Repository.DatabaseProvider.EF.Context;
using NewCRM.Repository.DataBaseProvider.EF;

namespace NewCRM.Repository.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<NewCrmContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;

        }

        protected override void Seed(NewCrmContext context)
        {
            //  This method will be called after migrating to the lates  t version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            //if (Debugger.IsAttached == false)
            //{
            //    Debugger.Launch();
            //}
        }
    }
}
