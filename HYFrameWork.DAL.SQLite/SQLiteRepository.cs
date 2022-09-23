using System;
using System.Data;
using HYFrameWork.Core;
using Dapper;

namespace HYFrameWork.DAL.SQLite
{
    /// <summary>
    /// Sqlite仓储
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public partial class SQLiteRepository<T> : IRepository<T>
    {
        #region 字段、属性
        private readonly IDbConnection _conn;
        private readonly IDbConnection _connReadonly;//只读连接
        private string _readonlyConnKey =  "Conn_ReadOnly";

        public string ReadonlyConnKey
        {
            get
            {
                return _readonlyConnKey;
            }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    _readonlyConnKey = value;
                }
            }
        }
        public bool UseReadonly
        {
            get
            {
                return !_readonlyConnKey.ValueOfConnectionString().IsNullOrEmpty();
            }
        }
        #endregion

        #region 构造函数
        public SQLiteRepository(IDbConnection conn)
        {
            _conn = conn;
        }

        public SQLiteRepository(Func<string, IDbConnection> aquire)
        {
            _conn = aquire(typeof(T).Namespace);
            if (UseReadonly)
            {
                _connReadonly = aquire(ReadonlyConnKey);
            }
        }

        public IDbConnection GetConnection(bool readOnly)
        {
            if (UseReadonly && readOnly && _connReadonly != null)
            {
                return _connReadonly;
            }
            return _conn;
        }
        #endregion

        #region  执行 SQL 语句

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parms">参数</param>
        /// <returns>影响行数</returns>
        public int Execute(string sql, object parms)
        {
            return DbExecute(sql, parms, null);
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parms">参数</param>
        /// <param name="tran">单元事务</param>
        public void Execute(string sql, object parms, IUnitTransaction tran)
        {
            ((UnitTransaction)tran).Register(t => DbExecute(sql, parms, t), _conn);
        }
        private int DbExecute(string sql, object parms, IDbTransaction tran)
        {
            return _conn.Execute(sql, parms, tran);
        }

        #endregion
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            if (_conn != null) _conn.Dispose();
            if (_connReadonly != null) _connReadonly.Dispose();
        }
    }
}
