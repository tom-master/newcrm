using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleApplication1.Mapper
{
    public abstract class SqlBuilder<TModel> where TModel : class, new()
    {
        private StringBuilder _sqlBuilder = new StringBuilder();
        private Type _modelType;
        private TModel _modelInstance;
        private Stack<String> _sqlParameterStack;
        private IList<SqlParameter> _sqlParameters;

        protected SqlBuilder(TModel model)
        {
            _modelType = model.GetType();
            _modelInstance = model;

            _sqlParameterStack = new Stack<String>();
            _sqlParameters = new List<SqlParameter>();
        }

        protected Type ModelType
        {
            get { return _modelType; }
        }

        public IList<SqlParameter> SqlParameters
        {
            get { return _sqlParameters; }
        }

        protected TModel ModelInstance
        {
            get { return _modelInstance; }
        }

        protected Stack<String> ParameterStack
        {
            get { return _sqlParameterStack; }
        }

        protected virtual void AddSqlParameters(SqlParameter parameter)
        {
            SqlParameters.Add(parameter);
        }

        public abstract void GenerateSqlHead();

        protected virtual IEnumerable<String> GetFields()
        {
            foreach(var item in ModelType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(w => w.PropertyType.Name != "IList`1"))
            {
                yield return item.Name;
            }
        }

        protected void And()
        {
            _sqlBuilder.Append("AND");
        }

        protected void Or()
        {
            _sqlBuilder.Append("OR");
        }

        protected void Append(String appendString)
        {
            _sqlBuilder.Append(appendString);
        }

        public override string ToString()
        {
            return _sqlBuilder.ToString();
        }

    }
}
