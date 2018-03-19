using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Mapper.MapperExtension
{
    public abstract class Validate : Attribute
    {
        public abstract Boolean IsValidate(Object value);
    }
}
