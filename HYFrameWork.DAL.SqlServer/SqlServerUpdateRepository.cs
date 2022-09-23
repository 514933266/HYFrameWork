using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Dapper;
using HYFrameWork.Core;
using System.Linq;

namespace HYFrameWork.DAL.SqlServer
{
   partial class SqlServerRepository<T> : IRepository<T>
    {
        public int Update(T entity)
        {
            var cmd = SqlBuilder<T>.BuildUpdateCommand(entity);
            return DbUpdate(cmd, null);
        }
        public void Update(T entity, IUnitTransaction tran)
        {
            var cmd = SqlBuilder<T>.BuildUpdateCommand(entity);
            ((UnitTransaction)tran).Register(t => DbUpdate(cmd, t), _conn);
        }
        public int Update(Expression<Func<T, bool>> predicate,Expression<Func<T, T>> updater)
        {
            var cmd = SqlBuilder<T>.BuildUpdateCommand(predicate, updater);
            return DbUpdate(cmd, null);
        }
        public void Update(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updater, IUnitTransaction tran)
        {
            var cmd = SqlBuilder<T>.BuildUpdateCommand(predicate, updater);
            ((UnitTransaction)tran).Register(t => DbUpdate(cmd, t), _conn);
        }

        private int DbUpdate(SqlCommand cmd, IDbTransaction tran)
        {
            return _conn.Execute(cmd.Sql, cmd.Parameters, tran);
        }
        public List<TResult> UpdateSelect<TResult>(
          Expression<Func<T, bool>> predicate,
          Expression<Func<T, T>> updater,
          Expression<Func<T, TResult>> selector,
          int top,
          bool isInserted = true)
        {
            var cmd = SqlBuilder<T>.BuildUpdateSelectCommand(predicate, updater, selector, top, isInserted);
            return _conn.Query<TResult>(cmd.Sql, cmd.Parameters).ToList();
        }

    }
}
