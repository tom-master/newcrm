using System.ComponentModel.Composition;

namespace NewCRM.Domain.UnitWork
{
    public abstract class InternalImportUnitOfWork
    {

        /// <summary>
        /// 获取 仓储上下文的实例
        /// </summary>
        [Import]
        protected IUnitOfWork UnitOfWork { get; set; }

    }
}
