using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace NewCRM.Repository
{
    /// <summary>
    /// 数据库访问操作类
    /// </summary>
    public class SqlHelper : IDisposable
    {
        private SqlConnection _connection;
        private IList<SqlParameter> _parameters;
        private SqlTransaction _dataTransaction;

        public SqlHelper(string connectionName)
        {
            _parameters = new List<SqlParameter>();
            _connection = new SqlConnection(ConfigurationManager.AppSettings[connectionName]);
        }

        #region 事务处理


        /// <summary>
        /// 是否使用事务
        /// </summary>
        public bool UseTransaction { get; set; }


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
            else
            {
                throw new Exception("没有启动事务");
            }
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
                if(_dataTransaction != null)
                {
                    _dataTransaction.Rollback();
                }
            }
            else
            {
                throw new Exception("没有启动事务，无法执行事务回滚");
            }
        }

        #endregion

        /// <summary>
        /// 开启数据库连接
        /// </summary>
        protected virtual void Open()
        {
            if(_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        /// <summary>
        /// 执行 SQL 语句并返回受影响的行数。
        /// </summary>
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

        /// <summary>
        /// 执行 SQL 语句，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <returns></returns>
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

        /// <summary>
        /// 执行SQL语句，获得数据表
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <returns></returns>
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

        /// <summary>
        /// 执行SQL语句，返回只进结果集流的读取方法
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <returns></returns>
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

        /// <summary>
        /// 执行SQL语句，返回只进结果集流的读取方法
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 执行带参数的sql语句 返回受影响行数
        /// </summary>
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

        /// <summary>
        /// 执行SQL语句，返回只进结果集流的读取方法
        /// </summary>
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
                    _parameters = null;
                }
                disposed = true;
            }
        }
        #endregion

    }
}
