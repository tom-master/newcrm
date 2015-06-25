using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace NewCRM.Domain.DomainModel.System
{
    [Description("应用类型")]
    [Serializable]
    public class AppType : EntityBase<Int32>,IAggregationRoot
    {
        #region private field
        private String _name;
        private String _remake;
        private ICollection<App> _apps;
        #endregion

        #region ctor
        public AppType(String name, String remake)
            : this()
        {
            _name = name;
            _remake = remake;
        }

        public AppType()
        {
            // TODO: Complete member initialization
        }
        #endregion

        #region public attribute
        [Required, StringLength(50)]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [StringLength(250)]
        public String Remake
        {
            get { return _remake; }
            set { _remake = value; }
        }
   
        public virtual ICollection<App> Apps
        {
            get { return _apps; }
            set { _apps = value; }
        }
        #endregion
    }
}
