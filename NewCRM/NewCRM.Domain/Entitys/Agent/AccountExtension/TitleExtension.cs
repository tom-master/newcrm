using System;

namespace NewCRM.Domain.Entitys.Agent
{
    public partial class Title
    {

        #region public method
        /// <summary>
        /// 修改职称名
        /// </summary>
        /// <param name="newTitleName"></param>
        public void ModifyTitleName(String newTitleName)
        {
            Name = newTitleName;
        }

        /// <summary>
        /// 移除职称
        /// </summary>
        public void Remove()
        {
            IsDeleted = true;
        }

        public override String KeyGenerator()
        {
            return $"NewCRM:{nameof(Title)}:Id:{Id}";
        }

        #endregion
    }
}
