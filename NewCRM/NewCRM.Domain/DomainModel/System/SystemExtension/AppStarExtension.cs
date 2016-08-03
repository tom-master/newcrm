using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Domain.Entities.DomainModel.System
{
    public partial class AppStar
    {
        public void RemoveStar()
        {
            IsDeleted = true;
        }
    }
}
