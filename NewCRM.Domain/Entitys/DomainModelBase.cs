using System;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entitys
{
    [Serializable]
    public abstract class DomainModelBase : PropertyMonitor
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

        public Boolean IsDeleted { get; private set; }

        public DateTime AddTime
        {
            get
            {
                return DateTime.Now;
            }
        }

        public DateTime LastModifyTime
        {
            get
            {
                return DateTime.Now;
            }
        }


        public void Remove()
        {
            IsDeleted = true;
            OnPropertyChanged(nameof(IsDeleted));
        }

        #endregion
    }
}