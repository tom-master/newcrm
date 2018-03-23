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
        private Boolean _isVerifyModel;

        public InsertBuilder(TModel model, Boolean isVerifyModel = false) : base(model)
        {
            _isVerifyModel = isVerifyModel;
        }

        public override String ParseToSql()
        {
            if(_isVerifyModel)
            {
                VerifyModel();
            }

            Append($@" INSERT dbo.{ModelType.Name} (");
            Append($@" {String.Join(",", GetFields())} )");
            Append($@" VALUES ({String.Join(",", GetFields().Select(key => $@"@{key}"))})");

            foreach(var item in ModelInstance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(w => w.PropertyType.Name != "IList`1" && w.CustomAttributes.Count() != 0))
            {
                Parameters.Add(new SqlParameter($@"@{item.Name}", item.GetValue(ModelInstance)));
            }

            return ToString();
        }
    }
}
