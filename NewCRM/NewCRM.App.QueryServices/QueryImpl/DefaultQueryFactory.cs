using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using NewCRM.Domain.Entities.DomainModel;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainModel.Security;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.QueryServices.DomainSpecification;
using NewCRM.QueryServices.Query;
using NewCRM.QueryServices.QueryImpl.ConcreteQuery;

namespace NewCRM.QueryServices.QueryImpl
{
    [Export(typeof(QueryFactory))]
    public sealed class DefaultQueryFactory : QueryFactory
    {
        public override IQuery<T> CreateQuery<T>()
        {
            var tInstance = new T();

            if (tInstance is Account)
            {
                return new AccountQuery() as IQuery<T>;
            }

            if (tInstance is App)
            {
                return new AppQuery() as IQuery<T>;
            }

            if (tInstance is AppType)
            {
                return new AppTypeQuery() as IQuery<T>;
            }

            if (tInstance is Power)
            {
                return new PowerQuery() as IQuery<T>;
            }

            if (tInstance is Role)
            {
                return new RoleQuery() as IQuery<T>;
            }

            if (tInstance is Wallpaper)
            {
                return new WallpaperQuery() as IQuery<T>;
            }

            return null;
        }
    }
}
