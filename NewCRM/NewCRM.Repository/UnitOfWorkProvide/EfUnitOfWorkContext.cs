using System;
using System.Data.Entity;
using Microsoft.Practices.Unity;
using NewCRM.Repository.DatabaseProvider.EF.Context;

namespace NewCRM.Repository.UnitOfWorkProvide
{
	/// <summary>
	/// 数据单元操作类
	/// </summary> 
	public class EfUnitOfWorkContext : UnitOfWorkContextBase
	{
		/// <summary>
		/// 获取 当前使用的数据访问上下文对象
		/// </summary>
		
		protected override DbContext Context => EfDbContext.Value;

		[Dependency]
		public Lazy<NewCrmContext> EfDbContext { get; set; }

	}
}
