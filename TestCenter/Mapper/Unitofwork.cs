using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace ConsoleApplication1.Mapper
{
    public class Unitofwork : IUnitofwork
    {
        private IDictionary<String, IList<SqlParameter>> _addStatements;
        private IDictionary<String, IList<SqlParameter>> _modifyStatements;

        private static readonly Object _async;

        private Boolean _isVerifyModel;

        static Unitofwork()
        {
            _async = new Object();
        }

        public Unitofwork(Boolean isVerifyModel = false)
        {
            _addStatements = new Dictionary<String, IList<SqlParameter>>();
            _modifyStatements = new Dictionary<String, IList<SqlParameter>>();
            _isVerifyModel = isVerifyModel;
        }

        public void RegisterAdd<TModel>(TModel model) where TModel : class, new()
        {
            if(model == null)
            {
                throw new Exception();
            }

            SqlBuilder<TModel> addBuilder = new InsertBuilder<TModel>(model, _isVerifyModel);
            AddToDictionary(addBuilder, _addStatements);
        }

        public void RegisterModify<TModel>(TModel model, Expression<Func<TModel, Boolean>> where = null) where TModel : class, new()
        {
            if(model == null)
            {
                throw new Exception();
            }

            SqlBuilder<TModel> modifyBuilder = new UpdateBuilder<TModel>(model, where, _isVerifyModel);
            AddToDictionary(modifyBuilder, _modifyStatements);
        }

        public void Commit()
        {
            using(var dataStore = new DataStore())
            {
                dataStore.OpenTransaction();
                try
                {
                    foreach(var addItem in _addStatements)
                    {
                        var successCount = dataStore.SqlExecute(addItem.Key, addItem.Value);
                        if(successCount <= 0)
                        {
                            throw new Exception(" insert fail");
                        }
                    }

                    foreach(var modifyItem in _modifyStatements)
                    {
                        var successCount = dataStore.SqlExecute(modifyItem.Key, modifyItem.Value);
                        if(successCount <= 0)
                        {
                            throw new Exception(" modify fail");
                        }
                    }

                    dataStore.Commit();
                }
                catch(Exception ex)
                {
                    dataStore.Rollback();
                }
            }
        }


        private void AddToDictionary<TModel>(SqlBuilder<TModel> sqlBuilder, IDictionary<String, IList<SqlParameter>> statements) where TModel : class, new()
        {
            var sql = sqlBuilder.ParseToSql();
            var parameters = sqlBuilder.GetParameters();

            lock(_async)
            {
                if(!_addStatements.Keys.Contains(sql))
                {
                    statements.Add(new KeyValuePair<String, IList<SqlParameter>>(sql, parameters));
                }
                else
                {
                    statements[sql] = parameters;
                }
            }
        }
    }
}
