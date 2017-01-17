using System;
using System.Linq;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Domain.Services.Service;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    internal sealed class ModifyAppTypeServices : BaseService, IModifyAppTypeServices
    {
        public void DeleteAppType(Int32 appTypeId)
        {

            var apps = Query.Find(FilterFactory.Create<App>(app => app.AppTypeId == appTypeId)).ToList();
            if (apps.Any())
            {
                apps.ForEach(app =>
                {
                    app.ModifyAppType(13);
                });
            }

            var internalAppType = Query.FindOne(FilterFactory.Create<AppType>(appType => appType.Id == appTypeId));

            internalAppType.Remove();

        }
    }
}
