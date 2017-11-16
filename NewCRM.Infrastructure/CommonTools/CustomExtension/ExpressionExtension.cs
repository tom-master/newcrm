using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NewCRM.Infrastructure.CommonTools.CustomExtension
{
    public static class ExpressionExtension
    {

        public static String GeneratorRedisKey<T>(this Expression expression)
        {
            StringBuilder key = new StringBuilder($"NewCRM:{typeof(T).Name}:");

            Parse(expression, ref key);

            return key.ToString().TrimEnd(':');

        }

        private static Expression Parse(Expression expression, ref StringBuilder key)
        {
            if (expression == null)
            {
                return null;
            }

            switch (expression.NodeType)
            {
                //一元运算符
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:

                //二元运算符
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    {
                        var binary = (BinaryExpression)expression;

                        Parse(binary.Left, ref key);

                        Parse(binary.Right, ref key);

                        return expression;
                    }
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                    {

                        var equalExpression = (BinaryExpression)expression;

                        Parse((MemberExpression)equalExpression.Left, ref key);

                        Parse(equalExpression.Right, ref key);

                        return expression;
                    }
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:

                //其他
                case ExpressionType.Call:
                case ExpressionType.Lambda:
                    {
                        var lambda = (LambdaExpression)expression;

                        return Parse(lambda.Body, ref key);
                    }
                case ExpressionType.MemberAccess:
                    {
                        var memberExpression = (MemberExpression)expression;

                        var expressionExpression = memberExpression.Expression as ConstantExpression;

                        if (expressionExpression != null)
                        {
                            var constantExpression = expressionExpression;

                            var field = constantExpression.Type.GetMember(memberExpression.Member.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).FirstOrDefault();

                            Object value = null;

                            if (field is PropertyInfo)
                            {
                                value = (field as PropertyInfo).GetValue(constantExpression.Value);
                            }
                            else if (field is FieldInfo)
                            {
                                value = (field as FieldInfo).GetValue(constantExpression.Value);
                            }


                            key.Append($"{value}:");

                        }
                        else
                        {
                            key.Append($"{memberExpression.Member.Name}:");
                        }

                        return expression;
                    }
                case ExpressionType.Parameter:
                case ExpressionType.Constant:
                    {
                        return expression;
                    }
                case ExpressionType.TypeIs:
                case ExpressionType.Invoke:
                    {
                        var exp = (InvocationExpression)expression;

                        var value = Expression.Lambda(Expression.Invoke(exp.Expression, exp.Arguments)).Compile().DynamicInvoke();

                        key.Append($"{value}:");

                        return expression;
                    }
                case ExpressionType.Conditional:
                    {
                        var exp = (ConditionalExpression)expression;

                        var isEqual = (Boolean)Expression.Lambda((BinaryExpression)exp.Test).Compile().DynamicInvoke();

                        if (isEqual)
                        {
                            Parse(exp.IfTrue, ref key);

                        }
                        else
                        {
                            Parse(exp.IfFalse, ref key);
                        }

                        return expression;
                    }
                default:
                    throw new Exception($"Unhandled expression type: '{expression.NodeType}'");
            }
        }


    }
}
