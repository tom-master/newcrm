using System;

namespace NewCRM.Domain.Entities.DomainModel.System
{
    public partial class AppType
    {
        #region public method

        /// <summary>
        /// 修改app类型名称
        /// </summary>
        /// <param name="newAppTypeName"></param>
        public void ModifyAppTypeName(String newAppTypeName)
        {
            Name = newAppTypeName;
        }


        #endregion
    }
}
