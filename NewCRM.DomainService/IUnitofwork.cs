using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entitys;

namespace NewCRM.Domain.Services
{
    public interface IUnitofwork
    {

        void RegisterAdd<TModel>(TModel model) where TModel : DomainModelBase;

        void RegisterModify<TModel>(TModel model) where TModel : DomainModelBase;

        void Commit(Boolean isValidateModel = default(Boolean));
    }
}
