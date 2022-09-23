using System;
using System.Data;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 数据库仓储接口
    /// </summary>
    /// <typeparam name="T">数据库表对象</typeparam>
    public partial interface IRepository<T>: IDisposable
    {
        /// <summary>
        /// 设置或获取从库连接字符串的Key（默认为Conn_ReadOnly）
        /// </summary>
        string ReadonlyConnKey { get; set; }

        /// <summary>
        /// 获取从库的开启状态
        /// </summary>
        bool UseReadonly { get; }

        /// <summary>
        /// 获取连接对象
        /// </summary>
        /// <param name="readOnly">是否只读对象</param>
        /// <returns>数据库连接对象</returns>
        IDbConnection GetConnection(bool readOnly);
        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parms">参数</param>
        /// <returns>影响行数</returns>
        int Execute(string sql, object parms);

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parms">参数</param>
        /// <param name="tran">单元事务</param>
        void Execute(string sql, object parms, IUnitTransaction tran);
    }
}
