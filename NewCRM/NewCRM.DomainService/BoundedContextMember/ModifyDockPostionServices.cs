using System;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Domain.Services.Service;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    [Export(typeof(IModifyDockPostionServices))]
    internal sealed class ModifyDockPostionServices : BaseService, IModifyDockPostionServices
    {
        [Import(typeof(Func<Int32, Account>))]
        public Func<Int32, Account> Func { get; set; }


        public void ModifyDockPosition(Int32 accountId, Int32 defaultDeskNumber, String newPosition)
        {
            var accountResult = Func(accountId);

            DockPostion dockPostion;

            if (Enum.TryParse(newPosition, true, out dockPostion))
            {
                if (dockPostion == DockPostion.None)
                {
                    var deskResult = Query.FindOne(FilterFactory.Create<Desk>(desk => desk.DeskNumber == accountResult.Config.DefaultDeskNumber));

                    var dockMembers = deskResult.Members.Where(member => member.IsOnDock).ToList();

                    if (dockMembers.Any())
                    {
                        dockMembers.ForEach(f =>
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
