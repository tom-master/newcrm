using System;

namespace ConsoleApplication1.Mapper.MapperExtension
{

    public class InputRangeAttribute : Validate
    {
        private Int32 _min;

        private Int32 _max;

        public InputRangeAttribute(Int32 min, Int32 max)
        {
            _min = min;
            _max = max;
        }

        public InputRangeAttribute(Int32 max)
        {
            _min = 0;
            _max = max;
        }

        public override Boolean IsValidate(Object value)
        {
            if(value == null)
            {
                return false;
            }

            var internalValue = value.ToString();

            var valueLength = internalValue.Length;

            if(_min == 0 && valueLength <= _max)
            {
                return true;
            }

            if(valueLength < _min || valueLength > _max)
            {
                return false;
            }

            if(valueLength > _min && valueLength < _max)
            {
                return true;
            }

            return false;
        }
    }
}
