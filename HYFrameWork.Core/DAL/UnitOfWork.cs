using System;
using System.Collections.Generic;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 工作单元：实现对Repository的统一调度 不支持事务嵌套
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        #region 构造/字段/属性
        private List<IUnitTransaction> _transactions;// 单元事务集合
        private TransactionType _transactionType;
        private bool _commited;
        private long _effected = 0;
        /// <summary>
        /// 工作单元执行过程中发生的异常
        /// </summary>
        public Exception Excetion { get; set; }
        /// <summary>
        /// 事务类型
        /// </summary>
        public TransactionType TransactionType { get { return _transactionType; } }
        /// <summary>
        ///构造：初始化事务集合并设置事务类型
        /// </summary>
        /// <param name="tranType"></param>
        public UnitOfWork(TransactionType tranType=TransactionType.LocalDistribute)
        {
            _transactionType = tranType;
            _transactions = new List<IUnitTransaction>();
        }

        #endregion

        #region 方法
        public IUnitTransaction BeginTransaction()
        {
            var trans = new UnitTransaction(this);
            _transactions.Add(trans);
            return trans;
        }
        public new void Dispose()
        {
            if (_transactions != null)
            {
                _transactions.ForEach(t =>
                {
                    t.Dispose();
                });
                _transactions.Clear();
            }
        }
        /// <summary>
        /// 提交工作单元中的事务
        /// </summary>
        public long Commit()
        {
            if (_commited)
            {
                throw new InvalidOperationException("禁止重复提交工作单元!");
            }
            else
            {
                _commited = true;
                switch (_transactionType)
                {
                    case TransactionType.Local:
                    case TransactionType.Distribute: CommitLocal();break;
                    case TransactionType.LocalDistribute: CommitForLocalDistributedTran();break;
                    default:throw new Exception("不支持的事务类型！");
                }
            }
            return _effected;
        }

        //提交
        private void CommitLocal()
        {
            _transactions.ForEach(t =>
            {
                if (!t.Commited)
                {
                    _effected += t.Commit(_transactionType);
                }
            });
        }

        private void CommitForLocalDistributedTran()
        {
            try
            {
                //伪提交
                _transactions.ForEach(t =>
                {
                    if (!t.Commited)
                    {
                        var effected = t.Commit(TransactionType.LocalDistribute);
                    }
                });
                // 真提交
                _transactions.ForEach(t =>
                {
                    if (t.Commited)
                    {
                        _effected += t.Commit(TransactionType.LocalDistribute);
                    }
                });
            }
            catch (Exception ex)
            {
                _effected = 0;
                Excetion = ex;
                RollBack();
            }
        }

        public void RollBack()
        {
            if (_transactions != null)
            {
                _transactions.ForEach(t =>
                {
                    if (t.Commited) t.RollBack();
                });
            }
        }
        #endregion
    }

}
