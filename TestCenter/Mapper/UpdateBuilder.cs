using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace ConsoleApplication1.Mapper
{
    public class UpdateBuilder<TModel> : SqlBuilder<TModel> where TModel : class, new()
    {

        public UpdateBuilder(TModel model) : base(model)
        {

        }

        public override void GenerateSqlHead()
        {
            Append($@"UPDATE {ModelType.Name} SET ");
            Append($@"{String.Join(",", GetFields().Select(s => $@"{s}=@{s}"))}");
        }

        protected override IEnumerable<string> GetFields()
        {
            var method = ModelType.GetMethod("GetPropertyValues").Invoke(ModelInstance, null);
            var returnValue = method as IDictionary<String, Object>;
            if(returnValue != null)
            {
                foreach(var item in returnValue)
                {
                    AddSqlParameters(new SqlParameter($@"@{item.Key}", item.Value));
                    yield return item.Key;
                }
            }

        }

        private void AppendField(Object fieldName)
        {
            Append($@" {fieldName} ");
        }

        public void GenerateCondition(Expression exp)
        {
            if(!ToString().Contains("WHERE"))
            {
                Append(" WHERE ");
            }

            switch(exp.NodeType)
            {
                case ExpressionType.Add:
                    break;
                case ExpressionType.AddChecked:
                    break;
                case ExpressionType.And:
                    break;
                case ExpressionType.AndAlso:
                    {
                        var binaryExp = (BinaryExpression)exp;
                        And();
                        GenerateCondition(binaryExp.Left);
                        And();
                        GenerateCondition(binaryExp.Right);
                        break;
                    }
                case ExpressionType.ArrayLength:
                    break;
                case ExpressionType.ArrayIndex:
                    break;
                case ExpressionType.Call:
                    break;
                case ExpressionType.Conditional:
                    break;
                case ExpressionType.Constant:
                    break;
                case ExpressionType.Equal:
                    {
                        var binaryExp = (BinaryExpression)exp;
                        GenerateCondition(binaryExp.Left);
                        if(binaryExp.Right.NodeType != ExpressionType.Constant)
                        {
                            GenerateCondition(binaryExp.Right);
                        }
                        else
                        {
                            AddSqlParameters(new SqlParameter($@"@{ParameterStack.Pop()}", ((ConstantExpression)binaryExp.Right).Value));
                        }
                        break;
                    }
                case ExpressionType.GreaterThan:
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    break;
                case ExpressionType.Invoke:
                    break;
                case ExpressionType.Lambda:
                    {
                        var lamdbaExp = (LambdaExpression)exp;
                        GenerateCondition((BinaryExpression)lamdbaExp.Body);
                        break;
                    }
                case ExpressionType.LessThan:
                    break;
                case ExpressionType.LessThanOrEqual:
                    break;
                case ExpressionType.MemberAccess:
                    {
                        var memberExp = (MemberExpression)exp;
                        if(memberExp.Expression.NodeType == ExpressionType.Parameter)
                        {
                            var memberName = memberExp.Member.Name;
                            AppendField($@"{memberName}=@{memberName}");
                            ParameterStack.Push(memberName);
                        }
                        else
                        {
                            var parameterName = Guid.NewGuid().ToString().Replace("-", "");
                            switch(memberExp.Type.Name.ToLower())
                            {
                                case "int32":
                                    {
                                        var getter = Expression.Lambda<Func<Int32>>(memberExp).Compile();
                                        AddSqlParameters(new SqlParameter($@"@{ParameterStack.Pop()}", getter()));
                                        break;
                                    }
                                case "string":
                                    {
                                        var getter = Expression.Lambda<Func<String>>(memberExp).Compile();
                                        AddSqlParameters(new SqlParameter($@"@{ParameterStack.Pop()}", getter()));
                                        break;
                                    }
                                default:
                                    break;
                            }
                        }
                        break;
                    }
                case ExpressionType.Not:
                    break;
                case ExpressionType.NotEqual:
                    break;
                case ExpressionType.Or:
                    break;
                case ExpressionType.OrElse:
                    {
                        var binaryExp = (BinaryExpression)exp;
                        GenerateCondition(binaryExp.Left);
                        Or();
                        GenerateCondition(binaryExp.Right);
                        break;
                    }
                case ExpressionType.Parameter:
                    {
                        var parameterExp = (ParameterExpression)exp;
                        break;
                    }
                case ExpressionType.IsTrue:
                    break;
                case ExpressionType.IsFalse:
                    break;
                default:
                    break;
            }
        }
    }
}
