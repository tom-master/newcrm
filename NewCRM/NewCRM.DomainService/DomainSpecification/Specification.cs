using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entities.DomainModel;

namespace NewCRM.Domain.Services.DomainSpecification
{
    internal sealed class Specification<T> : ISpecification<T> where T : DomainModelBase
    {
        private readonly Func<T, Boolean> _func;

        internal Specification(Func<T, Boolean> func)
        {
            _func = func;
        }


        public Boolean IsSatisfiedBy(T candidate)
        {
            return _func(candidate);
        }
    }
}
