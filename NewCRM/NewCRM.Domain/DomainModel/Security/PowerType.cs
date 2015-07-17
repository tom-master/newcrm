using System;
using System.Collections.Generic;

namespace NewCRM.Domain.DomainModel.Security
{
    public class PowerType : EntityBase<Int32>, IAggregationRoot
    {
        #region private field
        private String _typeName;
        private ICollection<Power> _powers;
        #endregion

        #region ctor

      
        public PowerType()
        {
            // TODO: Complete member initialization
        }

        #endregion

        #region public attirbute
        public String TypeName
        {
            get { return _typeName; }
            set { _typeName = value; }
        }

        public ICollection<Power> Powers
        {
            get { return _powers; }
            set { _powers = value; }
        }
        #endregion
    }
}
