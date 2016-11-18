using System;
using System.ComponentModel;

namespace NewCRM.Infrastructure.CommonTools.CustomExtension
{
    /// <summary>
    ///     属性排序条件信息类
    /// </summary>
    public class PropertySortCondition
    {
        /// <summary>
        ///     构造一个指定属性名称的升序排序的排序条件
        /// </summary>
        /// <param name="propertyName">排序属性名称</param>
        public PropertySortCondition(String propertyName)
            : this(propertyName, ListSortDirection.Descending)
        { }

        /// <summary>
        ///     构造一个排序属性名称和排序方式的排序条件
        /// </summary>
        /// <param name="propertyName">排序属性名称</param>
        /// <param name="listSortDirection">排序方式</param>
        public PropertySortCondition(String propertyName, ListSortDirection listSortDirection)
        {
            PropertyName = propertyName;
            ListSortDirection = listSortDirection;
        }

        /// <summary>
        ///     获取或设置 排序属性名称
        /// </summary>
        public String PropertyName { get; set; }

        /// <summary>
        ///     获取或设置 排序方向
        /// </summary>
        public ListSortDirection ListSortDirection { get; set; }
    }
}
