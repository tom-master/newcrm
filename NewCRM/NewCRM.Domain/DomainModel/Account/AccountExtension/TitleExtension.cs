using System;

namespace NewCRM.Domain.Entities.DomainModel.Account
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


        public void Remove()
        {
            IsDeleted = true;
        }

        #endregion

     
    }
}
