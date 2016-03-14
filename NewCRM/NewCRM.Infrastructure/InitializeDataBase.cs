using System.Data.Entity;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;

namespace NewCRM.Infrastructure
{
    public static class InitializeDataBase
    {
        public static void Initialize()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<NewCrmBackSite>());
            using (var db = new NewCrmBackSite())
            {
                db.Database.Initialize(false);
            }
        }
    }
}
