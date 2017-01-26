using System; 
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    [Export(typeof(IModifyAppTypeServices))]
    internal sealed class ModifyAppTypeServices : IModifyAppTypeServices
    {

        [Import]
        public BaseServiceContext BaseContext { get; set; }

        public void DeleteAppType(Int32 appTypeId)
        {

            var apps = BaseContext.Query.Find(BaseContext.FilterFactory.Create<App>(app => app.AppTypeId == appTypeId)).ToList();
            if (apps.Any())
            {
                apps.ForEach(app =>
                {
                    app.ModifyAppType(13);
                });
            }

            var internalAppType = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create<AppType>(appType => appType.Id == appTypeId));

            internalAppType.Remove();

        }
    }
}
