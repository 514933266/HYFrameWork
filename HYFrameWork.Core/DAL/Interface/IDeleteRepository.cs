using System;
using System.Linq.Expressions;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 数据库仓储模式：删除接口
    /// </summary>
    public partial interface IRepository<T> 
    {
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>影响行数</returns>
        int Delete(T entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="tran">单元事务</param>
        void Delete(T entity, IUnitTransaction tran);

        /// <summary>
        /// 按条件删除实体
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>影响行数</returns>
        int Delete(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 按条件删除实体
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="tran">单元事务</param>
        void Delete(Expression<Func<T, bool>> predicate, IUnitTransaction tran);

    }
}
