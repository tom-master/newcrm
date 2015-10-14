﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.DomainModel.Account;

namespace NewCRM.Domain.DomainModel.System
{
    [Description("系统配置")]
    [Serializable]
    public class UserConfig : EntityBase<Int32>
    {

        #region private field
        private String _appXy;
        private String _dockPosition;
        private String _skin;
        private String _dock;
        private String _remark;

        private String _wallpaperShowType;
        private String _userFace;
        private Int32 _defaultDesk;
        private Int32 _appSize;
        private Int32 _appVerticalSpacing;
        private Int32 _appHorizontalSpacing;


        private User _user;

        #endregion

        #region ctor


        public UserConfig()
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

        public Int32 DefaultDesk
        {
            get { return _defaultDesk; }
            set { _defaultDesk = value; }
        }

        public virtual User User
        {
            get { return _user; }
            set { _user = value; }
        }
        #endregion
    }
}