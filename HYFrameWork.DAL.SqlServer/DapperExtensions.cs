using HYFrameWork.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Linq.Expressions;

namespace HYFrameWork.DAL.SqlServer
{
    /// <summary>
    /// 基于Dapper语义的扩展方法
    /// </summary>
    public static class DapperExtensions
    {
        /// <summary>
        /// 插入一条实体数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rep">仓储对象</param>
        /// <param name="entity">实体</param>
        /// <returns>受影响行数</returns>
        public static int Insert<T>(this IRepository<T> rep, T entity)
        {
            return DbInsert(rep, entity, null);
        }
        /// <summary>
        /// 插入一条实体数据（事务）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rep">仓储对象</param>
        /// <param name="entity">实体</param>
        /// <param name="tran">事务对象</param>
        public static void Insert<T>(this IRepository<T> rep, T entity,IUnitTransaction tran)
        {
            ((UnitTransaction)tran).Register(t => DbInsert(rep, entity,t), rep.GetConnection(false));
        }
        private static int DbInsert<T>(IRepository<T> rep, T entity, IDbTransaction tran)
        {
            var sql = SqlBuilder<T>.DapperInsertSql();
            return rep.GetConnection(false).Execute(sql, entity, tran);
        }

        /// <summary>
        /// 批量插入实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rep">仓储对象</param>
        /// <param name="entities">实体集合</param>
        /// <returns>受影响行数</returns>
        public static int Insert<T>(this IRepository<T> rep, IEnumerable<T> entities)
        {
            return DbInsert(rep,entities,null);
        }
        /// <summary>
        /// 插入一条实体数据（事务）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rep">仓储对象</param>
        /// <param name="entities">实体集合</param>
        /// <param name="tran">事务对象</param>
        public static void Insert<T>(this IRepository<T> rep, IEnumerable<T> entities, IUnitTransaction tran)
        {
            ((UnitTransaction)tran).Register(t => DbInsert(rep, entities, t), rep.GetConnection(false));
        }
        private static int DbInsert<T>(IRepository<T> rep, IEnumerable<T> entities, IDbTransaction tran)
        {
            var sql = SqlBuilder<T>.DapperInsertSql();
            return rep.GetConnection(false).Execute(sql, entities, tran);
        }

        /// <summary>
        /// Select In 查询
        /// </summary>
        /// <typeparam name="T">表对象类型</typeparam>
        /// <typeparam name="TResult">查询的结果类型</typeparam>
        /// <typeparam name="TTarget">IN条件的指定对象类型</typeparam>
        /// <param name="rep">仓储对象</param>
        /// <param name="target">IN条件指定对象属性</param>
        /// <param name="arr">IN条件参数数组</param>
        /// <param name="orderby">排序</param>
        /// <param name="selector">查询字段选择器</param>
        /// <param name="top">Top数量</param>
        /// <param name="dbLock">数据库锁</param>
        /// <returns>结果集合</returns>
        public static IEnumerable<TResult> SelectIn<T, TResult, TTarget>(
            this IRepository<T> rep,
            Expression<Func<T, TTarget>> target,
            TTarget[] arr,
            Func<DbSort<T>, DbSort<T>> orderby = null,
            Expression<Func<T, TResult>> selector = null,
            int top = 0,
            string dbLock = DbLock.Default)
        {
            var sql = SqlBuilder<T>.DapperSelectInSql(target, orderby, selector, top, dbLock);
            return rep.GetConnection(false).Query<TResult>(sql, new { ins = arr });
        }

    }
}
