using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.Factory.DomainCreate;
using NewCRM.Domain.Factory.DomainQuery.Query;
using NewCRM.Domain.Factory.DomainSpecification;
using NewCRM.Domain.Repositories;
using NewCRM.Infrastructure.CommonTools.CustomExtension;

namespace NewCRM.Domain.Factory.DomainQuery.ConcreteQuery
{
	[Export("EF", typeof(QueryBase))]
	internal class DefaultQuery : QueryBase
	{
		private readonly IDomainModelQueryProvider _queryProvider;

		private readonly DomainFactory _domainFactory;

		[ImportingConstructor]
		public DefaultQuery(
			[Import("EF", typeof(IDomainModelQueryProvider))]IDomainModelQueryProvider queryProvider,
			[Import(typeof(DomainFactory))] DomainFactory domainFactory)
		{
			_queryProvider = queryProvider;

			_domainFactory = domainFactory;
		}

		/// <summary>
		/// 查找并返回单个对象
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <returns></returns>
		public override T FindOne<T>(Specification<T> specification, Expression<Func<T, Object>> selector)
		{
			return ConvertToDomain<T>(_queryProvider.Query(specification).Select(selector).ToList()).FirstOrDefault();
		}

		/// <summary>查找并返回单个对象</summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="specification"></param>
		/// <returns></returns>
		public override T FindOne<T>(Specification<T> specification)
		{
			return _queryProvider.Query(specification).FirstOrDefault();
		}

		/// <summary>
		/// 查找并返回集合
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <returns></returns>
		public override IEnumerable<T> Find<T>(Specification<T> specification, Expression<Func<T, Object>> selector)
		{
			return ConvertToDomain<T>(_queryProvider.Query(specification).Select(selector).ToList());
		}


		/// <summary>查找并返回集合</summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="specification"></param>
		/// <returns></returns>
		public override IEnumerable<T> Find<T>(Specification<T> specification)
		{
			return _queryProvider.Query(specification);
		}

		/// <summary>
		/// 分页
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <param name="totalCount"></param>
		/// <returns></returns>
		public override IEnumerable<T> PageBy<T>(Specification<T> specification, Int32 pageIndex, Int32 pageSize, out Int32 totalCount, Expression<Func<T, Object>> selector)
		{

			IQueryable<T> query = _queryProvider.Query(specification);

			totalCount = query.Count();

			return ConvertToDomain<T>(query.PageBy(pageIndex, pageSize, specification.OrderBy).Select(selector).ToList());

		}

		/// <summary>分页</summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="specification"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <param name="totalCount"></param>
		/// <returns></returns>
		public override IEnumerable<T> PageBy<T>(Specification<T> specification, int pageIndex, int pageSize, out int totalCount)
		{
			IQueryable<T> query = _queryProvider.Query(specification);

			totalCount = query.Count();

			return query.PageBy(pageIndex, pageSize, specification.OrderBy);
		}


		public IEnumerable<T> ConvertToDomain<T>(IEnumerable<Object> parameters) where T : DomainModelBase, IAggregationRoot
		{
			if (!parameters.Any())
			{
				return new List<T>();
			}

			var parameterType = parameters.FirstOrDefault().GetType();

			var domainType = typeof(T);

			IList<T> lst = new List<T>();

			foreach (var parameter in parameters)
			{
				var instance = _domainFactory.Create<T>();

				lst.Add(instance);

				foreach (var domainProperty in domainType.GetProperties())
				{
					var property = parameterType.GetProperty(domainProperty.Name);

					if (property == null)
					{
						continue;
					}

					domainProperty.SetValue(instance, property.GetValue(parameter));
				}
			}

			return lst;
		}
	}
}
