using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NewCRM.Domain.Entitys;


namespace NewCRM.Repository.UnitOfWorkProvide
{
    /// <summary>
    /// 数据库上下文扩展
    /// </summary>
    public static class UnitOfWorkContextExtensions
    {
        internal static void Update<T, TKey>(this DbContext dbContext, params T[] entities) where T : DomainModelBase
        {
            if (dbContext == null) throw new ArgumentNullException($"{nameof(dbContext)}");
            if (entities == null) throw new ArgumentNullException($"{nameof(entities)}");

            foreach (T entity in entities)
            {
                DbSet<T> dbSet = dbContext.Set<T>();
                try
                {
                    DbEntityEntry<T> entry = dbContext.Entry(entity);
                    if (entry.State == EntityState.Detached)
                    {
                        dbSet.Attach(entity);
                        entry.State = EntityState.Modified;
                    }
                }
                catch (InvalidOperationException)
                {
                    T oldEntity = dbSet.Find(entity.Id);
                    dbContext.Entry(oldEntity).CurrentValues.SetValues(entity);
                }
            }
        }

        internal static void Update<T, TKey>(this DbContext dbContext, Expression<Func<T, Object>> propertyExpression, params T[] entities)
            where T : DomainModelBase
        {
            if (propertyExpression == null) throw new ArgumentNullException($"{nameof(propertyExpression)}");
            if (entities == null) throw new ArgumentNullException($"{nameof(entities)}");
            ReadOnlyCollection<MemberInfo> memberInfos = ((dynamic)propertyExpression.Body).Members;
            foreach (T entity in entities)
            {
                DbSet<T> dbSet = dbContext.Set<T>();
                try
                {
                    DbEntityEntry<T> entry = dbContext.Entry(entity);
                    entry.State = EntityState.Unchanged;
                    foreach (var memberInfo in memberInfos)
                    {
                        entry.Property(memberInfo.Name).IsModified = true;
                    }
                }
                catch (InvalidOperationException)
                {
                    T originalEntity = dbSet.Local.Single(m => Equals(m.Id, entity.Id));
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

        internal static Int32 SaveChanges(this DbContext dbContext, Boolean validateOnSaveEnabled)
        {
            Boolean isReturn = dbContext.Configuration.ValidateOnSaveEnabled != validateOnSaveEnabled;
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