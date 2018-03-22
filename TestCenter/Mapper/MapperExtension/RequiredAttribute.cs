using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Mapper.MapperExtension
{

    public class RequiredAttribute : Validate
    {
        public override bool IsValidate(object value)
        {
            return !(value == null);
        }
    }
}
