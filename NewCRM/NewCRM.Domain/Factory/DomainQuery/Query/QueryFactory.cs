namespace NewCRM.Domain.DomainQuery.Query
{
    public abstract class QueryFactory
    {
        /// <summary>
        /// 创建一个查询
        /// </summary>
        public abstract IQuery CreateQuery { get; }
    }
}
