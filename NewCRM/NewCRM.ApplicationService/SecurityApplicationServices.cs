using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Application.Interface;
using NewCRM.Domain;
using NewCRM.Domain.DomainSpecification;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Application.Services
{
    [Export(typeof(ISecurityApplicationServices))]
    internal class SecurityApplicationServices : ISecurityApplicationServices
    {

        [Import]
        public BaseServiceContext BaseContext { get; set; }

        #region Role

        public List<RoleDto> GetAllRoles()
        {
            return BaseContext.Query.Find(BaseContext.FilterFactory.Create<Role>()).Select(role => new
            {
                role.Name,
                role.Id
            }).ConvertDynamicToDtos<RoleDto>().ToList();

        }

        public RoleDto GetRole(Int32 roleId)
        {
            BaseContext.ValidateParameter.Validate(roleId);

            var roleResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create<Role>(role => role.Id == roleId));

            if (roleResult == null)
            {
                throw new BusinessException($"角色可能已被删除，请刷新后再试");
            }

            return DtoConfiguration.ConvertDynamicToDto<RoleDto>(new
            {
                roleResult.Name,
                roleResult.RoleIdentity,
                roleResult.Remark,
                Powers = roleResult.Powers.Select(s => new { Id = s.AppId })
            });

        }

        public List<RoleDto> GetAllRoles(String roleName, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            BaseContext.ValidateParameter.Validate(roleName).Validate(pageIndex).Validate(pageIndex);

            var roleSpecification = BaseContext.FilterFactory.Create<Role>();

            if ((roleName + "").Length > 0)
            {
                roleSpecification.And(role => role.Name.Contains(roleName));
            }

            return BaseContext.Query.PageBy(roleSpecification, pageIndex, pageSize, out totalCount).Select(s => new
            {
                s.Name,
                s.Id,
                s.RoleIdentity
            }).ConvertDynamicToDtos<RoleDto>().ToList();

        }


        public void RemoveRole(Int32 roleId)
        {
            BaseContext.ValidateParameter.Validate(roleId);

            var roleResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create<Role>(role => role.Id == roleId));

            if (roleResult == null)
            {
                throw new BusinessException("该角色可能已不存在，请刷新后再试");
            }

            if (roleResult.Powers.Any())
            {
                roleResult.Powers.ToList().ForEach(rolePower => rolePower.Remove());
            }

            roleResult.Remove();

            BaseContext.Repository.Create<Role>().Update(roleResult);

            BaseContext.UnitOfWork.Commit();
        }

        public void AddNewRole(RoleDto roleDto)
        {
            BaseContext.ValidateParameter.Validate(roleDto);

            var role = roleDto.ConvertToModel<RoleDto, Role>();

            BaseContext.Repository.Create<Role>().Add(role);

            BaseContext.UnitOfWork.Commit();
        }

        public void ModifyRole(RoleDto roleDto)
        {
            BaseContext.ValidateParameter.Validate(roleDto);

            var role = roleDto.ConvertToModel<RoleDto, Role>();

            var roleResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create<Role>(internalRole => internalRole.Id == role.Id));

            if (roleResult == null)
            {
                throw new BusinessException("该角色可能已被删除，请刷新后再试");
            }

            roleResult.ModifyRoleName(role.Name).ModifyRoleIdentity(role.RoleIdentity);

            BaseContext.Repository.Create<Role>().Update(roleResult);

            BaseContext.UnitOfWork.Commit();
        }

        public void AddPowerToCurrentRole(Int32 roleId, IEnumerable<Int32> powerIds)
        {
            BaseContext.ValidateParameter.Validate(roleId).Validate(powerIds);

            var roleResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create<Role>(role => role.Id == roleId));

            if (roleResult == null)
            {
                throw new BusinessException("该角色可能已被删除，请刷新后再试");
            }

            roleResult.Powers.ToList().ForEach(f => f.Remove());

            roleResult.AddPower(powerIds.ToArray());

            BaseContext.Repository.Create<Role>().Update(roleResult);

            BaseContext.UnitOfWork.Commit();
        }

        #endregion

        public Boolean CheckPermissions(Int32 accessAppId, params Int32[] roleIds)
        {
            var roles = BaseContext.Query.Find(BaseContext.FilterFactory.Create<Role>(role => roleIds.Contains(role.Id))).ToArray();

            return roles.Any(role => role.CheckPower(accessAppId));

        }
    }
}
