using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.Entities.DomainModel;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.Repositories;
using NewCRM.Domain.Entities.UnitWork;
using NewCRM.Infrastructure.CommonTools.CustemException;
using NewCRM.Infrastructure.CommonTools.CustomHelper;
using NewCRM.Repository.UnitOfWorkProvide;

namespace NewCRM.Repository.RepositoryProvide
{
    /// <summary>
    ///     EntityFramework仓储操作基类
    /// </summary>
    /// <typeparam name="TEntity">动态实体类型</typeparam>
    public abstract class EntityFrameworkRepository<TEntity> : IRepository<TEntity> where TEntity : DomainModelBase, IAggregationRoot
    {
        public virtual Parameter VaildateParameter => new Parameter();

        #region 获取 当前实体的查询数据集

        /// <summary>
        ///  获取 当前实体的查询数据集
        /// </summary>
        public virtual IQueryable<TEntity> Entities => EfContext.Set<TEntity, Int32>();

        #endregion

        #region 仓储上下文的实例

        /// <summary>
        ///     获取 仓储上下文的实例
        /// </summary>
        [Import]
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
                throw new RepositoryException($"无法获取当前工作单元的实例:{UnitOfWork}");
            }
        }




        #endregion

        #region 公共方法

        /// <summary>
        ///     插入实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        public virtual void Add(TEntity entity, Boolean isSave = true)
        {
            VaildateParameter.Validate(entity);
            EfContext.RegisterNew<TEntity, Int32>(entity);

            if (isSave)
            {
                EfContext.Commit();
            }
            else
            {
                throw new RepositoryException("数据添加失败");
            }
        }

        /// <summary>
        ///     批量插入实体记录集合
        /// </summary>
        /// <param name="entities"> 实体记录集合 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        public virtual void Add(IEnumerable<TEntity> entities, Boolean isSave = true)
        {
            VaildateParameter.Validate(entities);
            EfContext.RegisterNew<TEntity, Int32>(entities);

            if (isSave)
            {
                EfContext.Commit();
            }
            else
            {
                throw new RepositoryException("数据添加失败");
            }
        }

        /// <summary>
        ///     删除指定编号的记录
        /// </summary>
        /// <param name="id"> 实体记录编号 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        public virtual void Remove(Int32 id, Boolean isSave = true)
        {
            VaildateParameter.Validate(id);
            TEntity entity = EfContext.Set<TEntity, Int32>().Find(id);
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
        public virtual void Remove(TEntity entity, Boolean isSave = true)
        {
            VaildateParameter.Validate(entity);
            EfContext.RegisterDeleted<TEntity, Int32>(entity);
            if (isSave)
            {
                EfContext.Commit();
            }
            else
            {
                throw new RepositoryException("数据移除失败");
            }
        }

        /// <summary>
        ///     删除实体记录集合
        /// </summary>
        /// <param name="entities"> 实体记录集合 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        public virtual void Remove(IEnumerable<TEntity> entities, Boolean isSave = true)
        {
            VaildateParameter.Validate(entities);
            EfContext.RegisterDeleted<TEntity, Int32>(entities);
            if (isSave)
            {
                EfContext.Commit();
            }
            else
            {
                throw new RepositoryException("数据添加失败");
            }
        }

        /// <summary>
        ///     删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="predicate"> 查询条件谓语表达式 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        public virtual void Remove(Expression<Func<TEntity, Boolean>> predicate, Boolean isSave = true)
        {
            VaildateParameter.Validate(predicate);
            IList<TEntity> entities = EfContext.Set<TEntity, Int32>().Where(predicate).ToList();
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
        public virtual void Update(TEntity entity, Boolean isSave = true)
        {
            VaildateParameter.Validate(entity);
            EfContext.RegisterModified<TEntity, Int32>(entity);
            if (isSave)
            {
                EfContext.Commit();
            }
            else
            {
                throw new RepositoryException("数据更新失败");
            }
        }

        /// <summary>
        /// 使用附带新值的实体信息更新指定实体属性的值
        /// </summary>
        /// <param name="propertyExpression">属性表达式</param>
        /// <param name="isSave">是否执行保存</param>
        /// <param name="entity">附带新值的实体信息，必须包含主键</param>
        public virtual void Update(Expression<Func<TEntity, Boolean>> propertyExpression, TEntity entity, Boolean isSave = true)
        {
            throw new NotSupportedException("上下文公用，不支持按需更新功能。");
        }
        #endregion
    }
}