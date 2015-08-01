using System.Collections.Generic;
using AutoMapper;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Dto.Dto;

namespace NewCRM.Dto
{
    public static class DtoConfiguration
    {

        static DtoConfiguration()
        {
            #region user
            Mapper.CreateMap<User, UserDto>();
            Mapper.CreateMap<UserDto, User>();
            #endregion

            #region wallpaper
            Mapper.CreateMap<Wallpaper, WallpaperDto>();
            Mapper.CreateMap<WallpaperDto, Wallpaper>();
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
