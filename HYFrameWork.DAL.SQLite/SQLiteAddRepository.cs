using HYFrameWork.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dapper;
using System.Data;

namespace HYFrameWork.DAL.SQLite
{

   partial class SQLiteRepository<T> : IRepository<T>
    {
        #region 单个
        public int Add(T entity)
        {
            var cmd = SqlBuilder<T>.BuildAddCommand(entity);
            return DbAdd(cmd, null);
        }
        public void Add(T entity, IUnitTransaction tran)
        {
            var cmd = SqlBuilder<T>.BuildAddCommand(entity);
            ((UnitTransaction)tran).Register(t => DbAdd(cmd,t), _conn);
        }

        public int AddIfNotExists(T entity, Expression<Func<T, bool>> predicate)
        {
            var cmd = SqlBuilder<T>.BuildAddIfNotExistsCommand(entity, predicate);
            return DbAddIfNotExists(cmd, null);
        }
        public void AddIfNotExists(T entity, Expression<Func<T, bool>> predicate, IUnitTransaction tran)
        {
            var cmd = SqlBuilder<T>.BuildAddIfNotExistsCommand(entity, predicate);
            ((UnitTransaction)tran).Register(t => DbAddIfNotExists(cmd,t), _conn);
        }
        private int DbAdd(SqlCommand cmd, IDbTransaction tran)
        {
            return _conn.Execute(cmd.Sql, cmd.Parameters, tran);
        }
        private int DbAddIfNotExists(SqlCommand cmd, IDbTransaction tran)
        {
            return _conn.Execute(cmd.Sql, cmd.Parameters, tran);
        }
        #endregion

        #region 批量
        public int AddList(IEnumerable<T> entities)
        {
            if (entities != null && entities.Any())
            {
                var cmd = SqlBuilder<T>.BuildAddCommand(entities);
                return DbAdd(cmd, null);
            }
            return 0;
        }

        public void AddList(IEnumerable<T> entities, IUnitTransaction tran)
        {
            foreach(var entity in entities)
            {
                var cmd = SqlBuilder<T>.BuildAddCommand(entity);
                ((UnitTransaction)tran).Register(t => DbAddList(cmd, t), _conn);
            }
        }

        public void AddListIfNotExists(IEnumerable<T> entities, Func<T, Expression<Func<T, bool>>> predicate, IUnitTransaction tran)
        {
            foreach (var entity in entities)
            {
                var cmd = SqlBuilder<T>.BuildAddIfNotExistsCommand(entity, predicate(entity));
                ((UnitTransaction)tran).Register(t => DbAddList(cmd, t), _conn);
            }
        }
        private int DbAddList(SqlCommand cmd, IDbTransaction tran)
        {
           return _conn.Execute(cmd.Sql, cmd.Parameters, tran);
        }

        #endregion
    }
}
