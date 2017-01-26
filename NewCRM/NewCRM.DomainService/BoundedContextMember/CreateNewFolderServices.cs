using System;
using System.ComponentModel.Composition;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    [Export(typeof(ICreateNewFolderServices))]
    internal class CreateNewFolderServices : ICreateNewFolderServices
    {
        [Import]
        public BaseServiceContext BaseContext { get; set; }

        public void NewFolder(Int32 deskId, String folderName, String folderImg)
        {
            var newMember = new Member(folderName, folderImg, 0);

            var desk = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create((Desk d) => d.Id == deskId));

            BaseContext.Repository.Create<Desk>().Update(desk.AddMember(newMember));
        }
    }
}
