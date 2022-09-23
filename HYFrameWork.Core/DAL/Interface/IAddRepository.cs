using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 数据库仓储模式：新增接口
    /// </summary>
    public partial interface IRepository<T>
    {

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>KeyMode为Identity返回自增Id，其余返回受影响行数</returns>
        int Add(T entity);

        /// <summary>
        /// 新增实体(事务)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="tran">单元事务</param>
        void Add(T entity, IUnitTransaction tran);

        /// <summary>
        /// 新增实体（符合查询条件的记录为空才会新增）
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="predicate">插入条件</param>
        /// <returns>KeyMode为Identity返回自增Id，其余返回受影响行数</returns>
        int AddIfNotExists(T entity, Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 新增实体（符合查询条件的记录为空才会新增）(事务)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="predicate">插入条件</param>
        /// <param name="tran">事务</param>
        void AddIfNotExists(T entity, Expression<Func<T, bool>> predicate, IUnitTransaction tran);

        /// <summary>
        /// 批量新增实体
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>影响行数</returns>
        int AddList(IEnumerable<T> entities);

        /// <summary>
        /// 批量新增实体(事务)
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <param name="tran">单元事务</param>
        void AddList(IEnumerable<T> entities, IUnitTransaction tran);

        /// <summary>
        /// 新增实体列表（符合查询条件的记录为空才会新增）(事务)
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <param name="predicate">判断条件方法</param>
        /// <param name="tran">事务</param>
        void AddListIfNotExists(IEnumerable<T> entities, Func<T, Expression<Func<T, bool>>> predicate, IUnitTransaction tran);

    }
}
