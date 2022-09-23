using Dapper;
using HYFrameWork.Core;
using System;
using System.Data;
using System.Linq.Expressions;

namespace HYFrameWork.DAL.SQLite
{
    partial class SQLiteRepository<T> : IRepository<T>
    {
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>影响行数</returns>
        public int Delete(T entity)
        {
            var cmd = SqlBuilder<T>.BuildDeleteCommand(entity);
            return DbDelete(cmd, null);
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="tran">单元事务</param>
        public void Delete(T entity, IUnitTransaction tran)
        {
            var cmd = SqlBuilder<T>.BuildDeleteCommand(entity);
            ((UnitTransaction)tran).Register(t => DbDelete(cmd, t), _conn);
        }
        /// <summary>
        /// 按条件删除实体
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>影响行数</returns>
        public int Delete(Expression<Func<T, bool>> predicate)
        {
            var cmd = SqlBuilder<T>.BuildDeleteCommand(predicate);
            return DbDelete(cmd, null);
        }

        /// <summary>
        /// 按条件删除实体
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="tran">单元事务</param>
        public void Delete(Expression<Func<T, bool>> predicate, IUnitTransaction tran)
        {
            var cmd = SqlBuilder<T>.BuildDeleteCommand(predicate);
            ((UnitTransaction)tran).Register(t => DbDelete(cmd, t), _conn);
        }
        private int DbDelete(SqlCommand cmd, IDbTransaction tran)
        {
            return _conn.Execute(cmd.Sql, cmd.Parameters, tran);
        }
    }
}
