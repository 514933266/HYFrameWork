using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 数据库仓储模式：查询接口
    /// </summary>
    public partial interface IRepository<T>
    {
        #region 其他查询
        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="dbLock">数据库锁</param>
        /// <param name="readOnly">是否读从库</param>
        int Count(
            Expression<Func<T, bool>> predicate,
            string dbLock = DbLock.Default,
            bool readOnly = false);

        /// <summary>
        /// 查询记录是否存在
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="dbLock">数据库锁</param>
        /// <param name="readOnly">是否读从库</param>
        bool Exists(
            Expression<Func<T, bool>> predicate,
            string dbLock = DbLock.Default,
            bool readOnly = false);
        #endregion

        #region  查询单个实体

        /// <summary>
        /// 查询个实体
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="dbLock">数据库锁</param>
        /// <param name="readOnly">是否读从库</param>
        T Get(
            Expression<Func<T, bool>> predicate,
            string dbLock = DbLock.Default,
            bool readOnly = false);

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="orderby">排序 例：o=>o.Desc(u=>u.CreateTime)</param>
        /// <param name="dbLock">数据库锁</param>
        /// <param name="readOnly">是否读从库</param>
        T Get(
            Expression<Func<T, bool>> predicate,
            Func<DbSort<T>, DbSort<T>> orderby,
            string dbLock = DbLock.Default,
            bool readOnly = false);
        /// <summary>
        /// 查询单条记录（可选字段）
        /// </summary>
        /// <param name="selector">选择查询的字段
        /// <param name="predicate">查询条件</param>
        /// <para>例一: u=>u.UserId   例二: u=>new{ u.UserId, u.UserName}</para>  
        /// <para>例三: u=>new User() 例四:  u=>new object{ u.UserName,u.Id}</para> 
        /// </param>
        /// <param name="dbLock">数据库锁</param>
        /// <param name="readOnly">是否读从库</param>
        TResult GetEx<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            string dbLock = DbLock.Default,
            bool readOnly = false
            );
        /// <summary>
        /// 查询单条记录（可选字段）
        /// </summary>
        /// <param name="selector">选择查询的字段
        /// <param name="predicate">查询条件</param>
        /// <para>例一: u=>u.UserId   例二: u=>new{ u.UserId, u.UserName}</para>  
        /// <para>例三: u=>new User() 例四:  u=>new object{ u.UserName,u.Id}</para> 
        /// </param>
        /// <param name="orderby">排序 例：o=>o.Desc(u=>u.CreateTime)</param>
        /// <param name="dbLock">数据库锁 例：DbLock.NoLock</param>
        /// <param name="readOnly">是否读从库</param>
        TResult GetEx<TResult>(
            Expression<Func<T, bool>> predicate,
            Func<DbSort<T>, DbSort<T>> orderby,
            Expression<Func<T, TResult>> selector,
            string dbLock = DbLock.Default,
            bool readOnly = false
            );
        #endregion

        #region 查询实体列表

        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="dbLock">数据库锁</param>
        /// <param name="readOnly">是否读取从库</param>
        List<T> GetList(
            Expression<Func<T, bool>> predicate,
            string dbLock = DbLock.Default,
            bool readOnly = false
            );

        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="orderby">排序 例：o=>o.Desc(u=>u.CreateTime)</param>
        /// <param name="dbLock">数据库锁</param>
        /// <param name="readOnly">是否读取从库</param>
        List<T> GetList(
            Expression<Func<T, bool>> predicate,
            Func<DbSort<T>, DbSort<T>> orderby,
            string dbLock = DbLock.Default,
            bool readOnly = false
            );
        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="top">最多返回几条记录</param>
        /// <param name="dbLock">数据库锁</param>
        /// <param name="readOnly">是否读取从库</param>
        List<T> GetList(
            Expression<Func<T, bool>> predicate,
            int top,
            string dbLock = DbLock.Default,
            bool readOnly = false
            );

        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="top">最多返回几条记录</param>
        /// <param name="orderby">排序 例：o=>o.Desc(u=>u.CreateTime)</param>
        /// <param name="dbLock">数据库锁</param>
        /// <param name="readOnly"></param>
        List<T> GetList(
            Expression<Func<T, bool>> predicate,
            Func<DbSort<T>, DbSort<T>> orderby,
            int top,
            string dbLock = DbLock.Default,
            bool readOnly = false
            );

        /// <summary>
        /// 查询记录列表
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="selector">查询哪些字段（例一: u=>u.UserId 例二: u=>new{ u.UserId, u.UserName}）</param>
        /// <param name="dbLock">数据库锁</param>
        /// <param name="readOnly">是否读从库</param>
        List<TResult> GetListEx<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            string dbLock = DbLock.Default,
            bool readOnly = false
            );

        /// <summary>
        /// 查询记录列表（可选字段）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="selector">查询哪些字段（例一: u=>u.UserId 例二: u=>new{ u.UserId, u.UserName}）</param>
        /// <param name="orderby">排序 例：o=>o.Desc(u=>u.CreateTime)</param>
        /// <param name="dbLock">数据库锁</param>
        /// <param name="readOnly">是否读从库</param>
        List<TResult> GetListEx<TResult>(
            Expression<Func<T, bool>> predicate,
            Func<DbSort<T>, DbSort<T>> orderby,
            Expression<Func<T, TResult>> selector,
            string dbLock = DbLock.Default,
            bool readOnly = false
            );
        /// <summary>
        /// 查询记录列表（可选字段）
        /// </summary>
        /// <param name="top">最多返回几条记录（0表示不限制）</param>
        /// <param name="selector">查询哪些字段（例一: u=>u.UserId 例二: u=>new{ u.UserId, u.UserName}）</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="dbLock">数据库锁</param>
        /// <param name="readOnly">是否读从库</param>
        List<TResult> GetListEx<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            int top,
            string dbLock = DbLock.Default,
            bool readOnly = false
            );

        /// <summary>
        /// 查询记录列表
        /// </summary>
        /// <param name="top">最多返回几条记录（0表示不限制）</param>
        /// <param name="selector">查询哪些字段（例一: u=>u.UserId 例二: u=>new{ u.UserId, u.UserName}）</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="orderby">排序 例：o=>o.Desc(u=>u.CreateTime)</param>
        /// <param name="dbLock">数据库锁</param>
        /// <param name="readOnly">是否读从库</param>
        List<TResult> GetListEx<TResult>(
            Expression<Func<T, bool>> predicate,
            Func<DbSort<T>, DbSort<T>> orderby,
            Expression<Func<T, TResult>> selector,
            int top,
            string dbLock = DbLock.Default,
            bool readOnly = false
            );
        #endregion

        #region  查询分页数据

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="pageIndex">查询第几页(1为首页)</param>
        /// <param name="pageSize">每页记录数</param>
        ///  <param name="dbLock">数据库锁</param>
        /// <param name="readOnly">是否读从库</param>
        /// <returns></returns>
        PageList<T> PageList(
            Expression<Func<T, bool>> predicate,
            int pageIndex,
            int pageSize,
            string dbLock = DbLock.Default,
            bool readOnly = false
            );

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="orderby">排序 例：o=>o.Desc(u=>u.CreateTime)</param>
        /// <param name="pageIndex">查询第几页(1为首页)</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="dbLock">数据库锁 例：DbLock.NoLock</param>
        /// <param name="readOnly">是否读从库</param>
        PageList<T> PageList(
            Expression<Func<T, bool>> predicate,
            Func<DbSort<T>, DbSort<T>> orderby,
            int pageIndex,
            int pageSize,
            string dbLock = DbLock.Default,
            bool readOnly = false
            );
        /// <summary>
        /// 查询分页记录
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="selector">查询哪些字段（例一: u=>u.UserId 例二: u=>new{ u.UserId, u.UserName}）</param>
        /// <param name="pageIndex">查询第几页(1为首页)</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="dbLock">数据库锁 例：DbLock.NoLock</param>
        /// <param name="readOnly">是否读从库</param>
        PageList<TResult> PageListEx<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            int pageIndex,
            int pageSize,
            string dbLock = DbLock.Default,
            bool readOnly = false
            );

        /// <summary>
        /// 查询分页记录
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="selector">查询哪些字段（例一: u=>u.UserId 例二: u=>new{ u.UserId, u.UserName}）</param>
        /// <param name="orderby">排序 排序 例：CreateTime DESC</param>
        /// <param name="pageIndex">查询第几页(1为首页)</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="dbLock">数据库锁 例：DbLock.NoLock</param>
        /// <param name="readOnly">是否读从库</param>
        PageList<TResult> PageListEx<TResult>(
            Expression<Func<T, bool>> predicate,
            Func<DbSort<T>, DbSort<T>> orderby,
            Expression<Func<T, TResult>> selector,
            int pageIndex,
            int pageSize,
            string dbLock,
            bool readOnly
            );
        #endregion

        #region SQL 查询

        /// <summary>
        /// 执行 SQL 语句查询
        /// </summary>
        /// <typeparam name="TResult">返回对象类型</typeparam>
        /// <param name="sql">要执行的SQL语句</param>
        /// <param name="parms">SQL参数，例： new {name=="test1",mobile="138888888"}</param>
        /// <param name="readOnly">是否读从库</param>
        /// <returns>查询结果对象枚举</returns>
        IEnumerable<TResult> Query<TResult>(string sql, object parms, bool readOnly = false);

        /// <summary>
        /// 执行 SQL 语句查询
        /// </summary>
        /// <typeparam name="T1">返回对象类型1</typeparam>
        /// <typeparam name="T2">返回对象类型2</typeparam>
        /// <param name="sql">要执行的SQL语句</param>
        /// <param name="parms">SQL参数，如 new {name=="test1",mobile="138888888"}</param>
        /// <param name="readOnly">是否读从库</param>
        /// <returns>查询结果对象枚举</returns> 
        List<IEnumerable> Query<T1, T2>(string sql, object parms, bool readOnly = false);

        /// <summary>
        /// 执行 SQL 语句查询
        /// </summary>
        /// <typeparam name="T1">返回对象类型1</typeparam>
        /// <typeparam name="T2">返回对象类型2</typeparam>
        /// <typeparam name="T3">返回对象类型3</typeparam>
        /// <param name="sql">要执行的SQL语句</param>
        /// <param name="parms">SQL参数，如 new {name=="test1",mobile="138888888"}</param>
        /// <param name="readOnly">是否读从库</param>
        /// <returns>查询结果对象枚举</returns>
        List<IEnumerable> Query<T1, T2, T3>(string sql, object parms, bool readOnly = false);

        /// <summary>
        /// 执行 SQL 语句查询
        /// </summary>
        /// <typeparam name="T1">返回对象类型1</typeparam>
        /// <typeparam name="T2">返回对象类型2</typeparam>
        /// <typeparam name="T3">返回对象类型3</typeparam>
        /// <typeparam name="T4">返回对象类型4</typeparam>
        /// <param name="sql">要执行的SQL语句</param>
        /// <param name="parms">SQL参数，如 new {name=="test1",mobile="138888888"}</param>
        /// <param name="readOnly">是否读从库</param>
        /// <returns>查询结果对象枚举</returns>
        List<IEnumerable> Query<T1, T2, T3, T4>(string sql, object parms, bool readOnly = false);

        /// <summary>
        /// 执行 SQL 语句查询
        /// </summary>
        /// <typeparam name="T1">返回对象类型1</typeparam>
        /// <typeparam name="T2">返回对象类型2</typeparam>
        /// <typeparam name="T3">返回对象类型3</typeparam>
        /// <typeparam name="T4">返回对象类型4</typeparam>
        /// <typeparam name="T5">返回对象类型5</typeparam>
        /// <param name="sql">要执行的SQL语句</param>
        /// <param name="parms">SQL参数，如 new {name=="test1",mobile="138888888"}</param>
        /// <param name="readOnly">是否读从库</param>
        /// <returns>查询结果对象枚举</returns>  
        List<IEnumerable> Query<T1, T2, T3, T4, T5>(string sql, object parms, bool readOnly = false);

        /// <summary>
        /// 执行 SQL 语句查询单个对象
        /// </summary>
        /// <typeparam name="TResult">返回对象类型</typeparam>
        /// <param name="sql">要执行的SQL语句</param>
        /// <param name="parms">SQL参数，例： new {name=="test1",mobile="138888888"}</param>
        /// <param name="readOnly">是否读从库</param>
        /// <returns>查询结果对象</returns>
        TResult ExecuteScalar<TResult>(string sql, object parms, bool readOnly = false);

        #endregion

    }
}
