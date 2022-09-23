using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 数据库仓储模式：修改接口
    /// </summary>
    public partial interface IRepository<T>
    {
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>影响行数</returns>
        int Update(T entity);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="tran">单元事务</param>
        void Update(T entity, IUnitTransaction tran);

        /// <summary>
        /// 按条件更新实体
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="updater">更新字段 例：u => new User{ Age = 31, IsActive = true }</param>
        /// <returns>影响行数</returns>
        int Update(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updater);

        /// <summary>
        /// 按条件更新实体
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="updater">更新字段 例：u => new User{ Age = 31, IsActive = true }</param>
        /// <param name="tran">单元事务</param>
        void Update( Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updater, IUnitTransaction tran);

        /// <summary>
        /// 更新实体并返回新行（随机）
        /// </summary>
        /// <typeparam name="TResult">返回对象类型</typeparam>
        /// <param name="updater">更新选择器</param>
        /// <param name="predicate">更新条件</param>
        /// <param name="selector">查询选择器</param>
        /// <param name="top">更新数据量</param>
        /// <param name="isInserted">True返回更新后的数据，False返回更新前的数据</param>
        List<TResult> UpdateSelect<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, T>> updater,
            Expression<Func<T, TResult>> selector,
            int top,
            bool isInserted = true
            );
    }
}
