using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Application.Interface;
using NewCRM.Application.Services.Services;
using NewCRM.Domain.DomainSpecification;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Application.Services
{
    [Export(typeof(ISecurityApplicationServices))]
    internal class SecurityApplicationServices : BaseService, ISecurityApplicationServices
    {
        #region Role

        public List<RoleDto> GetAllRoles()
        {
            return Query.Find(FilterFactory.Create<Role>()).Select(role => new
            {
                role.Name,
                role.Id
            }).ConvertDynamicToDtos<RoleDto>().ToList();

        }

        public RoleDto GetRole(Int32 roleId)
        {
            ValidateParameter.Validate(roleId);

            var roleResult = Query.FindOne(FilterFactory.Create<Role>(role => role.Id == roleId));

            if (roleResult == null)
            {
                throw new BusinessException($"角色可能已被删除，请刷新后再试");
            }

            return DtoConfiguration.ConvertDynamicToDto<RoleDto>(new
            {
                roleResult.Name,
                roleResult.RoleIdentity,
                roleResult.Remark,
                Powers = roleResult.Powers.Select(s => new { Id = s.AppId})
            });

        }

        public List<RoleDto> GetAllRoles(String roleName, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(roleName).Validate(pageIndex).Validate(pageIndex);

            var roleSpecification = FilterFactory.Create<Role>();

            if ((roleName + "").Length > 0)
            {
                roleSpecification.And(role => role.Name.Contains(roleName));
            }

            return Query.PageBy(roleSpecification, pageIndex, pageSize, out totalCount).Select(s => new
            {
                s.Name,
                s.Id,
                s.RoleIdentity
            }).ConvertDynamicToDtos<RoleDto>().ToList();

        }


        public void RemoveRole(Int32 roleId)
        {
            ValidateParameter.Validate(roleId);

            var roleResult = Query.FindOne(FilterFactory.Create<Role>(role => role.Id == roleId));

            if (roleResult == null)
            {
                throw new BusinessException("该角色可能已不存在，请刷新后再试");
            }

            if (roleResult.Powers.Any())
            {
                roleResult.Powers.ToList().ForEach(rolePower => rolePower.Remove());
            }

            roleResult.Remove();

            Repository.Create<Role>().Update(roleResult);

            UnitOfWork.Commit();
        }

        public void AddNewRole(RoleDto roleDto)
        {
            ValidateParameter.Validate(roleDto);

            var role = roleDto.ConvertToModel<RoleDto, Role>();

            Repository.Create<Role>().Add(role);

            UnitOfWork.Commit();
        }

        public void ModifyRole(RoleDto roleDto)
        {
            ValidateParameter.Validate(roleDto);

            var role = roleDto.ConvertToModel<RoleDto, Role>();

            var roleResult = Query.FindOne(FilterFactory.Create<Role>(internalRole => internalRole.Id == role.Id));

            if (roleResult == null)
            {
                throw new BusinessException("该角色可能已被删除，请刷新后再试");
            }

            roleResult.ModifyRoleName(role.Name).ModifyRoleIdentity(role.RoleIdentity);

            Repository.Create<Role>().Update(roleResult);

            UnitOfWork.Commit();
        }

        public void AddPowerToCurrentRole(Int32 roleId, IEnumerable<Int32> powerIds)
        {
            ValidateParameter.Validate(roleId).Validate(powerIds);

            var roleResult = Query.FindOne(FilterFactory.Create<Role>(role => role.Id == roleId));

            if (roleResult == null)
            {
                throw new BusinessException("该角色可能已被删除，请刷新后再试");
            }

            roleResult.Powers.ToList().ForEach(f => f.Remove());

            roleResult.AddPower(powerIds.ToArray());

            Repository.Create<Role>().Update(roleResult);

            UnitOfWork.Commit();
        }

        #endregion
        
        public Boolean CheckPermissions(String powerName, params Int32[] roleIds)
        {
            //var powersIds = Query.Find(FilterFactory.Create<Power>(power => power.PowerIdentity == powerName)).Select(power => power.Id).ToArray();

            //var roles = Query.Find(FilterFactory.Create<Role>(role => roleIds.Contains(role.Id))).ToArray();

            //var isParentPermission = roles.Any(role => role.Powers.Any(power =>true/* power.Power.ParentId == null*/));

            //return isParentPermission || roles.Any(role => role.CheckPower(powersIds));

            return false;
        }
    }
}
