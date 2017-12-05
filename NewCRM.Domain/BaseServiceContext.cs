using NewCRM.Infrastructure.CommonTools.CustomHelper;
using NewLib.Validate;

namespace NewCRM.Domain
{

    public class BaseServiceContext
    {
        /// <summary>
        /// 参数验证
        /// </summary>
        public ParameterValidate ValidateParameter => new ParameterValidate();
    }
}
