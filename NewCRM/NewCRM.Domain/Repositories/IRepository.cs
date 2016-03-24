using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainModel;
namespace NewCRM.Domain.Repositories
{
    /// <summary>
    ///     定义仓储模型中的数据标准操作
    /// </summary>
    /// <typeparam name="T">动态实体类型</typeparam>
    public interface IRepository<T> where T : EntityBase<Int32>
    {
        #region 属性
        /// <summary>
        ///     获取 当前实体的查询数据集
        /// </summary>
        IQueryable<T> Entities { get; }
        #endregion

        #region 公共方法

        /// <summary>
        ///     插入实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>

        void Add(T entity, Boolean isSave = true);

        /// <summary>
        ///     批量插入实体记录集合
        /// </summary>
        /// <param name="entities"> 实体记录集合 </param>
        /// <param name="isSave"> 是否执行保存 </param>

        void Add(IEnumerable<T> entities, Boolean isSave = true);

        /// <summary>
        ///     删除指定编号的记录
        /// </summary>
        /// <param name="id"> 实体记录编号 </param>
        /// <param name="isSave"> 是否执行保存 </param>

        void Remove(Int32 id, Boolean isSave = true);

        /// <summary>
        ///     删除实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>

        void Remove(T entity, Boolean isSave = true);

        /// <summary>
        ///     删除实体记录集合
        /// </summary>
        /// <param name="entities"> 实体记录集合 </param>
        /// <param name="isSave"> 是否执行保存 </param>

        void Remove(IEnumerable<T> entities, Boolean isSave = true);

        /// <summary>
        ///     删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="predicate"> 查询条件谓语表达式 </param>
        /// <param name="isSave"> 是否执行保存 </param>

        void Remove(Expression<Func<T, Boolean>> predicate, Boolean isSave = true);

        /// <summary>
        ///  更新实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>

        void Update(T entity, Boolean isSave = true);

        /// <summary>
        /// 使用附带新值的实体信息更新指定实体属性的值
        /// </summary>
        /// <param name="propertyExpression">属性表达式</param>
        /// <param name="isSave">是否执行保存</param>
        /// <param name="entity">附带新值的实体信息，必须包含主键</param>
        /// <returns>操作影响的行数</returns>
        void Update(Expression<Func<T, dynamic>> propertyExpression, T entity, Boolean isSave = true);

        #endregion
    }
}