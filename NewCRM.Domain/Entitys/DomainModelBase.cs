using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NewCRM.Domain.KeyGenerators;

namespace NewCRM.Domain.Entitys
{
    [Serializable]
    public abstract class DomainModelBase : RedisKeyGenerator
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

        public DateTime AddTime { get; protected set; } = DateTime.Parse($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}");

        public DateTime LastModifyTime { get; protected set; } = DateTime.Parse($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}");

        #endregion
    }
}