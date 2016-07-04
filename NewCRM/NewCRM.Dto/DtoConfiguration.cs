using System.Collections.Generic;
using AutoMapper;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustomExtension;

namespace NewCRM.Dto
{
    public static class DtoConfiguration
    {

        static DtoConfiguration()
        {
            #region Domain To Dto

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
                .ForMember(dto => dto.WallpaperMode, user => user.MapFrom(u => u.Config.WallpaperMode.ToString()))
                .ForMember(dto => dto.AppXy, user => user.MapFrom(u => u.Config.AppXy.ToString()))
                .ForMember(dto => dto.DockPosition, user => user.MapFrom(u => u.Config.DockPosition.ToString()))
                .ForMember(dto => dto.WallpaperUrl, user => user.MapFrom(u => u.Config.Wallpaper.Url))
                .ForMember(dto => dto.WallpaperWidth, user => user.MapFrom(u => u.Config.Wallpaper.Width))
                .ForMember(dto => dto.WallpaperHeigth, user => user.MapFrom(u => u.Config.Wallpaper.Heigth))
                .ForMember(dto => dto.WallpaperSource, user => user.MapFrom(u => u.Config.Wallpaper.Source))
                .ForMember(dto => dto.Desks, user => user.MapFrom(u => u.Config.Desks))
                .ForMember(dto => dto.UserId, user => user.MapFrom(u => u.Id))
                .ForMember(dto=>dto.ConfigId,user=>user.MapFrom(u=>u.Config.Id));



            #endregion
        }


        #region DomainModelToDto

        /// <summary>
        /// 领域模型转换成DTO
        /// </summary>
        /// <typeparam name="T1">领域模型</typeparam>
        /// <typeparam name="T2">DTO模型</typeparam>
        /// <param name="source">领域模型</param>
        /// <returns></returns>
        public static T2 ConvertToDto<T1, T2>(this T1 source)
        {
            return Mapper.Map<T1, T2>(source);
        }

        public static IList<T2> ConvertToDto<T1, T2>(this IEnumerable<T1> source)
        {
            return Mapper.Map<IEnumerable<T1>, IList<T2>>(source);
        }

        #endregion

        #region DtoToDomainModel
        /// <summary>
        /// DTO转换成领域模型
        /// </summary>
        /// <typeparam name="T1">DTO</typeparam>
        /// <typeparam name="T2">领域模型</typeparam>
        /// <param name="source">DTO</param>
        /// <returns></returns>
        public static T2 ConvertToDomainModel<T1, T2>(this T1 source)
        {
            return Mapper.Map<T1, T2>(source);
        }

        public static IEnumerable<T2> ConvertToDomainModel<T1, T2>(this IList<T1> source)
        {
            return Mapper.Map<IList<T1>, IEnumerable<T2>>(source);
        }

        #endregion
    }
}
