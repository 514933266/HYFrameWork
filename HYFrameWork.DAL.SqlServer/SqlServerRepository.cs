using System;
using System.Data;
using HYFrameWork.Core;
using Dapper;

namespace HYFrameWork.DAL.SqlServer
{
    public partial class SqlServerRepository<T> : IRepository<T>
    {
        #region 字段、属性
        private readonly IDbConnection _conn;
        private readonly IDbConnection _connReadonly;//只读连接
        private string _readonlyConnKey = "Conn_ReadOnly";
        /// <summary>
        /// 数据主键字段设计模式(该值会影响生成的Sql)
        /// </summary>
        public PrimaryKeyMode KeyMode { get; set; } = PrimaryKeyMode.IDENTITY;
        /// <summary>
        /// 只读连接字符串
        /// </summary>
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
        /// <summary>
        /// 是否使用只读连接
        /// </summary>
        public bool UseReadonly
        {
            get
            {
                return !_readonlyConnKey.ValueOfConnectionString().IsNullOrEmpty();
            }
        }
        
        #endregion

        #region 构造函数
        /// <summary>
        /// 基本仓储（Identity主键设计模式）
        /// </summary>
        /// <param name="conn"></param>
        public SqlServerRepository(IDbConnection conn)
        {
            _conn = conn;
        }

        /// <summary>
        /// GUID|UUID仓储（GUID或UUID主键设计模式）
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="keymode"></param>
        public SqlServerRepository(IDbConnection conn, PrimaryKeyMode keymode)
        {
            _conn = conn;
            KeyMode = keymode;
        }

        /// <summary>
        /// 获取连接对象
        /// </summary>
        /// <param name="readOnly">是否获取从库连接</param>
        /// <returns>连接对象</returns>
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

        public int Execute(string sql, object parms)
        {
            return DbExecute(sql, parms, null);
        }

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
