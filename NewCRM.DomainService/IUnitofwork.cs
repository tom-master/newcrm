using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Domain.Services
{
    public interface IUnitofwork
    {

        void RegisterAdd<TModel>(TModel model) where TModel : class;

        void RegisterModify<TModel>(TModel model) where TModel : class;

        void Commit(Boolean isValidateModel = default(Boolean));
    }
}
