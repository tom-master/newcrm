using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using NewCRM.Domain.Entities.DomainModel;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustomExtension;

namespace NewCRM.Dto
{
    public static class DtoConfiguration
    {
        static DtoConfiguration()
        {

            #region Account


            Mapper.CreateMap<User, UserDto>()
                .ForMember(dto => dto.Name, user => user.MapFrom(u => u.Name))
                .ForMember(dto => dto.Title, user => user.MapFrom(u => u.Title.Name))
                .ForMember(dto => dto.IsOnline, user => user.MapFrom(u => u.IsOnline))
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
                .ForMember(dto => dto.WallpaperHeigth, user => user.MapFrom(u => u.Config.Wallpaper.Heigth))
                .ForMember(dto => dto.WallpaperSource, user => user.MapFrom(u => u.Config.Wallpaper.Source.ToString().ToLower()))
                .ForMember(dto => dto.Desks, user => user.MapFrom(u => u.Config.Desks))
                .ForMember(dto => dto.Id, user => user.MapFrom(u => u.Id))
                .ForMember(dto => dto.ConfigId, user => user.MapFrom(u => u.Config.Id));
            #endregion

            #region Wallpaper

            #region domain -> dto
            Mapper.CreateMap<Wallpaper, WallpaperDto>()
                .ForMember(dto => dto.Id, wallpaper => wallpaper.MapFrom(w => w.Id))
                .ForMember(dto => dto.Heigth, wallpaper => wallpaper.MapFrom(w => w.Heigth))
                .ForMember(dto => dto.Source, wallpaper => wallpaper.MapFrom(w => w.Source.ToString().ToLower()))
                .ForMember(dto => dto.Title, wallpaper => wallpaper.MapFrom(w => w.Title))
                .ForMember(dto => dto.Url, wallpaper => wallpaper.MapFrom(w => w.Url))
                .ForMember(dto => dto.Width, wallpaper => wallpaper.MapFrom(w => w.Width))
                .ForMember(dto => dto.ShortUrl, wallpaper => wallpaper.MapFrom(w => w.ShortUrl));
            #endregion

            #region dto -> domain
            Mapper.CreateMap<WallpaperDto, Wallpaper>()
                  .ForMember(wallpaper => wallpaper.Id, dto => dto.MapFrom(d => d.Id))
                .ForMember(wallpaper => wallpaper.Heigth, dto => dto.MapFrom(d => d.Heigth))
                .ForMember(wallpaper => wallpaper.Source, dto => dto.MapFrom(d => d.Source))
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
            Mapper.CreateMap<MemberDto, Member>()
            .ForMember(member => member.Id, dto => dto.MapFrom(w => w.Id))
            .ForMember(member => member.AppId, dto => dto.MapFrom(w => w.AppId))
            .ForMember(member => member.Width, dto => dto.MapFrom(w => w.Width))
            .ForMember(member => member.Height, dto => dto.MapFrom(w => w.Height))
            .ForMember(member => member.FolderId, dto => dto.MapFrom(w => w.FolderId))
            .ForMember(member => member.Name, dto => dto.MapFrom(w => w.Name))
            .ForMember(member => member.IconUrl, dto => dto.MapFrom(w => w.IconUrl))
            .ForMember(member => member.MemberType, dto => dto.MapFrom(w => w.MemberType.ToString().ToLower()))

            .ForMember(member => member.IsOnDock, dto => dto.MapFrom(w => w.IsOnDock))
            .ForMember(member => member.IsMax, dto => dto.MapFrom(w => w.IsMax))
            .ForMember(member => member.IsFull, dto => dto.MapFrom(w => w.IsFull))
            .ForMember(member => member.IsSetbar, dto => dto.MapFrom(w => w.IsSetbar))
            .ForMember(member => member.IsOpenMax, dto => dto.MapFrom(w => w.IsOpenMax))
            .ForMember(member => member.IsLock, dto => dto.MapFrom(w => w.IsLock))
            .ForMember(member => member.IsFlash, dto => dto.MapFrom(w => w.IsFlash))
            .ForMember(member => member.IsDraw, dto => dto.MapFrom(w => w.IsDraw))
            .ForMember(member => member.IsResize, dto => dto.MapFrom(w => w.IsResize))

            .ForMember(member => member.AppUrl, dto => dto.MapFrom(w => w.AppUrl));
            #endregion


            #endregion

            #region AppType

            Mapper.CreateMap<AppType, AppTypeDto>()
                .ForMember(dto => dto.Id, member => member.MapFrom(w => w.Id))
                .ForMember(dto => dto.Name, member => member.MapFrom(w => w.Name));


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

        public static List<TDto> ConvertToDto<TModel, TDto>(this IEnumerable<TModel> source) where TModel : DomainModelBase where TDto : BaseDto
        {
            return Mapper.Map<IEnumerable<TModel>, IList<TDto>>(source).ToList();
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
    }
}
