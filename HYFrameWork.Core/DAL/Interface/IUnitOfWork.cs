using System;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork:IDisposable
    {

        /// <summary>
        /// 开启单元事务
        /// </summary>
        IUnitTransaction BeginTransaction();

        /// <summary>
        /// 提交工作单元中的事务
        /// </summary>
        /// <returns>影响值</returns>
        long Commit();
        /// <summary>
        /// 执行事务的异常
        /// </summary>
        Exception Excetion { get; }
        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollBack();

        /// <summary>
        /// 释放工作单元
        /// </summary>
        new void Dispose();
    }
}
