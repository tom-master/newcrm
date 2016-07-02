using NewCRM.Domain.Entities.UnitWork;

namespace NewCRM.Domain.Services
{
    /// <summary>
    /// 基础服务实现
    /// </summary>
    public abstract class BaseService
    {
        protected IUnitOfWork UnitOfWork { get; set; }
    }
}
