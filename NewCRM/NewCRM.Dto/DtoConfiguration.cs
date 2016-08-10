using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using NewCRM.Domain.Entities.DomainModel;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainModel.Security;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Dto.Dto;

namespace NewCRM.Dto
{
    public static class DtoConfiguration
    {
        static DtoConfiguration()
        {

            #region Account


            Mapper.CreateMap<User, UserDto>()
                .ForMember(dto => dto.Name, user => user.MapFrom(u => u.Name))
                //.ForMember(dto => dto.Title, user => user.MapFrom(u => u.Title.Name))
                //.ForMember(dto => dto.IsOnline, user => user.MapFrom(u => u.IsOnline))
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
                .ForMember(dto => dto.Desks, user => user.MapFrom(u => u.Config.Desks))
                .ForMember(dto => dto.Id, user => user.MapFrom(u => u.Id));
            //.ForMember(dto => dto.ConfigId, user => user.MapFrom(u => u.Config.Id));
            #endregion

            #region Wallpaper

            #region domain -> dto
            Mapper.CreateMap<Wallpaper, WallpaperDto>()
                .ForMember(dto => dto.Id, wallpaper => wallpaper.MapFrom(w => w.Id))
                .ForMember(dto => dto.Height, wallpaper => wallpaper.MapFrom(w => w.Height))
                .ForMember(dto => dto.Source, wallpaper => wallpaper.MapFrom(w => w.Source.ToString().ToLower()))
                .ForMember(dto => dto.Title, wallpaper => wallpaper.MapFrom(w => w.Title))
                .ForMember(dto => dto.Url, wallpaper => wallpaper.MapFrom(w => w.Url))
                .ForMember(dto => dto.Width, wallpaper => wallpaper.MapFrom(w => w.Width))
                .ForMember(dto => dto.ShortUrl, wallpaper => wallpaper.MapFrom(w => w.ShortUrl));
            #endregion

            #region dto -> domain
            Mapper.CreateMap<WallpaperDto, Wallpaper>()
                  .ForMember(wallpaper => wallpaper.Id, dto => dto.MapFrom(d => d.Id))
                .ForMember(wallpaper => wallpaper.Height, dto => dto.MapFrom(d => d.Height))
                .ForMember(wallpaper => wallpaper.Source, dto => dto.MapFrom(d => ConvertEnum(typeof(WallpaperSource), d.Source)))
                .ForMember(wallpaper => wallpaper.Title, dto => dto.MapFrom(d => d.Title))
                .ForMember(wallpaper => wallpaper.Url, dto => dto.MapFrom(d => d.Url))
                .ForMember(wallpaper => wallpaper.Width, dto => dto.MapFrom(d => d.Width))
                .ForMember(wallpaper => wallpaper.ShortUrl, dto => dto.MapFrom(d => d.ShortUrl))
                .ForMember(wallpaper => wallpaper.Md5, dto => dto.MapFrom(d => d.Md5));
            #endregion

            #endregion

            #region Member

            #region domain -> dto
            Mapper.CreateMap<Member, MemberDto>()
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

            #endregion

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

            #region AppType

            Mapper.CreateMap<AppType, AppTypeDto>()
                .ForMember(dto => dto.Id, member => member.MapFrom(w => w.Id))
                .ForMember(dto => dto.Name, member => member.MapFrom(w => w.Name));


            #endregion

            #region App

            #region domain -> dto
            Mapper.CreateMap<App, AppDto>()
                .ForMember(dto => dto.Id, app => app.MapFrom(w => w.Id))
                .ForMember(dto => dto.Name, app => app.MapFrom(w => w.Name))
                .ForMember(dto => dto.IconUrl, app => app.MapFrom(w => w.IconUrl))
                .ForMember(dto => dto.Remark, app => app.MapFrom(w => w.Remark))
                .ForMember(dto => dto.UserCount, app => app.MapFrom(w => w.UserCount))
                .ForMember(dto => dto.StartCount, app => app.MapFrom(w => w.AppStars.Any(appStar => appStar.IsDeleted == false) ? (w.AppStars.Sum(s => s.StartNum) * 1.0) / (w.AppStars.Count(appStar => appStar.IsDeleted == false) * 1.0) : 0.0))
                .ForMember(dto => dto.UserId, app => app.MapFrom(w => w.UserId))
                .ForMember(dto => dto.AppStyle, app => app.MapFrom(w => (Int32)w.AppStyle))
                .ForMember(dto => dto.AppType, app => app.MapFrom(w => w.AppType.Name))
                .ForMember(dto => dto.AddTime, app => app.MapFrom(w => w.AddTime.ToString("yyyy-MM-dd")))
                .ForMember(dto => dto.AppAuditState, app => app.MapFrom(w => (Int32)w.AppAuditState))
                .ForMember(dto => dto.AppReleaseState, app => app.MapFrom(w => (Int32)w.AppReleaseState));
            #endregion

            #region dto -> domain

            Mapper.CreateMap<AppDto, App>()
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
                .ForMember(app => app.AppAuditState, dto => dto.MapFrom(w => ConvertEnum(typeof(AppAuditState), w.AppAuditState)))
                .ForMember(app => app.AppReleaseState, dto => dto.MapFrom(w => ConvertEnum(typeof(AppReleaseState), w.AppReleaseState)))
                .ForMember(app => app.AppTypeId, dto => dto.MapFrom(w => w.AppTypeId))
                .ForMember(app => app.AppStyle, dto => dto.MapFrom(w => ConvertEnum(typeof(AppStyle), w.AppStyle)));

            #endregion

            #endregion

            #region Role

            Mapper.CreateMap<Role, RoleDto>()
                .ForMember(dto => dto.Id, role => role.MapFrom(r => r.Id))
                .ForMember(dto => dto.Name, role => role.MapFrom(r => r.Name));

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
            return Mapper.Map<TModel, TDto>(source);
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
            return Mapper.Map<IEnumerable<TModel>, IList<TDto>>(source);
        }

        /// <summary>
        /// 将动态类型转换为指定的dto
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<TDto> ConvertDynamicToDtos<TDto>(this IEnumerable<dynamic> source)
        {
            return source.Select(Mapper.DynamicMap<TDto>).ToList();
        }

        /// <summary>
        /// 将动态类型转换为指定的dto
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDto ConvertDynamicToDto<TDto>(dynamic source)
        {
            return Mapper.DynamicMap<TDto>(source);
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
            return Mapper.Map<TDto, TModel>(source);
        }

        public static IEnumerable<TModel> ConvertToModel<TDto, TModel>(this IList<TDto> source)
            where TDto : BaseDto
            where TModel : DomainModelBase
        {
            return Mapper.Map<IList<TDto>, IEnumerable<TModel>>(source);
        }

        #endregion


        #region private method

        /// <summary>
        /// 转换枚举
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static Enum ConvertEnum(Type target, Int32 value)
            => (Enum)Enum.Parse(target, Enum.GetName(target, value), true);



        /// <summary>
        /// 转换枚举
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static Enum ConvertEnum(Type target, String value) => (Enum)Enum.Parse(target, value, true);

        #endregion
    }
}
