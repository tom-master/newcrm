using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Domain.Entitys.System;
using NewCRM.Dto.Dto;
using NewCRM.Dto.MapperProfile;

namespace NewCRM.Dto
{
    public static class DtoConfiguration
    {
        static DtoConfiguration()
        {
            Mapper.Initialize(d =>
            {
                //Account
                d.CreateMap<Account, AccountDto>();
                d.AddProfile<AccountToAccountDtoProfile>();

                d.CreateMap<AccountDto, Account>();
                d.AddProfile<AccountDtoToAccountProfile>();

                d.CreateMap<RoleDto, AccountRole>();
                d.AddProfile<RoleDtoToAccountRoleProfile>();

                d.CreateMap<AccountRole, RoleDto>();
                d.AddProfile<AccountRoleToRoleDtoProfile>();

                //Wallpaper
                d.CreateMap<Config, ConfigDto>();
                d.AddProfile<ConfigToConfigDtoProfile>();

                //Wallpaper
                d.CreateMap<Wallpaper, WallpaperDto>();
                d.AddProfile<WallpaperToWallpaperDtoProfile>();

                d.CreateMap<WallpaperDto, Wallpaper>();
                d.AddProfile<WallpaperDtoToWallpaperProfile>();

                //Member
                d.CreateMap<Member, MemberDto>();
                d.AddProfile<MemberToMemberDtoProfile>();

                //AppType
                d.CreateMap<AppType, AppTypeDto>();
                d.AddProfile<AppTypeToAppTypeDtoProfile>();

                d.CreateMap<AppTypeDto, AppType>();
                d.AddProfile<AppTypeDtoToAppTypeProfile>();

                //App
                d.CreateMap<App, AppDto>();
                d.AddProfile<AppToAppDtoProfile>();

                d.CreateMap<AppDto, App>();
                d.AddProfile<AppDtoToAppProfile>();

                //Role
                d.CreateMap<Role, RoleDto>();
                d.AddProfile<RoleToRoleDtoProfile>();

                d.CreateMap<RoleDto, Role>();
                d.AddProfile<RoleDtoToRoleProfile>();

                //Power
                d.CreateMap<Power, PowerDto>();
                d.AddProfile<PowerToPowerDtoProfile>();

                d.CreateMap<PowerDto, Power>();
                d.AddProfile<PowerDtoToPowerProfile>();

            });

            #region Member

            #region dto ->domain
            //Mapper.CreateMap<MemberDto, Member>()
            //.ForMember(member => member.Id, dto => dto.MapFrom(w => w.Id))
            //.ForMember(member => member.AppId, dto => dto.MapFrom(w => w.AppId))
            //.ForMember(member => member.Width, dto => dto.MapFrom(w => w.Width))
            //.ForMember(member => member.Height, dto => dto.MapFrom(w => w.Height))
            //.ForMember(member => member.FolderId, dto => dto.MapFrom(w => w.FolderId))
            //.ForMember(member => member.Name, dto => dto.MapFrom(w => w.Name))
            //.ForMember(member => member.IconUrl, dto => dto.MapFrom(w => w.IconUrl))
            //.ForMember(member => member.MemberType, dto => dto.MapFrom(w => w.MemberType.ToString().ToLower()))

            //.ForMember(member => member.IsOnDock, dto => dto.MapFrom(w => w.IsOnDock))
            //.ForMember(member => member.IsMax, dto => dto.MapFrom(w => w.IsMax))
            //.ForMember(member => member.IsFull, dto => dto.MapFrom(w => w.IsFull))
            //.ForMember(member => member.IsSetbar, dto => dto.MapFrom(w => w.IsSetbar))
            //.ForMember(member => member.IsOpenMax, dto => dto.MapFrom(w => w.IsOpenMax))
            //.ForMember(member => member.IsLock, dto => dto.MapFrom(w => w.IsLock))
            //.ForMember(member => member.IsFlash, dto => dto.MapFrom(w => w.IsFlash))
            //.ForMember(member => member.IsDraw, dto => dto.MapFrom(w => w.IsDraw))
            //.ForMember(member => member.IsResize, dto => dto.MapFrom(w => w.IsResize))

            //.ForMember(member => member.AppUrl, dto => dto.MapFrom(w => w.AppUrl));
            #endregion


            #endregion
        }

        #region DomainModelToDto

        /// <summary>
        /// 领域模型转换成DTO
        /// </summary>
        /// <typeparam name="TModel">领域模型</typeparam>
        /// <typeparam name="TDto">DTO模型</typeparam>
        /// <param name="source">领域模型</param>
        /// <returns></returns>
        public static TDto ConvertToDto<TModel, TDto>(this TModel source) where TModel : DomainModelBase where TDto : BaseDto
        {
            return Mapper.Map<TDto>(source);
        }

        /// <summary>
        /// 领域模型转换成DTO
        /// </summary>
        /// <typeparam name="TModel">领域模型</typeparam>
        /// <typeparam name="TDto">DTO模型</typeparam>
        /// <param name="source">领域模型</param>
        /// <returns></returns>
        public static IEnumerable<TDto> ConvertToDtos<TModel, TDto>(this IEnumerable<TModel> source) where TModel : DomainModelBase where TDto : BaseDto
        {
            return Mapper.Map<IList<TDto>>(source);
        }

        /// <summary>
        /// 将动态类型转换为指定的dto
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<TDto> ConvertDynamicToDtos<TDto>(this IEnumerable<dynamic> source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMissingTypeMaps = true);
            var mapper = config.CreateMapper();

            return source.Select(mapper.Map<TDto>).ToList();
        }

        /// <summary>
        /// 将动态类型转换为指定的dto
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDto ConvertDynamicToDto<TDto>(dynamic source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMissingTypeMaps = true);
            var mapper = config.CreateMapper();
            return mapper.Map<TDto>(source);
        }

        #endregion

        #region DtoToDomainModel
        /// <summary>
        /// DTO转换成领域模型
        /// </summary>
        /// <typeparam name="TDto">DTO</typeparam>
        /// <typeparam name="TModel">领域模型</typeparam>
        /// <param name="source">DTO</param>
        /// <returns></returns>
        public static TModel ConvertToModel<TDto, TModel>(this TDto source)
            where TDto : BaseDto
            where TModel : DomainModelBase
        {
            return Mapper.Map<TModel>(source);
        }

        public static IEnumerable<TModel> ConvertToModel<TDto, TModel>(this IList<TDto> source)
            where TDto : BaseDto
            where TModel : DomainModelBase
        {
            return Mapper.Map<IEnumerable<TModel>>(source);
        }

        #endregion
    }
}
