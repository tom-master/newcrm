using NewCRM.Domain.UnitWork;

namespace NewCRM.DomainService
{
    /// <summary>
    /// 基础服务实现
    /// </summary>
    public abstract class BaseService
    {
        protected IUnitOfWork UnitOfWork { get; set; }
    }
}
