using Dapper;
using HYFrameWork.Core;
using System;
using System.Data;
using System.Linq.Expressions;

namespace HYFrameWork.DAL.SqlServer
{
    partial class SqlServerRepository<T> : IRepository<T>
    {
        public int Delete(T entity)
        {
            var cmd = SqlBuilder<T>.BuildDeleteCommand(entity);
            return DbDelete(cmd, null);
        }

        public void Delete(T entity, IUnitTransaction tran)
        {
            var cmd = SqlBuilder<T>.BuildDeleteCommand(entity);
            ((UnitTransaction)tran).Register(t => DbDelete(cmd, t), _conn);
        }
        public int Delete(Expression<Func<T, bool>> predicate)
        {
            var cmd = SqlBuilder<T>.BuildDeleteCommand(predicate);
            return DbDelete(cmd, null);
        }

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
