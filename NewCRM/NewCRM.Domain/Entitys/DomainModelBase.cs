using System;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entitys
{
    [Serializable]
    public abstract class DomainModelBase
    {
        #region private field

        private Int32 _id;

        #endregion

        #region ctor

        protected DomainModelBase()
        {
            IsDeleted = false;
        }

        #endregion

        #region public property

        [Key]
        public Int32 Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public Boolean IsDeleted { get; protected set; }

        [DataType(DataType.DateTime)]
        public DateTime AddTime { get; protected set; } = DateTime.Now;

        #endregion
    }
}