using System.Data.Entity.Migrations;
using NewCRM.Repository.DatabaseProvider.EF.Context;
using NewCRM.Repository.DataBaseProvider.EF;

namespace NewCRM.Repository.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<NewCrmBackContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            
        }

        protected override void Seed(NewCrmBackContext context)
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
        }
    }
}
