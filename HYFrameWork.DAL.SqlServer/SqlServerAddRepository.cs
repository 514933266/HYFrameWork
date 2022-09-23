using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Dapper;
using HYFrameWork.Core;

namespace HYFrameWork.DAL.SqlServer
{

    partial class SqlServerRepository<T> : IRepository<T>
    {
        #region 单个
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>KeyMode为Identity返回自增Id，其余返回受影响行数</returns>
        public int Add(T entity)
        {
            var cmd = SqlBuilder<T>.BuildAddCommand(entity, KeyMode);
            return DbAdd(cmd, null);
        }
        /// <summary>
        /// 新增实体(事务)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="tran">单元事务</param>
        public void Add(T entity, IUnitTransaction tran)
        {
            var cmd = SqlBuilder<T>.BuildAddCommand(entity, KeyMode);
            ((UnitTransaction)tran).Register(t => DbAdd(cmd, t), _conn);
        }
        /// <summary>
        /// 新增实体（符合查询条件的记录为空才会新增）
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="predicate">插入条件</param>
        /// <returns>KeyMode为Identity返回自增Id，其余返回受影响行数</returns>
        public int AddIfNotExists(T entity, Expression<Func<T, bool>> predicate)
        {
            var cmd = SqlBuilder<T>.BuildAddIfNotExistsCommand(entity, predicate, KeyMode);
            return DbAddIfNotExists(cmd, null);
        }

        /// <summary>
        /// 新增实体（符合查询条件的记录为空才会新增）(事务)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="predicate">插入条件</param>
        /// <param name="tran">事务</param>
        public void AddIfNotExists(T entity, Expression<Func<T, bool>> predicate, IUnitTransaction tran)
        {
            var cmd = SqlBuilder<T>.BuildAddIfNotExistsCommand(entity, predicate, KeyMode);
            ((UnitTransaction)tran).Register(t => DbAddIfNotExists(cmd, t), _conn);
        }

        private int DbAdd(SqlCommand cmd, IDbTransaction tran)
        {
            if (KeyMode == PrimaryKeyMode.IDENTITY)
            {
                return _conn.ExecuteScalar<int>(cmd.Sql, cmd.Parameters, tran);
            }
            else
            {
                return _conn.Execute(cmd.Sql, cmd.Parameters, tran);
            }
        }

        private int DbAddIfNotExists(SqlCommand cmd, IDbTransaction tran)
        {
            return _conn.Execute(cmd.Sql, cmd.Parameters, tran);
        }
        #endregion

        #region 批量
        /// <summary>
        /// 批量新增实体（大批量插入时建议使用）
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>影响行数</returns>
        public int AddList(IEnumerable<T> entities)
        {
            int count = 0;
            if (entities != null && entities.Any())
            {
                using (var bulkCopy = new SqlBulkCopy((SqlConnection)_conn, SqlBulkCopyOptions.Default, null))
                {
                    _conn.Open();
                    bulkCopy.BatchSize = entities.Count();
                    bulkCopy.DestinationTableName = "[{0}]".Fmt(typeof(T).Name);
                    var table = new DataTable();
                    var props = SqlBuilder<T>.EffectiveFields;
                    foreach (var propertyInfo in props)
                    {
                        bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                        table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
                    }
                    var values = new object[props.Length];
                    foreach (var item in entities)
                    {
                        for (var i = 0; i < values.Length; i++)
                        {
                            values[i] = props[i].GetValue(item);
                        }
                        table.Rows.Add(values);
                    }
                    count = table.Rows.Count;
                    bulkCopy.WriteToServer(table);
                }
            }
            return count;
        }
        /// <summary>
        /// 批量新增实体(事务)
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <param name="tran">单元事务</param>
        public void AddList(IEnumerable<T> entities, IUnitTransaction tran)
        {
            var cmd = SqlBuilder<T>.BuildAddCommand(entities);
            ((UnitTransaction)tran).Register(t => DbAddList(cmd, t), _conn);
        }

        /// <summary>
        /// 新增实体列表（符合查询条件的记录为空才会新增）(事务)
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <param name="predicate">判断条件方法</param>
        /// <param name="tran">事务</param>
        public void AddListIfNotExists(IEnumerable<T> entities, Func<T, Expression<Func<T, bool>>> predicate, IUnitTransaction tran)
        {
            foreach (var entity in entities)
            {
                var cmd = SqlBuilder<T>.BuildAddIfNotExistsCommand(entity, predicate(entity),KeyMode);
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
