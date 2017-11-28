using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Infrastructure.CommonTools.CustomExtension
{
    public static class DataTableExtension
    {
        public static IList<T> AsList<T>(this DataTable dataTable) where T : class, new()
        {
            if(dataTable == null || dataTable.Rows.Count == 0)
            {
                throw new BusinessException("转换失败");
            }

            return ConvertToList<T>(dataTable);
        }

        public static T AsSignal<T>(this DataTable dataTable) where T : class, new()
        {
            return AsList<T>(dataTable).FirstOrDefault();
        }

        public static List<T> ConvertToList<T>(DataTable dt) where T : class, new()
        {
            var list = new List<T>();
            var t = new T();
            PropertyInfo[] propertys = t.GetType().GetProperties();
            foreach(DataRow dr in dt.Rows)
            {
                foreach(PropertyInfo propertyInfo in propertys)
                {
                    var tempName = propertyInfo.Name;
                    if(dt.Columns.Contains(tempName))
                    {
                        var value = dr[tempName];
                        if(value != DBNull.Value)
                        {
                            propertyInfo.SetValue(t, value, null);
                        }
                    }
                }

                list.Add(t);
            }
            return list;
        }
    }
}
