using System;
using System.Data;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 单元事务接口
    /// </summary>
    public interface IUnitTransaction : IDisposable
    {
        /// <summary>
        /// 注册事务操作
        /// </summary>
        void Register(Func<IDbTransaction, int> action, IDbConnection conn);

        /// <summary>
        /// 提交单元事务(除TransactionType.Local类型外，其余需要手动捕获异常)
        /// </summary>
        /// <param name="tranType">事务提交方式</param>
        /// <returns>事务方法执行后的int返回总值</returns>
        long Commit(TransactionType tranType = TransactionType.Local);

        /// <summary>
        /// 执行事务的异常
        /// </summary>
        Exception Excetion { get; }
        /// <summary>
        /// 指示事务是否已被提交
        /// </summary>
        bool Commited { get; }
        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollBack();

        /// <summary>
        /// 释放事务和连接对象
        /// </summary>
        new void Dispose();
    }
}
