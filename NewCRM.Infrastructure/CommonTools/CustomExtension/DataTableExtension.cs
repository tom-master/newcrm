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
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public static IList<T> AsList<T>(this DataTable dataTable) where T : class, new()
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return new List<T>();
            }

            return ConvertToList<T>(dataTable);
        }

        /// <summary>
        /// 获取单值
        /// </summary>
        /// <returns></returns>
        public static T AsSignal<T>(this DataTable dataTable) where T : class, new()
        {
            return AsList<T>(dataTable).FirstOrDefault();
        }

        private static List<T> ConvertToList<T>(DataTable dt) where T : class, new()
        {
            var list = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                var t = new T();
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo propertyInfo in propertys)
                {
                    var tempName = propertyInfo.Name;
                    if (dt.Columns.Contains(tempName))
                    {
                        var value = dr[tempName];
                        if (value != DBNull.Value)
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
