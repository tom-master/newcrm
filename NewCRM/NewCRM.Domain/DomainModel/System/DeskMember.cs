using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Domain.DomainModel.System
{
    [Serializable]
    [Description("桌面成员")]

    public class DeskMember : EntityBase<Int32>
    {
        private Int32 _userId;

        private Int32 _appId;

        private Int32 _folderId;

        private MemberType _memberType;

        public Int32 UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
            }
        }

        public Int32 AppId
        {
            get { return _appId; }
            set
            {
                _appId = value;
            }
        }

        public Int32 FolderId
        {
            get { return _folderId; }
            set
            {
                _folderId = value;
            }
        }

        public MemberType MemberType
        {
            get { return _memberType; }
            set
            {
                _memberType = value;
            }
        }
    }
}
