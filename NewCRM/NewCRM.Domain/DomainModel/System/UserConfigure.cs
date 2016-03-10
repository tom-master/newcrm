using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NewCRM.Domain.DomainModel.Account;

namespace NewCRM.Domain.DomainModel.System
{
    [Description("用户配置")]
    [Serializable]
    public class UserConfigure : EntityBase<Int32>, IAggregationRoot
    {

        #region private field
        private String _appXy;
        private String _dockPosition;
        private String _skin;
        private String _dock;
        private String _remark;

        private String _wallpaperShowType;
        private String _userFace;

        private Int32 _appSize;
        private Int32 _appVerticalSpacing;
        private Int32 _appHorizontalSpacing;


        private String _desk1;
        private String _desk2;
        private String _desk3;
        private String _desk4;
        private String _desk5;



        private Wallpaper _wallpaper;
        private User _user;

        #endregion

        #region ctor


        public UserConfigure()
        {
            // TODO: Complete member initialization
        }

        #endregion

        #region public attribute

        //新增字段
        [StringLength(10)]
        public String AppXy
        {
            get { return _appXy; }
            set { _appXy = value; }
        }

        [StringLength(10)]
        public String DockPosition
        {
            get { return _dockPosition; }
            set { _dockPosition = value; }
        }

        [StringLength(10)]
        public String Skin
        {
            get { return _skin; }
            set { _skin = value; }
        }

        [StringLength(500)]
        public String Dock
        {
            get { return _dock; }
            set { _dock = value; }
        }
        [StringLength(500)]
        public String Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }


        [StringLength(50)]
        public String WallpaperShowType
        {
            get { return _wallpaperShowType; }
            set { _wallpaperShowType = value; }
        }

        public String UserFace
        {
            get { return _userFace; }
            set { _userFace = value; }
        }

        public Int32 AppSize
        {
            get { return _appSize; }
            set { _appSize = value; }
        }

        public Int32 AppVerticalSpacing
        {
            get { return _appVerticalSpacing; }
            set { _appVerticalSpacing = value; }
        }

        public Int32 AppHorizontalSpacing
        {
            get { return _appHorizontalSpacing; }
            set { _appHorizontalSpacing = value; }
        }

        public String Desk1
        {
            get { return _desk1; }
            set
            {
                _desk1 = value;
            }
        }


        public String Desk2
        {
            get { return _desk2; }
            set
            {
                _desk2 = value;
            }
        }


        public String Desk3
        {
            get { return _desk3; }
            set
            {
                _desk3 = value;
            }
        }


        public String Desk4
        {
            get { return _desk4; }
            set
            {
                _desk4 = value;
            }
        }


        public String Desk5
        {
            get { return _desk5; }
            set
            {
                _desk5 = value;
            }
        }

        public virtual Wallpaper Wallpaper
        {
            get { return _wallpaper; }
            set { _wallpaper = value; }
        }

        public virtual User User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        #endregion
    }
}
