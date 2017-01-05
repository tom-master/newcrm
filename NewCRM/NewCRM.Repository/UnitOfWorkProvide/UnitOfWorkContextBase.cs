using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Threading;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.UnitWork;
using NewCRM.Repository.DataBaseProvider.EF.Event;
using NewCRM.Repository.DataBaseProvider.Redis;
using Newtonsoft.Json;

namespace NewCRM.Repository.UnitOfWorkProvide
{
    /// <summary>
    ///     单元操作实现基类
    /// </summary>
    public abstract class UnitOfWorkContextBase : IUnitOfWorkContext, ICacheCreateEvent, ICacheModifyEvent
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCacheCreate;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCacheModify;

        /// <summary>
        /// 获取 当前使用的数据访问上下文对象
        /// </summary>
        protected abstract DbContext Context { get; }

        /// <summary>
        ///     获取 当前单元操作是否已被提交
        /// </summary>
        public Boolean IsCommitted { get; private set; }

        /// <summary>
        /// 提交当前单元操作的结果
        /// </summary>
        /// <param name="validateOnSaveEnabled">保存时是否自动验证跟踪实体</param>
        /// <returns></returns>
        public Int32 Commit(Boolean validateOnSaveEnabled = true)
        {
            if (IsCommitted)
            {
                return 0;
            }

            try
            {

                Int32 result = Context.SaveChanges(validateOnSaveEnabled);

                IsCommitted = true;

                CacheCreate();

                CacheModify();

                return result;
            }
            catch (DbUpdateException)
            {
                Rollback();

                throw;
            }
        }

        /// <summary>
        ///     把当前单元操作回滚成未提交状态
        /// </summary>
        public void Rollback()
        {
            IsCommitted = false;
        }

        public void Dispose()
        {
            if (!IsCommitted)
            {
                Commit();
            }
            Context.Dispose();
        }

        /// <summary>
        ///   为指定的类型返回 System.Data.Entity.DbSet，这将允许对上下文中的给定实体执行 CRUD 操作。
        /// </summary>
        /// <typeparam name="T"> 应为其返回一个集的实体类型。 </typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <returns> 给定实体类型的 System.Data.Entity.DbSet 实例。 </returns>
        public DbSet<T> Set<T, TKey>() where T : DomainModelBase
        {
            return Context.Set<T>();
        }

        /// <summary>
        ///     注册一个新的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="T"> 要注册的类型 </typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <param name="entity"> 要注册的对象 </param>
        public void RegisterNew<T, TKey>(T entity) where T : DomainModelBase
        {
            EntityState state = Context.Entry(entity).State;

            if (state == EntityState.Detached)
            {
                Context.Entry(entity).State = EntityState.Added;
            }

            IsCommitted = false;

        }

        /// <summary>
        ///     批量注册多个新的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="T"> 要注册的类型 </typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <param name="entities"> 要注册的对象集合 </param>
        public void RegisterNew<T, TKey>(IEnumerable<T> entities) where T : DomainModelBase
        {
            try
            {
                Context.Configuration.AutoDetectChangesEnabled = false;
                foreach (T entity in entities)
                {
                    RegisterNew<T, TKey>(entity);
                }

            }
            finally
            {
                Context.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        /// <summary>
        ///     注册一个更改的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="T"> 要注册的类型 </typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <param name="entity"> 要注册的对象 </param>
        public void RegisterModified<T, TKey>(T entity) where T : DomainModelBase
        {
            Context.Update<T, TKey>(entity);

            IsCommitted = false;

            entity.LastModifyTime = DateTime.Parse($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        }

        /// <summary>
        /// 使用指定的属性表达式指定注册更改的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="T">要注册的类型</typeparam>
        /// <typeparam name="TKey">主键类型</typeparam>
        /// <param name="propertyExpression">属性表达式，包含要更新的实体属性</param>
        /// <param name="entity">附带新值的实体信息，必须包含主键</param>
        public void RegisterModified<T, TKey>(Expression<Func<T, Object>> propertyExpression, T entity) where T : DomainModelBase
        {
            Context.Update<T, TKey>(propertyExpression, entity);
            IsCommitted = false;
        }

        /// <summary>
        ///   注册一个删除的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="T"> 要注册的类型 </typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <param name="entity"> 要注册的对象 </param>
        public void RegisterDeleted<T, TKey>(T entity) where T : DomainModelBase
        {
            Context.Entry(entity).State = EntityState.Deleted;
            IsCommitted = false;
        }

        /// <summary>
        ///   批量注册多个删除的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="T"> 要注册的类型 </typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <param name="entities"> 要注册的对象集合 </param>
        public void RegisterDeleted<T, TKey>(IEnumerable<T> entities) where T : DomainModelBase
        {
            try
            {
                Context.Configuration.AutoDetectChangesEnabled = false;
                foreach (T entity in entities)
                {
                    RegisterDeleted<T, TKey>(entity);
                }
            }
            finally
            {
                Context.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        public void CacheCreate()
        {
            var temp = Interlocked.CompareExchange(ref OnCacheCreate, null, null);

            if (temp != null)
            {
                temp(this, null);
            }
        }

        public void CacheModify()
        {
            var temp = Interlocked.CompareExchange(ref OnCacheModify, null, null);

            if (temp != null)
            {
                temp(this, null);
            }
        }



        //private void SetCache<T>(T entity) where T : DomainModelBase
        //{
        //    _cacheQueryProvider.StringSet($"NewCRM:{typeof(T).Name}:{entity.Id}", JsonConvert.SerializeObject(entity, Formatting.Indented, new JsonSerializerSettings
        //    {
        //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //    }));
        //}
    }
}