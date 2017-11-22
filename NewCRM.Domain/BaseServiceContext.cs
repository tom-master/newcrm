using NewCRM.Infrastructure.CommonTools.CustomHelper;

namespace NewCRM.Domain
{

    public class BaseServiceContext
    {
        /// <summary>
        /// 参数验证
        /// </summary>
        public Parameter ValidateParameter => new Parameter();
    }
}
