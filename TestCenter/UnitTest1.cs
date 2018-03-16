using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCenter
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

        }

        public static void Modify<TModel>(TModel model, Expression<Func<TModel, Boolean>> where) where TModel : class, new()
        {
            var modelType = model.GetType();
            var method = modelType.GetMethod("GetPropertyValues").Invoke(model, null);
            var returnValue = method as IDictionary<String, Object>;
            if(returnValue != null)
            {
                var sqlBuilder = new SqlBuilder(modelType);
                sqlBuilder.GenerateHeadOfUpdate(returnValue).GenerateCondition(where);
                var sql = sqlBuilder.ToString();
                var sqlParameters = sqlBuilder.GetSqlParameters();
            }
        }

        public static void Insert<TModel>(TModel model) where TModel : class, new()
        {
            var modelType = model.GetType();
            var dicParameters = new Dictionary<String, Object>();
            foreach(var property in modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                dicParameters.Add(property.Name, property.GetValue(model));
            }

            var sqlBuilder = new SqlBuilder(modelType);
            sqlBuilder.GenerateHeadOfInsert(dicParameters);
            var parameters = sqlBuilder.GetSqlParameters();
            var sql = sqlBuilder.ToString();
        }
    }

    public class SqlBuilder
    {
        private StringBuilder _sqlBuilder = new StringBuilder();
        private Type _modelType;
        private Stack<String> _sqlParameterStack;
        private IList<SqlParameter> _sqlParameters;
        public SqlBuilder(Type modelType)
        {
            _modelType = modelType;
            _sqlParameterStack = new Stack<String>();
            _sqlParameters = new List<SqlParameter>();
        }
        public SqlBuilder GenerateHeadOfUpdate(IDictionary<String, Object> dicParameterValues)
        {
            _sqlBuilder.Append($@"UPDATE {_modelType.Name} SET ");
            _sqlBuilder.Append($@"{String.Join(",", dicParameterValues.Keys.Select(key => $@"{key}=@{key}"))}");
            _sqlBuilder.Append($@" WHERE");

            _sqlParameters = dicParameterValues.Select(item => new SqlParameter($@"@{item.Key}", item.Value)).ToList();

            return this;
        }

        public void GenerateHeadOfInsert(IDictionary<String, Object> dicParameterValues)
        {
            _sqlBuilder.Append($@" INSERT dbo.{_modelType.Name} (");
            _sqlBuilder.Append($@"{String.Join(",", dicParameterValues.Keys.Select(key => $@"{key}"))}");
            _sqlBuilder.Append($@") VALUES ({String.Join(",", dicParameterValues.Keys.Select(key => $@"@{key}"))}) ");

            _sqlParameters = dicParameterValues.Select(item => new SqlParameter($@"@{item.Key}", item.Value)).ToList();
        }
         
        public void And()
        {
            _sqlBuilder.Append("AND");
        }

        public void Or()
        {
            _sqlBuilder.Append("OR");
        }

        public void AddField(Object fieldName)
        {
            _sqlBuilder.Append($@" {fieldName} ");
        }

        public void GenerateCondition(Expression exp)
        {
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
                    {
                        break;
                    }
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
                            _sqlParameters.Add(new SqlParameter($@"@{_sqlParameterStack.Pop()}", ((ConstantExpression)binaryExp.Right).Value));
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
                            AddField($@"{memberName}=@{memberName}");
                            _sqlParameterStack.Push(memberName);
                        }
                        else
                        {
                            var parameterName = Guid.NewGuid().ToString().Replace("-", "");
                            switch(memberExp.Type.Name.ToLower())
                            {
                                case "int32":
                                    {
                                        var getter = Expression.Lambda<Func<Int32>>(memberExp).Compile();
                                        _sqlParameters.Add(new SqlParameter($@"@{_sqlParameterStack.Pop()}", getter()));
                                        break;
                                    }
                                case "string":
                                    {
                                        var getter = Expression.Lambda<Func<String>>(memberExp).Compile();
                                        _sqlParameters.Add(new SqlParameter($@"@{_sqlParameterStack.Pop()}", getter()));
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

        public override string ToString()
        {
            return _sqlBuilder.ToString();
        }

        public IList<SqlParameter> GetSqlParameters()
        {
            return _sqlParameters;
        }
    }

    public class AccountRole
    {
        #region private field
        private Int32 _accountId;

        private Int32 _roleId;
        #endregion

        #region public property
        public Int32 AccountId
        {
            get
            {
                return _accountId;
            }
            private set
            {
                _accountId = value;
            }
        }

        public Int32 RoleId
        {
            get
            {
                return _roleId;
            }
            private set
            {
                _roleId = value;
            }
        }

        #endregion

        #region ctor

        public AccountRole(Int32 accountId, Int32 roleId)
        {
            AccountId = accountId;
            RoleId = roleId;
        }

        public AccountRole() { }

        #endregion
    }

    public partial class Account : PropertyMonitor
    {
        private Int32 _id;

        private String _name;

        private String _loginPassword;

        private String _lockScreenPassword;

        private Boolean _isDisable;

        private DateTime _lastLoginTime;

        private Boolean _isOnline;

        private Boolean _isAdmin;

        private IEnumerable<AccountRole> _roles;
         
        public Int32 Id
        {
            get
            {
                return _id;
            }
            private set
            {
                _id = value;
            }
        }

        public String Name
        {
            get
            {
                return _name;
            }
            private set
            {
                _name = value;
            }
        }

        public String LoginPassword
        {
            get
            {
                return _loginPassword;
            }
            private set
            {
                _loginPassword = value;
            }
        }

        public String LockScreenPassword
        {
            get
            {
                return _lockScreenPassword;
            }
            private set
            {
                _lockScreenPassword = value;
            }
        }

        public Boolean IsDisable
        {
            get
            {
                return _isDisable;
            }
            private set
            {
                _isDisable = value;
            }
        }

        public DateTime LastLoginTime
        {
            get
            {
                return _lastLoginTime;
            }
            private set
            {
                _lastLoginTime = value;
            }
        }

        public Boolean IsOnline
        {
            get
            {
                return _isOnline;
            }
            private set
            {
                _isOnline = value;
            }
        }

        public Boolean IsAdmin
        {
            get
            {
                return _isAdmin;
            }
            private set
            {
                _isAdmin = value;
            }
        }

        public String AccountFace { get; set; }

        public IEnumerable<AccountRole> Roles
        {
            get
            {
                return _roles;
            }
            private set
            {
                _roles = value;
            }
        }

        #region ctor

        public Account(String name, String password, IEnumerable<AccountRole> roles)
        {
            Name = name;
            LoginPassword = password;
            IsDisable = false;
            LastLoginTime = DateTime.Now;
            LockScreenPassword = password;
            IsOnline = false;
            IsAdmin = false;
            Roles = roles;
        }

        public Account() { }

        #endregion
    }

    /// <summary>
    /// AccountExtension
    /// </summary>
    public partial class Account
    {
        public Account ModifyLoginPassword(String password)
        {
            if(String.IsNullOrEmpty(password))
            {
                throw new ArgumentException($@"{nameof(LoginPassword)} is null");
            }

            LoginPassword = password;
            OnPropertyChanged(nameof(LoginPassword));
            return this;
        }

        public Account ModifyLockScreenPassword(String password)
        {
            if(String.IsNullOrEmpty(password))
            {
                throw new ArgumentException($@"{nameof(LockScreenPassword)} is null");
            }

            LockScreenPassword = password;
            OnPropertyChanged(nameof(LockScreenPassword));
            return this;
        }

        public Account Enable()
        {
            IsDisable = false;
            OnPropertyChanged(nameof(Enable));
            return this;
        }

        public Account Disable()
        {
            IsDisable = true;
            OnPropertyChanged(nameof(IsDisable));
            return this;
        }

        public Account Online()
        {
            IsOnline = true;
            LastLoginTime = DateTime.Now;
            OnPropertyChanged(nameof(IsOnline), nameof(LastLoginTime));
            return this;
        }

        public Account Offline()
        {
            IsOnline = false;
            OnPropertyChanged(nameof(IsOnline));
            return this;
        }

        public Account ModifyRoles(params Int32[] roleIds)
        {
            if(roleIds.Length == 0)
            {
                return this;
            }

            Roles.ToList().Clear();
            Roles = roleIds.Select(roleId => new AccountRole(Id, roleId));
            OnPropertyChanged(nameof(Roles));
            return this;
        }
    }

    public class PropertyArgs : EventArgs
    {
        public String PropertyName { get; }

        public Object PropertyValue { get; }

        public PropertyArgs(String propertyName, Object propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }
    }

    public abstract class PropertyMonitor
    {
        public delegate void PropertyChangeEventHandler(Object sender, PropertyArgs args);

        public event PropertyChangeEventHandler PropertyChanged;

        private IDictionary<String, Object> _propertyValues = new Dictionary<String, Object>();

        private readonly Object _sync = new Object();

        public PropertyMonitor()
        {
            if(PropertyChanged == null)
            {
                PropertyChanged += PropertyMonitor_PropertyChanged;
            }
        }

        private void PropertyMonitor_PropertyChanged(Object sender, PropertyArgs e)
        {
            lock(_sync)
            {
                var instanceName = $@"{e.PropertyName}";
                if(!_propertyValues.Keys.Contains(instanceName))
                {
                    _propertyValues.Add(new KeyValuePair<String, Object>(instanceName, e.PropertyValue));
                }
                else
                {
                    _propertyValues[instanceName] = e.PropertyValue;
                }
            }
        }

        public void OnPropertyChanged(params String[] propertyNames)
        {
            if(propertyNames.Length == 0)
            {
                return;
            }

            var temp = Interlocked.CompareExchange(ref PropertyChanged, null, null);

            for(int i = 0 ; i < propertyNames.Length ; i++)
            {
                var property = GetType().GetProperty(propertyNames[i], BindingFlags.Instance | BindingFlags.Public);
                temp?.Invoke(this, new PropertyArgs(propertyNames[i], property.GetValue(this)));
            }
        }

        public IDictionary<String, Object> GetPropertyValues()
        {
            return _propertyValues;
        }
    }
}
