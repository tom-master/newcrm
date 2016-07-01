using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NewCRM.Domain.DomainModel;


namespace NewCRM.Repository.UnitOfWorkProvide
{
    /// <summary>
    ///     数据单元操作类
    /// </summary>

    public class EfUnitOfWorkContext : UnitOfWorkContextBase
    {
        private static readonly EfUnitOfWorkContext _efUnitOfWorkContext = new EfUnitOfWorkContext();

        private EfUnitOfWorkContext()
        {

        }

        public static UnitOfWorkContextBase GetUnitOfWorkContext => _efUnitOfWorkContext;


        /// <summary>
        /// 获取当前使用的数据访问上下文对象
        /// </summary>
        protected override DbContext Context => DbContextFactory.Context;

    }

    internal static class DbContextFactory
    {
        private static readonly NewCrmBackSite _newCrmBackSite = new NewCrmBackSite();

        internal static DbContext Context => _newCrmBackSite ?? new NewCrmBackSite();
    }


    /// <summary>
    /// 数据库上下文扩展
    /// </summary>
    internal static class UnitOfWorkContextExtensions
    {
        internal static void Update<TEntity, TKey>(this DbContext dbContext, params TEntity[] entities) where TEntity : DomainModelBase
        {
            if (dbContext == null) throw new ArgumentNullException($"{nameof(dbContext)}");
            if (entities == null) throw new ArgumentNullException($"{nameof(entities)}");

            foreach (TEntity entity in entities)
            {
                DbSet<TEntity> dbSet = dbContext.Set<TEntity>();
                try
                {
                    DbEntityEntry<TEntity> entry = dbContext.Entry(entity);
                    if (entry.State == EntityState.Detached)
                    {
                        dbSet.Attach(entity);
                        entry.State = EntityState.Modified;
                    }
                }
                catch (InvalidOperationException)
                {
                    TEntity oldEntity = dbSet.Find(entity.Id);
                    dbContext.Entry(oldEntity).CurrentValues.SetValues(entity);
                }
            }
        }

        internal static void Update<TEntity, TKey>(this DbContext dbContext, Expression<Func<TEntity, object>> propertyExpression, params TEntity[] entities)
            where TEntity : DomainModelBase
        {
            if (propertyExpression == null) throw new ArgumentNullException($"{nameof(propertyExpression)}");
            if (entities == null) throw new ArgumentNullException($"{nameof(entities)}");
            ReadOnlyCollection<MemberInfo> memberInfos = ((dynamic)propertyExpression.Body).Members;
            foreach (TEntity entity in entities)
            {
                DbSet<TEntity> dbSet = dbContext.Set<TEntity>();
                try
                {
                    DbEntityEntry<TEntity> entry = dbContext.Entry(entity);
                    entry.State = EntityState.Unchanged;
                    foreach (var memberInfo in memberInfos)
                    {
                        entry.Property(memberInfo.Name).IsModified = true;
                    }
                }
                catch (InvalidOperationException)
                {
                    TEntity originalEntity = dbSet.Local.Single(m => Equals(m.Id, entity.Id));
                    ObjectContext objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
                    ObjectStateEntry objectEntry = objectContext.ObjectStateManager.GetObjectStateEntry(originalEntity);
                    objectEntry.ApplyCurrentValues(entity);
                    objectEntry.ChangeState(EntityState.Unchanged);
                    foreach (var memberInfo in memberInfos)
                    {
                        objectEntry.SetModifiedProperty(memberInfo.Name);
                    }
                }
            }
        }

        internal static int SaveChanges(this DbContext dbContext, bool validateOnSaveEnabled)
        {
            bool isReturn = dbContext.Configuration.ValidateOnSaveEnabled != validateOnSaveEnabled;
            try
            {
                dbContext.Configuration.ValidateOnSaveEnabled = validateOnSaveEnabled;
                return dbContext.SaveChanges();
            }
            finally
            {
                if (isReturn)
                {
                    dbContext.Configuration.ValidateOnSaveEnabled = !validateOnSaveEnabled;
                }
            }
        }
    }
}