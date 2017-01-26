using System;
using System.Linq;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    [Export(typeof(IModifyDockPostionServices))]
    internal sealed class ModifyDockPostionServices : IModifyDockPostionServices
    {

        [Import]
        public BaseServiceContext BaseContext { get; set; }

        public void ModifyDockPosition(Int32 defaultDeskNumber, String newPosition)
        {
            var accountResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create((Account account) => account.Id == BaseContext.GetAccountId()));

            DockPostion dockPostion;

            if (Enum.TryParse(newPosition, true, out dockPostion))
            {
                if (dockPostion == DockPostion.None)
                {
                    var deskResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create<Desk>(desk => desk.DeskNumber == accountResult.Config.DefaultDeskNumber));

                    var dockMembers = deskResult.Members.Where(member => member.IsOnDock).ToList();

                    if (dockMembers.Any())
                    {
                        dockMembers.ForEach(f =>
                        {
                            f.OutDock();
                        });

                        BaseContext.Repository.Create<Desk>().Update(deskResult);
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

            BaseContext.Repository.Create<Account>().Update(accountResult);

        }
    }
}
