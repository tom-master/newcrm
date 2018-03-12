using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entitys;

namespace NewCRM.Domain.Services
{
    public class Unitofwork : IUnitofwork
    {
        private IDictionary<String, Object> _registerAdds;

        private IDictionary<String, Object> _registerModifys;

        public Unitofwork()
        {
            _registerAdds = new Dictionary<String, Object>();

            _registerModifys = new Dictionary<String, Object>();
        }


        public void Commit(bool isValidateModel = false)
        {
            //to do 安拉胡阿克巴
        }

        public void RegisterAdd<TModel>(TModel model) where TModel : DomainModelBase
        {
            var modelName = model.GetType().Name;
            if(!_registerAdds.Keys.Contains(modelName))
            {
                _registerAdds.Add(new KeyValuePair<String, Object>(modelName, model));
            }
        }

        public void RegisterModify<TModel>(TModel model) where TModel : DomainModelBase
        {
            var modelName = model.GetType().Name;
            if(!_registerAdds.Keys.Contains(modelName))
            {
                _registerAdds.Add(new KeyValuePair<String, Object>(modelName, model));
            }
        }
    }
}
