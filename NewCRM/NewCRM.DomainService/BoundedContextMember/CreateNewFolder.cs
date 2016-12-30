using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Domain.Services.Service;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    [Export(typeof(ICreateNewFolder))]
    internal class CreateNewFolder : BaseService, ICreateNewFolder
    {
        public void NewFolder(Int32 deskId, String folderName, String folderImg)
        {
            var newMember = new Member(folderName, folderImg, 0);

            var desk = Query.FindOne(FilterFactory.Create((Desk d) => d.Id == deskId));

            Repository.Create<Desk>().Update(desk.AddMember(newMember));
        }
    }
}
