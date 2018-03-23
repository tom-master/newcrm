using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using ConsoleApplication1.Mapper.MapperExtension;

namespace ConsoleApplication1.Mapper
{
    public abstract class SqlBuilder<TModel> where TModel : class, new()
    {
        private StringBuilder _sqlBuilder = new StringBuilder();
        private Type _modelType;
        private Stack<String> _parameterStack;

        private TModel _modelInstance;
        private IList<SqlParameter> _parameters;



        protected SqlBuilder(TModel model)
        {
            if(model == null)
            {
                throw new Exception();
            }

            _modelType = model.GetType();
            _modelInstance = model;

            _parameterStack = new Stack<String>();
            _parameters = new List<SqlParameter>();
        }

        public abstract String ParseToSql();

        public IList<SqlParameter> GetParameters()
        {
            return Parameters;
        }

        protected StringBuilder Builder
        {
            get { return _sqlBuilder; }
        }

        protected Type ModelType
        {
            get { return _modelType; }
        }

        protected TModel ModelInstance
        {
            get { return _modelInstance; }
        }

        protected Stack<String> ParameterStack
        {
            get { return _parameterStack; }
        }

        protected IList<SqlParameter> Parameters
        {
            get { return _parameters; }
        }

        protected virtual IEnumerable<String> GetFields()
        {
            foreach(var item in ModelType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(w => w.PropertyType.Name != "IList`1" && w.CustomAttributes.Count() != 0))
            {
                yield return item.Name;
            }
        }

        protected void And()
        {
            Builder.Append("AND");
        }

        protected void Or()
        {
            Builder.Append("OR");
        }

        protected void Append(String appendString)
        {
            if(String.IsNullOrEmpty(appendString))
            {
                return;
            }

            Builder.Append(appendString);
        }

        public override String ToString()
        {
            return Builder.ToString();
        }

        protected void VerifyModel()
        {
            var propertys = ModelType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach(var propertyItem in propertys)
            {
                if(propertyItem.CustomAttributes.Count() == 0)
                {
                    continue;
                }

                var requiredAttribute = GetPropertyAttribute<RequiredAttribute>(propertyItem);
                if(requiredAttribute != null)
                {
                    VerifyPropertyValue(requiredAttribute, GetPropertyValue(propertyItem));
                }

                var defaultValueAttribute = GetPropertyAttribute<DefaultValueAttribute>(propertyItem);
                if(defaultValueAttribute != null)
                {
                    VerifyPropertyValue(defaultValueAttribute, GetPropertyValue(propertyItem));
                    propertyItem.SetValue(ModelInstance, defaultValueAttribute.Value);
                }

                var inputRangeAttribute = GetPropertyAttribute<InputRangeAttribute>(propertyItem);
                if(inputRangeAttribute != null)
                {
                    VerifyPropertyValue(inputRangeAttribute, GetPropertyValue(propertyItem));
                }
            }
        }

        private TAttribute GetPropertyAttribute<TAttribute>(PropertyInfo propertyInfo) where TAttribute : Attribute
        {
            var attributeType = typeof(TAttribute);
            var attribute = propertyInfo.GetCustomAttribute(attributeType);
            if(attribute == null)
            {
                return null;
            }

            var customAttribute = attribute as TAttribute;
            if(customAttribute == null)
            {
                throw new Exception($@"{customAttribute.GetType().Name} convert fail");
            }

            return customAttribute;
        }

        private Object GetPropertyValue(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetValue(ModelInstance);
        }

        private void VerifyPropertyValue(Validate validate, Object value)
        {
            if(!validate.IsValidate(value))
            {
                throw new Exception();
            }
        }
    }
}
