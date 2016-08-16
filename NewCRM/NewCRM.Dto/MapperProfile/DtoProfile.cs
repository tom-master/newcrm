using System;
using System.Linq;
using AutoMapper;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainModel.Security;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Dto.Dto;

namespace NewCRM.Dto.MapperProfile
{

    internal class UserToUserDtoProfile : Profile
    {
        public UserToUserDtoProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dto => dto.Name, user => user.MapFrom(u => u.Name))
                .ForMember(dto => dto.Skin, user => user.MapFrom(u => u.Config.Skin))
                .ForMember(dto => dto.UserFace, user => user.MapFrom(u => u.Config.UserFace))
                .ForMember(dto => dto.AppSize, user => user.MapFrom(u => u.Config.AppSize))
                .ForMember(dto => dto.AppVerticalSpacing, user => user.MapFrom(u => u.Config.AppVerticalSpacing))
                .ForMember(dto => dto.AppHorizontalSpacing, user => user.MapFrom(u => u.Config.AppHorizontalSpacing))
                .ForMember(dto => dto.DefaultDeskNumber, user => user.MapFrom(u => u.Config.DefaultDeskNumber))
                .ForMember(dto => dto.WallpaperMode, user => user.MapFrom(u => u.Config.WallpaperMode.ToString().ToLower()))
                .ForMember(dto => dto.AppXy, user => user.MapFrom(u => u.Config.AppXy.ToString().ToLower()))
                .ForMember(dto => dto.DockPosition, user => user.MapFrom(u => u.Config.DockPosition.ToString().ToLower()))
                .ForMember(dto => dto.WallpaperUrl, user => user.MapFrom(u => u.Config.Wallpaper.Url))
                .ForMember(dto => dto.WallpaperWidth, user => user.MapFrom(u => u.Config.Wallpaper.Width))
                .ForMember(dto => dto.WallpaperHeigth, user => user.MapFrom(u => u.Config.Wallpaper.Height))
                .ForMember(dto => dto.WallpaperSource, user => user.MapFrom(u => u.Config.Wallpaper.Source.ToString().ToLower()))
                .ForMember(dto => dto.Desks, user => user.MapFrom(u => u.Config.Desks.Select(s => new DeskDto
                {
                    DeskNumber = s.DeskNumber,
                    Id = s.Id
                })))
                .ForMember(dto => dto.Id, user => user.MapFrom(u => u.Id))
                .ForMember(dto => dto.UserType, user => user.MapFrom(u => u.IsAdmin ? "管理员" : "用户"));


        }
    }

    internal class WallpaperToWallpaperDtoProfile : Profile
    {
        public WallpaperToWallpaperDtoProfile()
        {
            CreateMap<Wallpaper, WallpaperDto>()
                .ForMember(dto => dto.Id, wallpaper => wallpaper.MapFrom(w => w.Id))
                .ForMember(dto => dto.Height, wallpaper => wallpaper.MapFrom(w => w.Height))
                .ForMember(dto => dto.Source, wallpaper => wallpaper.MapFrom(w => w.Source.ToString().ToLower()))
                .ForMember(dto => dto.Title, wallpaper => wallpaper.MapFrom(w => w.Title))
                .ForMember(dto => dto.Url, wallpaper => wallpaper.MapFrom(w => w.Url))
                .ForMember(dto => dto.Width, wallpaper => wallpaper.MapFrom(w => w.Width))
                .ForMember(dto => dto.ShortUrl, wallpaper => wallpaper.MapFrom(w => w.ShortUrl));
        }
    }

    internal class WallpaperDtoToWallpaperProfile : Profile
    {
        public WallpaperDtoToWallpaperProfile()
        {
            CreateMap<WallpaperDto, Wallpaper>()
                .ForMember(wallpaper => wallpaper.Id, dto => dto.MapFrom(d => d.Id))
                .ForMember(wallpaper => wallpaper.Height, dto => dto.MapFrom(d => d.Height))
                .ForMember(wallpaper => wallpaper.Source, dto => dto.MapFrom(d => EnumOp.ConvertEnum(typeof(WallpaperSource), d.Source)))
                .ForMember(wallpaper => wallpaper.Title, dto => dto.MapFrom(d => d.Title))
                .ForMember(wallpaper => wallpaper.Url, dto => dto.MapFrom(d => d.Url))
                .ForMember(wallpaper => wallpaper.Width, dto => dto.MapFrom(d => d.Width))
                .ForMember(wallpaper => wallpaper.ShortUrl, dto => dto.MapFrom(d => d.ShortUrl))
                .ForMember(wallpaper => wallpaper.Md5, dto => dto.MapFrom(d => d.Md5));
        }
    }

    internal class MemberToMemberDtoProfile : Profile
    {
        public MemberToMemberDtoProfile()
        {
            CreateMap<Member, MemberDto>()
                .ForMember(dto => dto.Id, member => member.MapFrom(w => w.Id))
                .ForMember(dto => dto.AppId, member => member.MapFrom(w => w.AppId))
                .ForMember(dto => dto.Width, member => member.MapFrom(w => w.Width))
                .ForMember(dto => dto.Height, member => member.MapFrom(w => w.Height))
                .ForMember(dto => dto.FolderId, member => member.MapFrom(w => w.FolderId))
                .ForMember(dto => dto.Name, member => member.MapFrom(w => w.Name))
                .ForMember(dto => dto.IconUrl, member => member.MapFrom(w => w.IconUrl))
                .ForMember(dto => dto.MemberType, member => member.MapFrom(w => w.MemberType.ToString().ToLower()))

                .ForMember(dto => dto.IsOnDock, member => member.MapFrom(w => w.IsOnDock))
                .ForMember(dto => dto.IsMax, member => member.MapFrom(w => w.IsMax))
                .ForMember(dto => dto.IsFull, member => member.MapFrom(w => w.IsFull))
                .ForMember(dto => dto.IsSetbar, member => member.MapFrom(w => w.IsSetbar))
                .ForMember(dto => dto.IsOpenMax, member => member.MapFrom(w => w.IsOpenMax))
                .ForMember(dto => dto.IsLock, member => member.MapFrom(w => w.IsLock))
                .ForMember(dto => dto.IsFlash, member => member.MapFrom(w => w.IsFlash))
                .ForMember(dto => dto.IsDraw, member => member.MapFrom(w => w.IsDraw))
                .ForMember(dto => dto.IsResize, member => member.MapFrom(w => w.IsResize))

                .ForMember(dto => dto.AppUrl, member => member.MapFrom(w => w.AppUrl));
        }
    }

    internal class AppTypeToAppTypeDtoProfile : Profile
    {
        public AppTypeToAppTypeDtoProfile()
        {
            CreateMap<AppType, AppTypeDto>()
                .ForMember(dto => dto.Id, appType => appType.MapFrom(w => w.Id))
                .ForMember(dto => dto.Name, appType => appType.MapFrom(w => w.Name));
        }
    }

    internal class AppTypeDtoToAppTypeProfile : Profile
    {
        public AppTypeDtoToAppTypeProfile()
        {
            CreateMap<AppTypeDto, AppType>()
                .ForMember(appType => appType.Id, dto => dto.MapFrom(w => w.Id))
                .ForMember(appType => appType.Name, dto => dto.MapFrom(w => w.Name));
        }
    }

    internal class AppToAppDtoProfile : Profile
    {
        public AppToAppDtoProfile()
        {
            CreateMap<App, AppDto>()
                .ForMember(dto => dto.Id, app => app.MapFrom(w => w.Id))
                .ForMember(dto => dto.Name, app => app.MapFrom(w => w.Name))
                .ForMember(dto => dto.IconUrl, app => app.MapFrom(w => w.IconUrl))
                .ForMember(dto => dto.Remark, app => app.MapFrom(w => w.Remark))
                .ForMember(dto => dto.UserCount, app => app.MapFrom(w => w.UserCount))
                .ForMember(dto => dto.StartCount, app => app.MapFrom(w =>
                             w.AppStars.Any(appStar => appStar.IsDeleted == false)
                                 ? (w.AppStars.Sum(s => s.StartNum) * 1.0) /
                                   (w.AppStars.Count(appStar => appStar.IsDeleted == false) * 1.0) : 0.0))
                .ForMember(dto => dto.UserId, app => app.MapFrom(w => w.UserId))
                .ForMember(dto => dto.AppStyle, app => app.MapFrom(w => (Int32)w.AppStyle))
                .ForMember(dto => dto.AppType, app => app.MapFrom(w => w.AppType.Name))
                .ForMember(dto => dto.AddTime, app => app.MapFrom(w => w.AddTime.ToString("yyyy-MM-dd")))
                .ForMember(dto => dto.AppAuditState, app => app.MapFrom(w => (Int32)w.AppAuditState))
                .ForMember(dto => dto.AppReleaseState, app => app.MapFrom(w => (Int32)w.AppReleaseState));
        }
    }

    internal class AppDtoToAppProfile : Profile
    {
        public AppDtoToAppProfile()
        {
            CreateMap<AppDto, App>()
                .ForMember(app => app.Id, dto => dto.MapFrom(w => w.Id))
                .ForMember(app => app.Name, dto => dto.MapFrom(w => w.Name))
                .ForMember(app => app.IconUrl, dto => dto.MapFrom(w => w.IconUrl))
                .ForMember(app => app.AppUrl, dto => dto.MapFrom(w => w.AppUrl))
                .ForMember(app => app.Remark, dto => dto.MapFrom(w => w.Remark))
                .ForMember(app => app.Width, dto => dto.MapFrom(w => w.Width))
                .ForMember(app => app.Height, dto => dto.MapFrom(w => w.Height))
                .ForMember(app => app.IsOpenMax, dto => dto.MapFrom(w => w.IsOpenMax))
                .ForMember(app => app.IsFlash, dto => dto.MapFrom(w => w.IsFlash))
                .ForMember(app => app.IsResize, dto => dto.MapFrom(w => w.IsResize))
                .ForMember(app => app.UserId, dto => dto.MapFrom(w => w.UserId))
                .ForMember(
                    app => app.AppAuditState,
                    dto => dto.MapFrom(w => EnumOp.ConvertEnum(typeof(AppAuditState), w.AppAuditState)))
                .ForMember(
                    app => app.AppReleaseState,
                    dto => dto.MapFrom(w => EnumOp.ConvertEnum(typeof(AppReleaseState), w.AppReleaseState)))
                .ForMember(app => app.AppTypeId, dto => dto.MapFrom(w => w.AppTypeId))
                .ForMember(app => app.AppStyle, dto => dto.MapFrom(w => EnumOp.ConvertEnum(typeof(AppStyle), w.AppStyle)));
        }
    }


    internal class RoleToRoleDtoProfile : Profile
    {
        public RoleToRoleDtoProfile()
        {
            CreateMap<Role, RoleDto>()
                .ForMember(dto => dto.Name, role => role.MapFrom(r => r.Name))
                .ForMember(dto => dto.RoleIdentity, role => role.MapFrom(r => r.RoleIdentity))
                .ForMember(dto => dto.Remark, role => role.MapFrom(r => r.Remark))
                .ForMember(dto => dto.Id, role => role.MapFrom(r => r.Id))
                .ForMember(dto => dto.Powers, role => role.MapFrom(r => r.Powers.Select(s => s.PowerId)));
        }
    }

    internal class RoleDtoToRoleProfile : Profile
    {
        public RoleDtoToRoleProfile()
        {
            CreateMap<RoleDto, Role>()
               .ForMember(role => role.Name, dto => dto.MapFrom(r => r.Name))
               .ForMember(role => role.RoleIdentity, dto => dto.MapFrom(r => r.RoleIdentity))
               .ForMember(role => role.Remark, dto => dto.MapFrom(r => r.Remark))
               .ForMember(role => role.Id, dto => dto.MapFrom(r => r.Id))
               .ForMember(role => role.Powers, dto => dto.MapFrom(r => r.Powers));
        }
    }

    internal class PowerToPowerDtoProfile : Profile
    {
        public PowerToPowerDtoProfile()
        {
            CreateMap<Power, PowerDto>()
                .ForMember(dto => dto.Name, power => power.MapFrom(r => r.Name))
                .ForMember(dto => dto.PowerIdentity, power => power.MapFrom(r => r.PowerIdentity))
                .ForMember(dto => dto.Remark, power => power.MapFrom(r => r.Remark))
                .ForMember(dto => dto.Id, power => power.MapFrom(r => r.Id));
        }
    }

    internal class PowerDtoToPowerProfile : Profile
    {
        public PowerDtoToPowerProfile()
        {
            CreateMap<PowerDto, Power>()
                .ForMember(power => power.Name, dto => dto.MapFrom(r => r.Name))
                .ForMember(power => power.PowerIdentity, dto => dto.MapFrom(r => r.PowerIdentity))
                .ForMember(power => power.Remark, dto => dto.MapFrom(r => r.Remark))
                .ForMember(power => power.Id, dto => dto.MapFrom(r => r.Id));
        }
    }





    internal static class EnumOp
    {

        /// <summary>
        /// 转换枚举
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static Enum ConvertEnum(Type target, Int32 value)
            => (Enum)Enum.Parse(target, Enum.GetName(target, value), true);

        /// <summary>
        /// 转换枚举
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static Enum ConvertEnum(Type target, String value) => (Enum)Enum.Parse(target, value, true);

    }
}
