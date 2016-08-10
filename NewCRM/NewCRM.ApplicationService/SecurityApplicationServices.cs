using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Dto;
using NewCRM.Dto.Dto;

namespace NewCRM.Application.Services
{
    [Export(typeof(ISecurityApplicationServices))]
    internal sealed class SecurityApplicationServices : BaseApplicationServices, ISecurityApplicationServices
    {
        public List<RoleDto> GetAllRoles(String roleName, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(roleName).Validate(pageIndex).Validate(pageIndex);

            return SecurityServices.GetAllRoles(roleName, pageIndex, pageSize, out totalCount).ConvertDynamicToDtos<RoleDto>().ToList();
        }

        public void RemoveRole(Int32 roleId)
        {
            ValidateParameter.Validate(roleId);

            SecurityServices.RemoveRole(roleId);
        }
    }
}
