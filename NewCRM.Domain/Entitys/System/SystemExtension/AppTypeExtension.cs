using System;

namespace NewCRM.Domain.Entitys.System
{
    public partial class AppType
    {
        #region public method

        /// <summary>
        /// 修改app类型名称
        /// </summary>
        public void ModifyAppTypeName(String newAppTypeName)
        {
            Name = newAppTypeName;
        }

        /// <summary>
        /// 移除app类型
        /// </summary>
        public void Remove()
        {
            IsDeleted = true;
        }

        public override String KeyGenerator()
        {
            return $"NewCRM:{nameof(AppType)}";
        }

        #endregion
    }
}
