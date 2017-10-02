using System;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;
using ICreateNewFolderServices = NewCRM.Domain.Services.Interface.ICreateNewFolderServices;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    internal class CreateNewFolderServices : BaseServiceContext, ICreateNewFolderServices
    {
  
        public void NewFolder(Int32 deskId, String folderName, String folderImg)
        {
            var newMember = new Member(folderName, folderImg, 0);

            var desk = DatabaseQuery.FindOne(FilterFactory.Create((Desk d) => d.Id == deskId));

            Repository.Create<Desk>().Update(desk.AddMember(newMember));
        }
    }
}
