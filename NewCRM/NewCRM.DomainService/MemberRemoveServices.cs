using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Domain.Interface;

namespace NewCRM.Domain.Services
{
    [Export(typeof(IMemberRemoveServices))]
    internal sealed class MemberRemoveServices : BaseService.BaseService, IMemberRemoveServices
    {

        public void RemoveMember(Int32 accountId, Int32 memberId)
        {
            var accountConfig = GetAccountInfoService(accountId).Config;

            App appResult = null;

            foreach (var desk in accountConfig.Desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);

                if (memberResult != null)
                {
                    if (memberResult.MemberType == MemberType.Folder)
                    {
                        //移除文件夹中的内容
                        foreach (var desk1 in accountConfig.Desks)
                        {
                            desk1.Members.Where(d => d.FolderId == memberId).ToList().ForEach(m => m.Remove());
                        }
                    }
                    else
                    {
                        appResult = QueryFactory.Create<App>().FindOne(SpecificationFactory.Create<App>(app => app.Id == memberResult.AppId));

                        appResult.SubtractUseCount();

                        appResult.SubtractStar(accountId);

                    }

                    memberResult.Remove();

                    Repository.Create<Desk>().Update(desk);

                    if (appResult != null)
                    {
                        Repository.Create<App>().Update(appResult);
                    }
                    break;
                }
            }

        }

    }
}
