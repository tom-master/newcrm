using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Mapper.MapperExtension
{
    public class DefaultValueAttribute : Validate
    {
        private Type _valueType;

        private Object _value;

        public DefaultValueAttribute(Type valueType, Object value = default(Object))
        {
            _valueType = valueType;

            if(valueType == typeof(DateTime))
            {
                _value = DateTime.Now;
            }
            else
            {
                _value = value;
            }

        }


        public Type TypeValue { get { return _valueType; } }

        public Object Value { get { return _value; } }

        public override bool IsValidate(object value)
        {
            try
            {
                Convert.ChangeType(value, _valueType);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
