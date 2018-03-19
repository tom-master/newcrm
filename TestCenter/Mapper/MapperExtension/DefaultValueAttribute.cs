using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Mapper.MapperExtension
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DefaultValueAttribute : Validate
    {
        private Type _valueType;

        private Object _value;

        public DefaultValueAttribute(Type valueType, Object value)
        {
            _valueType = valueType;
            _value = value;
        }

        public Type TypeValue { get { return _valueType; } }

        public Object Value { get { return _value; } }

        public override bool IsValidate(object value)
        {
            if(_valueType == typeof(Boolean))
            {
                Convert.ChangeType(value, _valueType);
            }

            return false;
        }
    }
}
