using System;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.Repositories.IRepository.System;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    public class CreateNewFolderServices : BaseServiceContext, ICreateNewFolderServices
    {
        private readonly IDeskRepository _deskRepository;

        public CreateNewFolderServices(IDeskRepository deskRepository)
        {
            _deskRepository = deskRepository;
        }

        public void NewFolder(Int32 deskId, String folderName, String folderImg)
        {
            ValidateParameter.Validate(deskId).Validate(folderImg).Validate(folderName);

            var newMember = new Member(folderName, folderImg, 0);
            var desk = DatabaseQuery.FindOne(FilterFactory.Create((Desk d) => d.Id == deskId));
            _deskRepository.Update(desk.AddMember(newMember));
        }
    }
}
