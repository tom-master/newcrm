using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entitys.System;

namespace NewCRM.Domain.Interface.BoundedContextMember
{
    public interface ICreateNewFolderServices
    {
        void NewFolder(Int32 deskId, String folderName, String folderImg);
    }
}
