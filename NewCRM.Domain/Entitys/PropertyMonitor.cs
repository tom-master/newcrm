using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace NewCRM.Domain.Entitys
{
    public abstract class PropertyMonitor : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IDictionary<String, Object> _propertyValues = new Dictionary<String, Object>();

        public PropertyMonitor()
        {
            if(PropertyChanged != null)
            {
                PropertyChanged += PropertyMonitor_PropertyChanged;
            }
        }

        private void PropertyMonitor_PropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            var instanceName = this.GetType().FullName;
            if(!_propertyValues.Keys.Contains(instanceName))
            {
                _propertyValues.Add(new KeyValuePair<String, Object>(instanceName, e.PropertyName));
            }
        }

        public void OnPropertyChanged(String propertyName)
        {
            var temp = Interlocked.CompareExchange(ref PropertyChanged, null, null);

            if(temp != null)
            {
                temp(this, null);
            }
        }


        public IDictionary<String, Object> GetPropertyValues()
        {
            return _propertyValues;
        }
    }
}
