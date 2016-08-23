using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entities.DomainModel;

namespace NewCRM.Domain.Services.DomainSpecification
{
    public interface ISpecification<in T> where T : DomainModelBase
    {
        Boolean IsSatisfiedBy(T candidate);
    }
}
