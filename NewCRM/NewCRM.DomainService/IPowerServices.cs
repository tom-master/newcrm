using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainModel.Security;

namespace NewCRM.Domain.Services
{
    public interface IPowerServices
    {

        /// <summary>
        /// 获取所有的权限
        /// </summary>
        /// <returns></returns>
        List<dynamic> GetAllPowers();

        /// <summary>
        /// 创建新权限
        /// </summary>
        /// <param name="power"></param>
        void AddNewPower(Power power);

        /// <summary>
        /// 获取所有的权限
        /// </summary>
        /// <param name="powerName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Power> GetAllPowers(String powerName, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);

        /// <summary>
        /// 根据powerId获取权限信息
        /// </summary>
        /// <param name="powerId"></param>
        /// <returns></returns>
        Power GetPower(Int32 powerId);

        /// <summary>
        /// 修改权限信息
        /// </summary>
        /// <param name="power"></param>
        void ModifyPower(Power power);

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="powerId"></param>
        void RemovePower(Int32 powerId);

       
    }
}
