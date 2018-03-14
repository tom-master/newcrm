using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entitys.System
{
    [Description("应用类型"), Serializable]
    public partial class AppType : DomainModelBase
    {
        #region private field

        private String _name;

        private String _remark;
        #endregion

        #region public proptery

        /// <summary>
        /// 名称
        /// </summary>
        [Required, MaxLength(6)]
        public String Name
        {
            get { return _name; }
            private set
            {
                if(_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(50)]
        public String Remark
        {
            get { return _remark; }
            private set
            {
                if(_remark != value)
                {
                    _remark = value;
                    OnPropertyChanged(nameof(Remark));
                }
            }
        }

        #endregion

        #region ctor
        /// <summary>
        /// 实例化一个app类型对象
        /// </summary>
        public AppType(String name, String remark = default(String))
        {
            Name = name;
            Remark = remark;
        }

        public AppType()
        {
        }

        #endregion
    }

    /// <summary>
    /// AppTypeExtension
    /// </summary>
    public partial class AppType
    {
        public AppType ModifyName(String appTypeName)
        {
            Name = appTypeName;
            OnPropertyChanged(nameof(Name));
            return this;
        }

        public AppType ModifyRemark(String remark)
        {
            Remark = remark;
            OnPropertyChanged(nameof(Remark));
            return this;
        }
    }
}
