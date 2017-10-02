using System;

namespace NewCRM.Domain.Services.Interface
{
    public interface ICreateNewFolderServices
    {
        void NewFolder(Int32 deskId, String folderName, String folderImg);
    }
}
