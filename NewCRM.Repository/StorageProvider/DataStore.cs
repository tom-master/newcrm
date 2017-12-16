using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewLib.Security;

namespace NewCRM.Repository.StorageProvider
{
    public class DataStore : IDisposable
    {
        private SqlConnection _connection;
        private SqlTransaction _dataTransaction;

        public DataStore(string connectionName = default(String))
        {
            var conn = SensitiveDataSafetyProvider.Decrypt(ConfigurationManager.ConnectionStrings[connectionName ?? "NewCrm"].ToString());
            _connection = new SqlConnection(conn);
        }

        #region 事务处理

        public void OpenTransaction()
        {
            UseTransaction = true;
        }

        /// <summary>
        /// 是否使用事务
        /// </summary>
        private bool UseTransaction { get; set; }

        /// <summary>
        /// 获得当前事务
        /// </summary>
        /// <returns></returns>
        protected SqlTransaction GetNonceTransaction()
        {
            if (UseTransaction)
            {
                if (_dataTransaction == null)
                {
                    UseTransaction = true;
                    _dataTransaction = _connection.BeginTransaction();
                }
                return _dataTransaction;
            }
            throw new Exception("没有启动事务");
        }

        /// <summary>
        /// 事务提交
        /// </summary>
        public virtual void Commit()
        {
            if (UseTransaction)
            {
                _dataTransaction.Commit();
            }
            else
            {
                throw new Exception("没有启动事务，无法执行事务提交");
            }
        }
        /// <summary>
        /// 事务回滚
        /// </summary>
        public virtual void Rollback()
        {
            if (UseTransaction)
            {
                _dataTransaction?.Rollback();
            }
            else
            {
                throw new Exception("没有启动事务，无法执行事务回滚");
            }
        }

        #endregion

        protected virtual void Open()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        public virtual TValue FindSingleValue<TValue>(string sqlStr, CommandType commandType = CommandType.Text)
        {
            Open();
            try
            {
                using (SqlCommand cmd = _connection.CreateCommand())
                {
                    if (UseTransaction)
                    {
                        cmd.Transaction = GetNonceTransaction();
                    }
                    cmd.CommandType = commandType;
                    cmd.CommandText = sqlStr;
                    TValue obj = (TValue)cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return obj;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual TModel FindOne<TModel>(string sqlStr, CommandType commandType = CommandType.Text) where TModel : class, new()
        {
            return Find<TModel>(sqlStr, commandType).FirstOrDefault();
        }

        public virtual List<TModel> Find<TModel>(string sqlStr, CommandType commandType = CommandType.Text) where TModel : class, new()
        {
            Open();
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                if (UseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction();
                }
                cmd.CommandType = commandType;
                cmd.CommandText = sqlStr;
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dataTable = new DataTable("tmpDt");
                dataTable.Load(dr, LoadOption.Upsert);
                var result = dataTable.AsList<TModel>().ToList();
                return result;
            }
        }

        public virtual SqlDataReader SqlGetDataReader(string sqlStr, CommandType commandType = CommandType.Text)
        {
            Open();
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                if (UseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction();
                }
                cmd.CommandType = commandType;
                cmd.CommandText = sqlStr;
                return cmd.ExecuteReader();
            }
        }

        public virtual SqlDataReader SqlGetDataReader(string sqlStr, CommandBehavior behavior, CommandType commandType = CommandType.Text)
        {
            Open();
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                if (UseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction();
                }
                cmd.CommandType = commandType;
                cmd.CommandText = sqlStr;
                return cmd.ExecuteReader(behavior);
            }
        }

        #region 执行带参数的sql语句

        public virtual int SqlExecute(string sqlStr, CommandType commandType = CommandType.Text)
        {
            Open();
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                if (UseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction();
                }
                cmd.CommandType = commandType;
                cmd.CommandText = sqlStr;
                return cmd.ExecuteNonQuery();
            }
        }

        public virtual int SqlExecute(string sqlStr, IEnumerable<SqlParameter> parameters, CommandType commandType = CommandType.Text)
        {
            Open();
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                if (UseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction();
                }
                cmd.CommandType = commandType;
                cmd.CommandText = sqlStr;
                if (parameters.Any())
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }
                int count = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return count;
            }
        }

        public SqlDataReader SqlGetDataReader(string sqlStr, IEnumerable<SqlParameter> parameters, CommandType commandType = CommandType.Text)
        {
            Open();
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                if (UseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction();
                }
                cmd.CommandType = commandType;
                cmd.CommandText = sqlStr;
                //参数化
                if (parameters.Any())
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }
                var dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return dr;
            }
        }

        public virtual TValue FindSingleValue<TValue>(string sqlStr, IEnumerable<SqlParameter> parameters, CommandType commandType = CommandType.Text)
        {
            Open();
            try
            {
                using (SqlCommand cmd = _connection.CreateCommand())
                {
                    if (UseTransaction)
                    {
                        cmd.Transaction = GetNonceTransaction();
                    }
                    cmd.CommandType = commandType;
                    cmd.CommandText = sqlStr;
                    //参数化
                    if (parameters.Any())
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    TValue obj = (TValue)cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return obj;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual TModel FindOne<TModel>(string sqlStr, IEnumerable<SqlParameter> parameters, CommandType commandType = CommandType.Text) where TModel : class, new()
        {
            return Find<TModel>(sqlStr, parameters, commandType).FirstOrDefault();
        }

        public virtual List<TModel> Find<TModel>(string sqlStr, IEnumerable<SqlParameter> parameters, CommandType commandType = CommandType.Text) where TModel : class, new()
        {
            Open();
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                if (UseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction();
                }
                cmd.CommandType = commandType;
                cmd.CommandText = sqlStr;
                if (parameters.Any())
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }

                IDataReader dr = cmd.ExecuteReader();
                var tmpDt = new DataTable("tmpDt");
                tmpDt.Load(dr, LoadOption.Upsert);
                return tmpDt.AsList<TModel>().ToList();
            }
        }
        #endregion

        #region IDisposable 成员

        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (!disposing)
                    return;

                if (_connection != null)
                {
                    if (_connection.State != ConnectionState.Closed)
                    {
                        _connection.Close();
                    }
                    //_connection.Dispose();
                    //_connection = null;
                }
                disposed = true;
            }
        }


        #endregion


        #region 缓存事件
        #endregion

    }
}
