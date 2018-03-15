using System;
using System.Collections.Generic;
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
    }

    class Program
    {

        static void Main(string[] args)
        {
            var accountRoles = new List<AccountRole>
            {
                new AccountRole(1,1),
                new AccountRole(2,2)
            };
            var account = new Account();
            account.ModifyLoginPassword("xiaofan123");
            account.Disable();
            var cons = 1;
            var str = "123";
            Modify(account, a => a.Id == cons && a.Name == "xiaofan" || a.AccountFace == str);
        }

        public static void Modify<TModel>(TModel model, Expression<Func<TModel, Boolean>> where) where TModel : class, new()
        {
            var modelType = model.GetType();
            var method = modelType.GetMethod("GetPropertyValues").Invoke(model, null);
            var returnValue = method as IDictionary<String, Object>;
            if(returnValue != null)
            {
                var sqlBuilder = new StringBuilder();
                sqlBuilder.Append($@"UPDATE {modelType.Name} SET ");
                sqlBuilder.Append($@"{String.Join(",", returnValue.Keys.Select(key => $@"{key}=@{key}"))}");
                sqlBuilder.Append($@" WHERE 1=1 ");
                GetWhereCond(where, ref sqlBuilder);
                var sql = sqlBuilder.ToString();
            }

        }

        private static void GetWhereCond(Expression exp, ref StringBuilder where)
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
                        GetWhereCond(binaryExp.Left, ref where);
                        GetWhereCond(binaryExp.Right, ref where);
                        break;
                    }
                case ExpressionType.ArrayLength:
                    break;
                case ExpressionType.ArrayIndex:
                    break;
                case ExpressionType.Call:
                    break;
                case ExpressionType.Coalesce:
                    break;
                case ExpressionType.Conditional:
                    break;
                case ExpressionType.Constant:
                    {
                        var constantExp = (ConstantExpression)exp;
                        where.Append($@" {constantExp.Value}");
                        break;
                    }
                case ExpressionType.Convert:
                    break;
                case ExpressionType.ConvertChecked:
                    break;
                case ExpressionType.Divide:
                    break;
                case ExpressionType.Equal:
                    {
                        var binaryExp = (BinaryExpression)exp;
                        GetWhereCond(binaryExp.Left, ref where);
                        GetWhereCond(binaryExp.Right, ref where);
                        break;
                    }
                case ExpressionType.ExclusiveOr:
                    break;
                case ExpressionType.GreaterThan:
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    break;
                case ExpressionType.Invoke:
                    break;
                case ExpressionType.Lambda:
                    {
                        var lamdbaExp = (LambdaExpression)exp;
                        GetWhereCond((BinaryExpression)lamdbaExp.Body, ref where);
                        break;
                    }
                case ExpressionType.LeftShift:
                    break;
                case ExpressionType.LessThan:
                    break;
                case ExpressionType.LessThanOrEqual:
                    break;
                case ExpressionType.ListInit:
                    break;
                case ExpressionType.MemberAccess:
                    {
                        var memberExp = (MemberExpression)exp;
                        if(memberExp.Expression.NodeType == ExpressionType.Parameter)
                        {
                            where.Append($@" AND {memberExp.Member.Name}=");
                        }
                        else
                        {
                            switch(memberExp.Type.Name.ToLower())
                            {
                                case "int32":
                                    {
                                        var getter = Expression.Lambda<Func<Int32>>(memberExp).Compile();
                                        where.Append($@" {getter()}");
                                        break;
                                    }
                                case "string":
                                    {
                                        var getter = Expression.Lambda<Func<String>>(memberExp).Compile();
                                        where.Append($@" {getter()}");
                                        break;
                                    }

                                default:
                                    break;
                            }
                        }
                        break;
                    }
                case ExpressionType.MemberInit:
                    break;
                case ExpressionType.Modulo:
                    break;
                case ExpressionType.Multiply:
                    break;
                case ExpressionType.MultiplyChecked:
                    break;
                case ExpressionType.Negate:
                    break;
                case ExpressionType.UnaryPlus:
                    break;
                case ExpressionType.NegateChecked:
                    break;
                case ExpressionType.New:
                    break;
                case ExpressionType.NewArrayInit:
                    break;
                case ExpressionType.NewArrayBounds:
                    break;
                case ExpressionType.Not:
                    break;
                case ExpressionType.NotEqual:
                    break;
                case ExpressionType.Or:
                    break;
                case ExpressionType.OrElse:
                    {
                        var binaryExp = (BinaryExpression)exp;
                        GetWhereCond(binaryExp.Left, ref where);
                        GetWhereCond(binaryExp.Right, ref where);
                        break;
                    }
                case ExpressionType.Parameter:
                    {
                        var parameterExp = (ParameterExpression)exp;
                        break;
                    }
                case ExpressionType.Power:
                    break;
                case ExpressionType.Quote:
                    break;
                case ExpressionType.RightShift:
                    break;
                case ExpressionType.Subtract:
                    break;
                case ExpressionType.SubtractChecked:
                    break;
                case ExpressionType.TypeAs:
                    break;
                case ExpressionType.TypeIs:
                    break;
                case ExpressionType.Assign:
                    break;
                case ExpressionType.Block:
                    break;
                case ExpressionType.DebugInfo:
                    break;
                case ExpressionType.Decrement:
                    break;
                case ExpressionType.Dynamic:
                    break;
                case ExpressionType.Default:
                    break;
                case ExpressionType.Extension:
                    break;
                case ExpressionType.Goto:
                    break;
                case ExpressionType.Increment:
                    break;
                case ExpressionType.Index:
                    break;
                case ExpressionType.Label:
                    break;
                case ExpressionType.RuntimeVariables:
                    break;
                case ExpressionType.Loop:
                    break;
                case ExpressionType.Switch:
                    break;
                case ExpressionType.Throw:
                    break;
                case ExpressionType.Try:
                    break;
                case ExpressionType.Unbox:
                    break;
                case ExpressionType.AddAssign:
                    break;
                case ExpressionType.AndAssign:
                    break;
                case ExpressionType.DivideAssign:
                    break;
                case ExpressionType.ExclusiveOrAssign:
                    break;
                case ExpressionType.LeftShiftAssign:
                    break;
                case ExpressionType.ModuloAssign:
                    break;
                case ExpressionType.MultiplyAssign:
                    break;
                case ExpressionType.OrAssign:
                    break;
                case ExpressionType.PowerAssign:
                    break;
                case ExpressionType.RightShiftAssign:
                    break;
                case ExpressionType.SubtractAssign:
                    break;
                case ExpressionType.AddAssignChecked:
                    break;
                case ExpressionType.MultiplyAssignChecked:
                    break;
                case ExpressionType.SubtractAssignChecked:
                    break;
                case ExpressionType.PreIncrementAssign:
                    break;
                case ExpressionType.PreDecrementAssign:
                    break;
                case ExpressionType.PostIncrementAssign:
                    break;
                case ExpressionType.PostDecrementAssign:
                    break;
                case ExpressionType.TypeEqual:
                    break;
                case ExpressionType.OnesComplement:
                    break;
                case ExpressionType.IsTrue:
                    break;
                case ExpressionType.IsFalse:
                    break;
                default:
                    break;
            }
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
