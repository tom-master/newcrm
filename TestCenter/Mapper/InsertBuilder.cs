using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Mapper
{

    public class InsertBuilder<TModel> : SqlBuilder<TModel> where TModel : class, new()
    {
        public InsertBuilder(TModel model) : base(model)
        {

        }

        public override void GenerateSqlHead()
        {
            Append($@" INSERT dbo.{ModelType.Name} (");
            Append($@" {String.Join(",", GetFields())} )");
            Append($@" VALUES ({String.Join(",", GetFields().Select(key => $@"@{key}"))})");

            foreach(var item in ModelInstance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(w => w.PropertyType.Name != "IList`1"))
            {
                AddSqlParameters(new SqlParameter($@"@{item.Name}", item.GetValue(ModelInstance)));
            }
        }
    }
}
