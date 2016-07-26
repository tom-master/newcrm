using System;

namespace NewCRM.Domain.Entities.DomainModel.Account
{
    public class UserRole : DomainModelBase
    {
        #region public property


        public Int32 UserId { get; private set; }


        public Int32 RoleId { get; private set; }

        #endregion


        #region ctor

        public UserRole(Int32 userId, Int32 roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public UserRole() { }

        public void Remove()
        {
            IsDeleted = true;
        }


        #endregion

    }
}
