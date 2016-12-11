using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Domain.Services.Service;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    [Export(typeof(IModifyAppTypeServices))]
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
