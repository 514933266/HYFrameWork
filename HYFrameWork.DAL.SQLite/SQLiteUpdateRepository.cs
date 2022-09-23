using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Dapper;
using HYFrameWork.Core;

namespace HYFrameWork.DAL.SQLite
{
   partial class SQLiteRepository<T> : IRepository<T>
    {
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>影响行数</returns>
        public int Update(T entity)
        {
            var cmd = SqlBuilder<T>.BuildUpdateCommand(entity);
            return DbUpdate(cmd, null);
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="tran">单元事务</param>
        public void Update(T entity, IUnitTransaction tran)
        {
            var cmd = SqlBuilder<T>.BuildUpdateCommand(entity);
            ((UnitTransaction)tran).Register(t => DbUpdate(cmd, t), _conn);
        }
        /// <summary>
        /// 按条件更新实体
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="updater">更新字段 例：u => new User{ Age = 31, IsActive = true }</param>
        /// <returns>影响行数</returns>
        public int Update(Expression<Func<T, bool>> predicate,Expression<Func<T, T>> updater)
        {
            var cmd = SqlBuilder<T>.BuildUpdateCommand(predicate, updater);
            return DbUpdate(cmd, null);
        }
        /// <summary>
        /// 按条件更新实体
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="updater">更新字段 例：u => new User{ Age = 31, IsActive = true }</param>
        /// <param name="tran">单元事务</param>
        public void Update(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updater, IUnitTransaction tran)
        {
            var cmd = SqlBuilder<T>.BuildUpdateCommand(predicate, updater);
            ((UnitTransaction)tran).Register(t => DbUpdate(cmd, t), _conn);
        }

        private int DbUpdate(SqlCommand cmd, IDbTransaction tran)
        {
            return _conn.Execute(cmd.Sql, cmd.Parameters, tran);
        }
        /// <summary>
        /// 更新实体并返回新行（随机）
        /// </summary>
        /// <typeparam name="TResult">返回对象类型</typeparam>
        /// <param name="updater">更新选择器</param>
        /// <param name="predicate">更新条件</param>
        /// <param name="selector">查询选择器</param>
        /// <param name="top">更新数据量</param>
        /// <param name="isInserted">True返回更新后的数据，False返回更新前的数据</param>
        [Obsolete]
        public List<TResult> UpdateSelect<TResult>(
          Expression<Func<T, bool>> predicate,
          Expression<Func<T, T>> updater,
          Expression<Func<T, TResult>> selector,
          int top,
          bool isInserted = true)
        {
            throw new NotSupportedException("SQLite does not support this method ! ");
        }

    }
}
