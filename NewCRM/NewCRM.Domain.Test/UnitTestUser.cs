using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewCRM.Domain.Entitys.Agent;
using Newtonsoft.Json;

namespace NewCRM.Domain.Test
{
    [TestClass]
    public class UnitTestUser
    {
        [TestMethod]
        public void TestMethod()
        {
            Expression<Func<A1, object>> expression = a1 => a1.DateTime;

            Test(expression);

        }

        private void Test<T>(Expression<Func<T, object>> expression)
        {
            List<A1> list = new List<A1>
            {
                new A1
                {
                    DateTime = DateTime.Now.AddMinutes(5)
                },
                new A1
                {
                    DateTime = DateTime.Now.AddMinutes(10)
                },
                new A1
                {
                    DateTime = DateTime.Now.AddMinutes(15)
                },
                new A1
                {
                    DateTime = DateTime.Now.AddMinutes(20)
                },
            };

            var member = ((MemberExpression)((UnaryExpression)expression.Body).Operand).Member;

            var parameter = Expression.Parameter(typeof(T), "t");

            var memberAccess = Expression.MakeMemberAccess(parameter, member);

            dynamic lam = Expression.Lambda(memberAccess, parameter);

            try
            {

                Expression<Func<A1, Object>> expression22 = a1 => a1.DateTime;

                var result = OrderByEx<A1>(list.AsQueryable(), expression22);
            }
            catch (Exception)
            {

                throw;
            }

        }


        public IOrderedQueryable<TSource> OrderByEx<TSource>(IQueryable<TSource> source, Expression<Func<TSource, object>> keySelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }

            Expression body = keySelector.Body;

            if (body.NodeType == ExpressionType.Convert)
            {
                body = ((UnaryExpression)body).Operand;
            }

            LambdaExpression keySelector2 = Expression.Lambda(body, keySelector.Parameters);
            Type tkey = keySelector2.ReturnType;

            MethodInfo orderbyMethod = (from x in typeof(Queryable).GetMethods()
                                        where x.Name == "OrderByDescending"
                                        let parameters = x.GetParameters()
                                        where parameters.Length == 2
                                        let generics = x.GetGenericArguments()
                                        where generics.Length == 2
                                        where parameters[0].ParameterType == typeof(IQueryable<>).MakeGenericType(generics[0]) &&
                                            parameters[1].ParameterType == typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(generics[0], generics[1]))
                                        select x).Single();

            return (IOrderedQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(null, orderbyMethod.MakeGenericMethod(typeof(TSource), tkey), new[]
            {
            source.Expression,
            Expression.Quote(keySelector2)
            }));
        }
    }


    public class A1
    {
        //public Int32 Id { get; set; }

        //public String Name { get; set; }

        //public IList<A2> A2 { get; set; }

        public DateTime DateTime { get; set; }

    }

    public class A2
    {
        //public Int32 Id { get; set; }
    }
}
