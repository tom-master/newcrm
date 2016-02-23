using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NewCRM.Domain.DomainModel.Account;

namespace NewCRM.Domain.DomainModel.System
{
    [Serializable]
    [Description("壁纸")]
    public class Wallpaper : EntityBase<Int32>, IAggregationRoot
    {
        #region private fiele

        private String _title;
        private String _url;
        private String _displayType;
        private String _description;
        private Int32 _width;
        private Int32 _heigth;
        private Boolean _isSystem;
        private Int32 _uploaderId;

        #endregion

        #region ctor



        public Wallpaper()
        {
            // TODO: Complete member initialization
        }

        #endregion

        #region public attribute

        [Required, StringLength(50)]
        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [Required, StringLength(100)]
        public String Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public Int32 Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public Int32 Heigth
        {
            get { return _heigth; }
            set { _heigth = value; }
        }

        [StringLength(250)]
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public Boolean IsSystem
        {
            get { return _isSystem; }
            set { _isSystem = value; }
        }

        [StringLength(10)]
        public String DisplayType
        {
            get { return _displayType; }
            set { _displayType = value; }
        }

        public Int32 UploaderId
        {
            get { return _uploaderId; }
            set { _uploaderId = value; }
        }
        #endregion
    }
}
