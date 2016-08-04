using System;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entities.DomainModel
{
    [Serializable]
    public abstract class DomainModelBase
    {
        #region private field

        private Int32 _id;

        private DateTime _dateTime;

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
        public DateTime AddTime
        {
            get { return _dateTime; }
            protected set { _dateTime = value; }
        }

        #endregion


    }
}