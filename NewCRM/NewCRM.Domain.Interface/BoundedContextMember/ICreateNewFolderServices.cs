using System;

namespace NewCRM.Domain.Interface.BoundedContextMember
{
    public interface ICreateNewFolderServices
    {
        void NewFolder(Int32 deskId, String folderName, String folderImg);
    }
}
