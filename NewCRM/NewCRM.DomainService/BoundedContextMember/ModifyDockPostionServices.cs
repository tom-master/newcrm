using System;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entitys.Account;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    [Export(typeof(IModifyDockPostionServices))]
    internal sealed class ModifyDockPostionServices : BaseService.BaseService, IModifyDockPostionServices
    {
        public void ModifyDockPosition(Int32 accountId, Int32 defaultDeskNumber, String newPosition)
        {
            var accountResult = GetAccountInfoService(accountId);

            DockPostion dockPostion;

            if (Enum.TryParse(newPosition, true, out dockPostion))
            {
                if (dockPostion == DockPostion.None)
                {
                    var deskResult = QueryFactory.Create<Desk>().FindOne(SpecificationFactory.Create<Desk>(desk => desk.DeskNumber == accountResult.Config.DefaultDeskNumber));

                    var dockMembers = deskResult.Members.Where(member => member.IsOnDock).ToList();

                    if (dockMembers.Any())
                    {
                        dockMembers.ToList().ForEach(
                        f =>
                        {
                            f.OutDock();
                        });
                        Repository.Create<Desk>().Update(deskResult);
                    }

                    accountResult.Config.ModifyDockPostion(dockPostion);
                }
                else
                {
                    accountResult.Config.ModifyDockPostion(dockPostion);
                }
            }
            else
            {
                throw new BusinessException($"未识别出的码头位置:{newPosition}");
            }

            accountResult.Config.ModifyDefaultDesk(defaultDeskNumber);

            Repository.Create<Account>().Update(accountResult);

        }
    }
}
