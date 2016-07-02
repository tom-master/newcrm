using System;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entities.DomainModel
{
    [Serializable]
    public abstract class DomainModelBase 
    {
        /// <summary>
        /// field
        /// </summary>
        private Int32 _id;

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


        public bool IsDeleted { get; protected set; }

        [DataType(DataType.DateTime)]
        public DateTime AddTime
        {
            get
            {
                return DateTime.Now;
            }

            private set
            {

            }
        }


        #endregion


    }
}