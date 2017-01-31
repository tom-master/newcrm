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
                .ForMember(dto => dto.IsAdmin, account => account.MapFrom(u => u.IsAdmin))
                .ForMember(dto => dto.Password, account => account.MapFrom(u => u.LoginPassword))
                .ForMember(dto => dto.AddTime, account => account.MapFrom(u => u.AddTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dto => dto.LastLoginTime, account => account.MapFrom(u => u.LastLoginTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dto => dto.LastModifyTime, account => account.MapFrom(u => u.LastModifyTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dto => dto.Roles, account => account.MapFrom(u => u.AccountRoles.Select(s => new RoleDto
                {
                    Id = s.RoleId,
                    Name = s.Role.Name,
                    RoleIdentity = s.Role.RoleIdentity,
                    Powers = s.Role.Powers.Select(power => new PowerDto
                    {
                        Id = power.AppId
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
                .ForMember(account => account.IsAdmin, dto => dto.MapFrom(d => d.IsAdmin))
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
                .ForMember(dto => dto.DefaultDeskCount, config => config.MapFrom(c => c.DefaultDeskCount));
            //.ForMember(dto => dto.Desks, account => account.MapFrom(u => u.Desks.Select(s => new DeskDto
            //{
            //    DeskNumber = s.DeskNumber,
            //    Id = s.Id
            //})));

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

                .ForMember(dto => dto.AppUrl, member => member.MapFrom(w => w.AppUrl))
                .ForMember(dto => dto.DeskId, member => member.MapFrom(w => w.DeskId));
        }
    }

    internal class MemberDtoToMemberProfile : Profile
    {
        public MemberDtoToMemberProfile()
        {
            CreateMap<MemberDto, Member>()
             .ForMember(member => member.Id, dto => dto.MapFrom(w => w.Id))
             .ForMember(member => member.AppId, dto => dto.MapFrom(w => w.AppId))
             .ForMember(member => member.Width, dto => dto.MapFrom(w => w.Width))
             .ForMember(member => member.Height, dto => dto.MapFrom(w => w.Height))
             .ForMember(member => member.FolderId, dto => dto.MapFrom(w => w.FolderId))
             .ForMember(member => member.Name, dto => dto.MapFrom(w => w.Name))
             .ForMember(member => member.IconUrl, dto => dto.MapFrom(w => w.IconUrl))
             .ForMember(member => member.MemberType, dto => dto.MapFrom(w => EnumOp.ConvertEnum(typeof(MemberType), w.MemberType)))

             .ForMember(member => member.IsOnDock, dto => dto.MapFrom(w => w.IsOnDock))
             .ForMember(member => member.IsMax, dto => dto.MapFrom(w => w.IsMax))
             .ForMember(member => member.IsFull, dto => dto.MapFrom(w => w.IsFull))
             .ForMember(member => member.IsSetbar, dto => dto.MapFrom(w => w.IsSetbar))
             .ForMember(member => member.IsOpenMax, dto => dto.MapFrom(w => w.IsOpenMax))
             .ForMember(member => member.IsLock, dto => dto.MapFrom(w => w.IsLock))
             .ForMember(member => member.IsFlash, dto => dto.MapFrom(w => w.IsFlash))
             .ForMember(member => member.IsDraw, dto => dto.MapFrom(w => w.IsDraw))
             .ForMember(member => member.IsResize, dto => dto.MapFrom(w => w.IsResize))

             .ForMember(member => member.AppUrl, dto => dto.MapFrom(w => w.AppUrl))

            .ForMember(member => member.DeskId, dto => dto.MapFrom(w => w.DeskId));
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
                //.ForMember(dto => dto.AppTypeName, app => app.MapFrom(w => w.AppType.Name))
                .ForMember(dto => dto.AddTime, app => app.MapFrom(w => w.AddTime.ToString("yyyy-MM-dd")))
                .ForMember(dto => dto.AppAuditState, app => app.MapFrom(w => (Int32)w.AppAuditState))
                .ForMember(dto => dto.AppReleaseState, app => app.MapFrom(w => (Int32)w.AppReleaseState))
                .ForMember(dto => dto.IsRecommand, app => app.MapFrom(w => w.IsRecommand));
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
                .ForMember(dto => dto.Powers, role => role.MapFrom(r => r.Powers.Select(s => s.AppId)));
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

    internal class DeskToDeskDtoProfile : Profile
    {
        public DeskToDeskDtoProfile()
        {
            CreateMap<Desk, DeskDto>().ForMember(dto => dto.Id, desk => desk.MapFrom(d => d.Id))
                .ForMember(dto => dto.DeskNumber, desk => desk.MapFrom(d => d.DeskNumber))
                .ForMember(dto => dto.Members, desk => desk.MapFrom(d => d.Members))
                .ForMember(dto => dto.AccountId, desk => desk.MapFrom(d => d.AccountId));
        }
    }

    internal class DeskDtoToDeskProfile : Profile
    {
        public DeskDtoToDeskProfile()
        {
            CreateMap<DeskDto, Desk>().ForMember(desk => desk.Id, dto => dto.MapFrom(d => d.Id))
                .ForMember(desk => desk.DeskNumber, dto => dto.MapFrom(d => d.DeskNumber))
                .ForMember(desk => desk.Members, dto => dto.MapFrom(d => d.Members))
                .ForMember(desk => desk.AccountId, dto => dto.MapFrom(d => d.AccountId));
        }
    }

    internal class LogDtoToLogProfile : Profile
    {
        public LogDtoToLogProfile()
        {
            CreateMap<LogDto, Log>()
                .ForMember(log => log.Id, dto => dto.MapFrom(d => d.Id))
                .ForMember(log => log.LogLevelEnum, dto => dto.MapFrom(d => EnumOp.ConvertEnum(typeof(LogLevel), d.LogLevelEnum)))
                .ForMember(log => log.Controller, dto => dto.MapFrom(d => d.Controller))
                .ForMember(log => log.Action, dto => dto.MapFrom(d => d.Action))
                .ForMember(log => log.ExceptionMessage, dto => dto.MapFrom(d => d.ExceptionMessage))
                .ForMember(log => log.Track, dto => dto.MapFrom(d => d.Track))
                .ForMember(log => log.AccountId, dto => dto.MapFrom(d => d.AccountId));
        }
    }

    internal class LogToLogDtoProfile : Profile
    {
        public LogToLogDtoProfile()
        {
            CreateMap<Log, LogDto>()
                .ForMember(dto => dto.Id, log => log.MapFrom(d => d.Id))
                .ForMember(dto => dto.LogLevelEnum, log => log.MapFrom(d => (Int32)d.LogLevelEnum))
                .ForMember(dto => dto.Controller, log => log.MapFrom(d => d.Controller))
                .ForMember(dto => dto.Action, log => log.MapFrom(d => d.Action))
                .ForMember(dto => dto.ExceptionMessage, log => log.MapFrom(d => d.ExceptionMessage))
                .ForMember(dto => dto.Track, log => log.MapFrom(d => d.Track))
                .ForMember(dto => dto.AccountId, log => log.MapFrom(d => d.AccountId));
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
