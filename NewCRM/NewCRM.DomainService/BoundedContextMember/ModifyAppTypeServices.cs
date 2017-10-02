using System;
using System.Linq;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;
using IModifyAppTypeServices = NewCRM.Domain.Services.Interface.IModifyAppTypeServices;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    internal sealed class ModifyAppTypeServices : BaseServiceContext, IModifyAppTypeServices
    {
  
        public void DeleteAppType(Int32 appTypeId)
        {
            var apps = DatabaseQuery.Find(FilterFactory.Create<App>(app => app.AppTypeId == appTypeId)).ToList();
            if (apps.Any())
            {
                apps.ForEach(app =>
                {
                    app.ModifyAppType(13);
                });
            }

            var internalAppType = DatabaseQuery.FindOne(FilterFactory.Create<AppType>(appType => appType.Id == appTypeId));

            internalAppType.Remove();

        }
    }
}
