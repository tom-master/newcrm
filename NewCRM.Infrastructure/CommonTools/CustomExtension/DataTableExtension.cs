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
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                throw new BusinessException("转换失败");
            }

            var data = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static T AsSignal<T>(this DataTable dataTable) where T : class, new()
        {
            return AsList<T>(dataTable).FirstOrDefault();
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName && dr[column.ColumnName] != DBNull.Value)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
    }
}
