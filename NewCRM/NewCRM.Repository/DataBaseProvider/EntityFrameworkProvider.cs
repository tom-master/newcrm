﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.Entities.DomainModel;
using NewCRM.Domain.Entities.Repositories;
using NewCRM.Domain.Entities.UnitWork;
using NewCRM.Infrastructure.CommonTools.CustemException;
using NewCRM.Infrastructure.CommonTools.CustomHelper;
using NewCRM.Repository.UnitOfWorkProvide;

namespace NewCRM.Repository.DataBaseProvider
{
    /// <summary>
    /// EntityFramework仓储操作基类
    /// </summary>
    /// <typeparam name="T">动态实体类型</typeparam>
    public abstract class EntityFrameworkProvider<T> : IRepository<T> where T : DomainModelBase, IAggregationRoot
    {
        public virtual Parameter VaildateParameter => new Parameter();


        #region 仓储上下文的实例

        /// <summary>
        /// 获取 仓储上下文的实例
        /// </summary>

        [Import("NewCRM.Domain.Entities.UnitWork")]
        public IUnitOfWork UnitOfWork { get; set; }

        #endregion

        #region 属性

        /// <summary>
        ///     获取 EntityFramework的数据仓储上下文
        /// </summary>
        protected UnitOfWorkContextBase EfContext
        {
            get
            {
                if (UnitOfWork is UnitOfWorkContextBase)
                {
                    return UnitOfWork as UnitOfWorkContextBase;
                }
                throw new RepositoryException($"无法获取当前工作单元的实例:{nameof(UnitOfWork)}");
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        ///     插入实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        public virtual void Add(T entity, Boolean isSave = true)
        {
            VaildateParameter.Validate(entity);
            EfContext.RegisterNew<T, Int32>(entity);
        }

        /// <summary>
        ///     批量插入实体记录集合
        /// </summary>
        /// <param name="entities"> 实体记录集合 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        public virtual void Add(IEnumerable<T> entities, Boolean isSave = true)
        {
            VaildateParameter.Validate(entities);
            EfContext.RegisterNew<T, Int32>(entities);
        }

        /// <summary>
        ///     删除指定编号的记录
        /// </summary>
        /// <param name="id"> 实体记录编号 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        public virtual void Remove(Int32 id, Boolean isSave = true)
        {
            VaildateParameter.Validate(id);
            T entity = EfContext.Set<T, Int32>().Find(id);
            if (entity != null)
            {
                Remove(entity, isSave);
            }
            else
            {
                throw new RepositoryException("数据移除失败");
            }
        }

        /// <summary>
        ///     删除实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        public virtual void Remove(T entity, Boolean isSave = true)
        {
            VaildateParameter.Validate(entity);
            EfContext.RegisterDeleted<T, Int32>(entity);
        }

        /// <summary>
        ///     删除实体记录集合
        /// </summary>
        /// <param name="entities"> 实体记录集合 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        public virtual void Remove(IEnumerable<T> entities, Boolean isSave = true)
        {
            VaildateParameter.Validate(entities);
            EfContext.RegisterDeleted<T, Int32>(entities);
        }

        /// <summary>
        ///     删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="predicate"> 查询条件谓语表达式 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        public virtual void Remove(Expression<Func<T, Boolean>> predicate, Boolean isSave = true)
        {
            VaildateParameter.Validate(predicate);
            IList<T> entities = EfContext.Set<T, Int32>().Where(predicate).ToList();
            if (entities.Any())
            {
                Remove(entities, isSave);
            }
            else
            {
                throw new RepositoryException("数据移除失败");
            }
        }

        /// <summary>
        ///     更新实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        public virtual void Update(T entity, Boolean isSave = true)
        {
            VaildateParameter.Validate(entity);
            EfContext.RegisterModified<T, Int32>(entity);
        }

        /// <summary>
        /// 使用附带新值的实体信息更新指定实体属性的值
        /// </summary>
        /// <param name="propertyExpression">属性表达式</param>
        /// <param name="isSave">是否执行保存</param>
        /// <param name="entity">附带新值的实体信息，必须包含主键</param>
        public virtual void Update(Expression<Func<T, Boolean>> propertyExpression, T entity, Boolean isSave = true)
        {
            throw new NotSupportedException("上下文公用，不支持按需更新功能。");
        }
        #endregion
    }
}