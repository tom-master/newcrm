using System;
using System.Linq;
using AutoMapper;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.ValueObject;
using NewCRM.Dto.Dto;

namespace NewCRM.Dto.MapperProfile
{

    internal class AccountToAccountDtoProfile : Profile
    {
        public AccountToAccountDtoProfile()
        {
            CreateMap<Account, AccountDto>()
                .ForMember(dto => dto.Name, account => account.MapFrom(u => u.Name))
                .ForMember(dto => dto.Id, account => account.MapFrom(u => u.Id))
                .ForMember(dto => dto.AccountType, account => account.MapFrom(u => u.IsAdmin ? "管理员" : "用户"))
                .ForMember(dto => dto.Password, account => account.MapFrom(u => u.LoginPassword))
                .ForMember(dto => dto.Roles, account => account.MapFrom(u => u.AccountRoles.Select(s => new RoleDto
                {
                    Id = s.RoleId,
                    Name=s.Role.Name,
                    RoleIdentity = s.Role.RoleIdentity,
                    Powers = s.Role.Powers.Select(power=>new PowerDto
                    {
                        Id =power.Id
                    }).ToList()
                }).ToList()));
        }
    }


    internal class AccountDtoToAccountProfile : Profile
    {
        public AccountDtoToAccountProfile()
        {
            CreateMap<AccountDto, Account>()
                .ForMember(account => account.Name, dto => dto.MapFrom(d => d.Name))
                .ForMember(account => account.Id, dto => dto.MapFrom(d => d.Id))
                .ForMember(account => account.LoginPassword, dto => dto.MapFrom(d => d.Password))
                .ForMember(account => account.IsAdmin, dto => dto.MapFrom(d => Int32.Parse(d.AccountType) == 2))
                .ForMember(account => account.AccountRoles, dto => dto.MapFrom(d => d.Roles));
        }
    }


    internal class ConfigToConfigDtoProfile : Profile
    {
        public ConfigToConfigDtoProfile()
        {
            CreateMap<Config, ConfigDto>()
                .ForMember(dto => dto.Skin, config => config.MapFrom(c => c.Skin))
                .ForMember(dto => dto.AccountFace, config => config.MapFrom(c => c.AccountFace))
                .ForMember(dto => dto.AppSize, config => config.MapFrom(c => c.AppSize))
                .ForMember(dto => dto.AppVerticalSpacing, config => config.MapFrom(c => c.AppVerticalSpacing))
                .ForMember(dto => dto.AppHorizontalSpacing, config => config.MapFrom(c => c.AppHorizontalSpacing))
                .ForMember(dto => dto.DefaultDeskNumber, config => config.MapFrom(c => c.DefaultDeskNumber))
                .ForMember(dto => dto.AppXy, config => config.MapFrom(c => c.AppXy))
                .ForMember(dto => dto.DockPosition, config => config.MapFrom(c => c.DockPosition))
                .ForMember(dto => dto.WallpaperUrl, config => config.MapFrom(c => c.Wallpaper.Url))
                .ForMember(dto => dto.WallpaperWidth, config => config.MapFrom(c => c.Wallpaper.Width))
                .ForMember(dto => dto.WallpaperHeigth, config => config.MapFrom(c => c.Wallpaper.Height))
                .ForMember(dto => dto.WallpaperSource, config => config.MapFrom(c => c.Wallpaper.Source))
                .ForMember(dto => dto.WallpaperMode, config => config.MapFrom(c => c.WallpaperMode))
                .ForMember(dto => dto.Desks, account => account.MapFrom(u => u.Desks.Select(s => new DeskDto
                {
                    DeskNumber = s.DeskNumber,
                    Id = s.Id
                })));

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
                .ForMember(dto => dto.UseCount, app => app.MapFrom(w => w.UseCount))
                .ForMember(dto => dto.StartCount, app => app.MapFrom(w =>
                             w.AppStars.Any()
                                 ? (w.AppStars.Sum(s => s.StartNum) * 1.0) /
                                   (w.AppStars.Count * 1.0) : 0.0))
                .ForMember(dto => dto.AccountId, app => app.MapFrom(w => w.AccountId))
                .ForMember(dto => dto.AppStyle, app => app.MapFrom(w => (Int32)w.AppStyle))
                .ForMember(dto => dto.AppTypeName, app => app.MapFrom(w => w.AppType.Name))
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
                .ForMember(app => app.AccountId, dto => dto.MapFrom(w => w.AccountId))
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

    internal class RoleDtoToAccountRoleProfile : Profile
    {
        public RoleDtoToAccountRoleProfile()
        {
            CreateMap<RoleDto, AccountRole>()
                .ForMember(accountRole => accountRole.RoleId, dto => dto.MapFrom(d => d.Id));
        }
    }

    internal class AccountRoleToRoleDtoProfile : Profile
    {
        public AccountRoleToRoleDtoProfile()
        {
            CreateMap<AccountRole, RoleDto>()
                .ForMember(dto => dto.Id, accountRole => accountRole.MapFrom(role => role.RoleId));
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
            // ReSharper disable once AssignNullToNotNullAttribute
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
