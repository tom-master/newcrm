using System;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entitys
{
    [Serializable]
    public abstract class DomainModelBase
    {
        #region ctor

        protected DomainModelBase()
        {
            IsDeleted = false;
        }

        #endregion

        #region public property

        [Key]
        public Int32 Id { get; protected set; }

        public Boolean IsDeleted { get; protected set; }

        [DataType(DataType.DateTime)]
        public DateTime AddTime { get; protected set; }

        [DataType(DataType.DateTime)]
        public DateTime LastModifyTime { get; set; }=DateTime.Now;

        #endregion
    }
}