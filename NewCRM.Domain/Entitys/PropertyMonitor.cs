using System;
using System.Collections.Generic;
using System.Threading;

namespace NewCRM.Domain.Entitys
{
    public abstract class PropertyMonitor
    {
        event EventHandler OnPropertyChanged;

        protected virtual void PropertyCreate()
        {
            var temp = Interlocked.CompareExchange(ref OnPropertyChanged, null, null);
            if(temp != null)
            {
                temp(this, null);
            }
        }

        public virtual IDictionary<String, Object> GetPropertyValues()
        {
            throw new NotImplementedException();
        }
    }
}
