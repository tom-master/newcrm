using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace NewCRM.Repository.StorageProvider
{
    public class DataStore : IDisposable
    {
        private SqlConnection _connection;
        private SqlTransaction _dataTransaction;

        public DataStore(string connectionName = default(String))
        {
            _connection = connectionName == default(String) ? new SqlConnection(ConfigurationManager.AppSettings["NewCrm"]) : new SqlConnection(ConfigurationManager.AppSettings[connectionName]);
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
            if(UseTransaction)
            {
                if(_dataTransaction == null)
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
            if(UseTransaction)
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
            if(UseTransaction)
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
            if(_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        public virtual int SqlExecute(string sqlStr, CommandType commandType = CommandType.Text)
        {
            Open();
            using(SqlCommand cmd = _connection.CreateCommand())
            {
                if(UseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction();
                }
                cmd.CommandType = commandType;
                cmd.CommandText = sqlStr;
                return cmd.ExecuteNonQuery();
            }
        }

        public virtual object SqlScalar(string sqlStr, CommandType commandType = CommandType.Text)
        {
            Open();
            using(SqlCommand cmd = _connection.CreateCommand())
            {
                if(UseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction();
                }
                cmd.CommandType = commandType;
                cmd.CommandText = sqlStr;
                object obj = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return obj;
            }
        }


        public DataTable SqlGetDataTable(string sqlStr, CommandType commandType = CommandType.Text)
        {
            Open();
            using(SqlCommand cmd = _connection.CreateCommand())
            {
                if(UseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction();
                }
                cmd.CommandType = commandType;
                cmd.CommandText = sqlStr;
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable tempDt = new DataTable("tmpDt");
                tempDt.Load(dr, LoadOption.Upsert);
                return tempDt;
            }
        }

        public SqlDataReader SqlGetDataReader(string sqlStr, CommandType commandType = CommandType.Text)
        {
            Open();
            using(SqlCommand cmd = _connection.CreateCommand())
            {
                if(UseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction();
                }
                cmd.CommandType = commandType;
                cmd.CommandText = sqlStr;
                return cmd.ExecuteReader();
            }
        }

        public SqlDataReader SqlGetDataReader(string sqlStr, CommandBehavior behavior, CommandType commandType = CommandType.Text)
        {
            Open();
            using(SqlCommand cmd = _connection.CreateCommand())
            {
                if(UseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction();
                }
                cmd.CommandType = commandType;
                cmd.CommandText = sqlStr;
                return cmd.ExecuteReader(behavior);
            }
        }

        #region 执行带参数的sql语句

        public int SqlExecute(string sqlStr, List<SqlParameter> parameters, CommandType commandType = CommandType.Text)
        {
            Open();
            using(SqlCommand cmd = _connection.CreateCommand())
            {
                if(UseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction();
                }
                cmd.CommandType = commandType;
                cmd.CommandText = sqlStr;
                if(parameters.Any())
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }
                int count = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return count;
            }
        }

        public DataTable SqlGetDataTable(string sqlStr, List<SqlParameter> parameters, CommandType commandType = CommandType.Text)
        {
            Open();
            using(SqlCommand cmd = _connection.CreateCommand())
            {
                if(UseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction();
                }
                cmd.CommandType = commandType;
                cmd.CommandText = sqlStr;
                if(parameters.Any())
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }

                IDataReader dr = cmd.ExecuteReader();
                DataTable tmpDt = new DataTable("tmpDt");
                tmpDt.Load(dr, LoadOption.Upsert);
                return tmpDt;
            }
        }

        public object SqlScalar(string sqlStr, List<SqlParameter> parameters, CommandType commandType = CommandType.Text)
        {
            Open();
            using(SqlCommand cmd = _connection.CreateCommand())
            {
                if(UseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction();
                }
                cmd.CommandType = commandType;
                cmd.CommandText = sqlStr;
                //参数化
                if(parameters.Any())
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }

                object obj = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return obj;
            }
        }

        public SqlDataReader SqlGetDataReader(string sqlStr, List<SqlParameter> parameters, CommandType commandType = CommandType.Text)
        {
            Open();
            using(SqlCommand cmd = _connection.CreateCommand())
            {
                if(UseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction();
                }
                cmd.CommandType = commandType;
                cmd.CommandText = sqlStr;
                //参数化
                if(parameters.Any())
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }
                var dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return dr;
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
            if(!disposed)
            {
                if(!disposing)
                    return;

                if(_connection != null)
                {
                    if(_connection.State != ConnectionState.Closed)
                    {
                        _connection.Close();
                    }
                    _connection.Dispose();
                    _connection = null;
                }
                disposed = true;
            }
        }
        #endregion

    }
}
