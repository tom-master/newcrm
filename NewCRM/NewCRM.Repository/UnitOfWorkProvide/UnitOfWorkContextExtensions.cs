﻿using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using NewCRM.Domain.Entitys;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Repository.UnitOfWorkProvide
{
    /// <summary>
    /// 数据库上下文扩展
    /// </summary>
    public static class UnitOfWorkContextExtensions
    {
        internal static void Update<T, TKey>(this DbContext dbContext, params T[] entities) where T : DomainModelBase
        {
            if (dbContext == null)
            {
                throw new BusinessException($"{nameof(dbContext)}：不能为空");
            }

            if (entities == null)
            {
                throw new BusinessException($"{nameof(entities)}：不能为空");
            }

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



        internal static void Update<T, TKey>(this DbContext dbContext, T entity, params Expression<Func<T, Object>>[] propertyExpressions)
            where T : DomainModelBase
        {
            if (propertyExpressions == null)
            {
                throw new BusinessException($"{nameof(propertyExpressions)}：不能为空");
            }

            if (entity == null)
            {
                throw new BusinessException($"{nameof(entity)}：不能为空");
            }

            //var dbEntityEntry = dbContext.Entry(entity);
            //if (propertyExpressions.Any())
            //{
            //    foreach (var property in propertyExpressions)
            //    {
            //        dbEntityEntry.Property(property).IsModified = true;
            //    }
            //}
            //else
            //{
            //    foreach (var property in dbEntityEntry.OriginalValues.PropertyNames)
            //    {
            //        var original = dbEntityEntry.OriginalValues.GetValue<Object>(property);
            //        var current = dbEntityEntry.CurrentValues.GetValue<Object>(property);
            //        if (original != null && !original.Equals(current))
            //        {
            //            dbEntityEntry.Property(property).IsModified = true;
            //        }
            //    }
            //}

            //ReadOnlyCollection<MemberInfo> memberInfos = ((dynamic)propertyExpression.Body).Members;

            //foreach (T entity in entities)
            //{
            //    DbSet<T> dbSet = dbContext.Set<T>();
            //    try
            //    {
            //        DbEntityEntry<T> entry = dbContext.Entry(entity);
            //        entry.State = EntityState.Unchanged;
            //        foreach (var memberInfo in memberInfos)
            //        {
            //            entry.Property(memberInfo.Name).IsModified = true;

            //        }
            //    }
            //    catch (InvalidOperationException)
            //    {
            //        T originalEntity = dbSet.Local.Single(m => Equals(m.Id, entity.Id));
            //        ObjectContext objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
            //        ObjectStateEntry objectEntry = objectContext.ObjectStateManager.GetObjectStateEntry(originalEntity);
            //        objectEntry.ApplyCurrentValues(entity);
            //        objectEntry.ChangeState(EntityState.Unchanged);
            //        foreach (var memberInfo in memberInfos)
            //        {
            //            objectEntry.SetModifiedProperty(memberInfo.Name);
            //        }
            //    }
            //}
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